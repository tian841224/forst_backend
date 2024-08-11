using admin_backend.Data;
using admin_backend.DTOs.News;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class NewsService : INewsService
    {
        private readonly ILogger<NewsService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IOperationLogService> _operationLogService;

        public NewsService(ILogger<NewsService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IOperationLogService> operationLogService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _operationLogService = operationLogService;
        }

        public async Task<PagedResult<NewsResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<News> news = _context.News;

            if (Id != null)
                news = news.Where(x => x.Id == Id);

            var newsResponse = _mapper.Map<List<NewsResponse>>(news);
            foreach (var item in newsResponse)
            {
                var adminUserName = await _context.AdminUser.Where(x => x.Id == item.AdminUserId).Select(x => x.Name).FirstOrDefaultAsync();
                if (adminUserName == null) continue;

                item.AdminUserName = adminUserName;
            }

            //分頁處理
            return newsResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<NewsResponse>> Get(GetNewsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<News> news = _context.News;

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                news = news.Where(x =>
                    x.Title.ToLower().Contains(keyword) ||
                    x.Id.ToString().Contains(keyword)
                );
            }

            if (dto.Status.HasValue)
            {
                news = news.Where(x => x.Status == dto.Status);
            }

            if (dto.Type.HasValue)
            {
                news = news.Where(x => x.Type == dto.Type);
            }

            var newsResponse = _mapper.Map<List<NewsResponse>>(news);
            //分頁處理
            return newsResponse.GetPaged(dto.Page!);
        }
        public async Task<NewsResponse> Add(AddNewsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var news = new News
            {
                AdminUserId = dto.AdminUserId,
                Content = dto.Content,
                Pinned = dto.Pinned,
                Schedule = dto.Schedule,
                Status = dto.Status,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Title = dto.Title,
                Type = dto.Type,
                WebsiteReleases = dto.WebsiteReleases,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.News.AddAsync(news);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增最新消息：{news.Title}",
                });
            };
            scope.Complete();
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> Update(int Id, UpdateNewsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var news = await _context.News.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (news == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (string.IsNullOrEmpty(dto.Title))
            {
                news.Title = dto.Title ?? string.Empty;
            }

            if (dto.Type.HasValue)
            {
                news.Type = dto.Type.Value;
            }

            if (string.IsNullOrEmpty(dto.Content))
            {
                news.Content = dto.Content ?? string.Empty;
            }

            if (dto.Pinned.HasValue)
            {
                news.Pinned = dto.Pinned.Value;
            }

            if (dto.WebsiteReleases.HasValue)
            {
                news.WebsiteReleases = dto.WebsiteReleases.Value;
            }

            if (dto.Schedule.HasValue)
            {
                news.Schedule = dto.Schedule.Value;
            }

            if (dto.StartTime.HasValue)
            {
                news.StartTime = dto.StartTime.Value;
            }

            if (dto.EndTime.HasValue)
            {
                news.EndTime = dto.EndTime.Value;
            }

            if (dto.Status.HasValue)
            {
                news.Status = dto.Status.Value;
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.News.Update(news);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改最新消息：{news.Id}/{news.Title}",
                });
            };
            scope.Complete();
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var news = await _context.News.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (news == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.News.Remove(news);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除最新消息：{news.Title}",
                });
            };
            scope.Complete();
            return _mapper.Map<NewsResponse>(news);
        }
    }
}
