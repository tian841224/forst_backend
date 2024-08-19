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
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace admin_backend.Services
{
    public class NewsService : INewsService
    {
        private readonly ILogger<NewsService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IAdminUserServices _adminUserServices;
        private readonly ICommonServicecs _commonServicecs;

        public NewsService(ILogger<NewsService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IOperationLogService> operationLogService, IAdminUserServices adminUserServices, ICommonServicecs commonServicecs)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _operationLogService = operationLogService;
            _adminUserServices = adminUserServices;
            _commonServicecs = commonServicecs;
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
                item.AdminUserName = await _commonServicecs.GetAdminUserNameAsync(item.AdminUserId);
                item.WebsiteReleases = news.FirstOrDefault(x => x.Id == item.Id)?.WebsiteReleases ?? new List<WebsiteEnum>();
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

            var newsResponses = new List<NewsResponse>();

            foreach (var item in await news.ToListAsync())
            {
                var adminUserName = await _context.AdminUser.Where(x => x.Id == item.AdminUserId).Select(x => x.Name).FirstOrDefaultAsync();
                if (adminUserName == null) continue;

                var newsResponse = new NewsResponse
                {
                    Id = item.Id,
                    AdminUserId = item.AdminUserId,
                    AdminUserName = await _commonServicecs.GetAdminUserNameAsync(item.AdminUserId),
                    Title = item.Title,
                    Type = item.Type,
                    Content = item.Content,
                    Pinned = item.Pinned,
                    Schedule = item.Schedule,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = item.Status,
                    WebsiteReleases = item.WebsiteReleases,
                    UpdateTime = item.UpdateTime,
                    CreateTime = item.CreateTime,
                };

                newsResponses.Add(newsResponse);
            }

            //分頁處理
            return newsResponses.GetPaged(dto.Page!);
        }
        public async Task<NewsResponse> Add(AddNewsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var news = await _context.News.Where(x => x.Title == dto.Title).FirstOrDefaultAsync();

            if (news != null)
            {
                throw new ApiException($"此標題已存在-{dto.Title}");
            }

            //取得當前使用者身分
            var adminUser = await _adminUserServices.Get();
            if (adminUser == null)
            { throw new ApiException($"無法取得當前使用者身分"); }

            if (!DateTime.TryParse(dto.StartTime, out DateTime StartTime))
            {
                throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
            }

            if (!DateTime.TryParse(dto.EndTime, out DateTime EndTime))
            {
                throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
            }

            news = new News
            {
                AdminUserId = adminUser.Id,
                Content = dto.Content,
                Pinned = dto.Pinned,
                Schedule = dto.Schedule,
                Status = dto.Status,
                StartTime = StartTime,
                EndTime = EndTime,
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

            if (!string.IsNullOrEmpty(dto.Title))
            {
                news.Title = dto.Title ?? string.Empty;
            }

            if (dto.Type.HasValue)
            {
                news.Type = dto.Type.Value;
            }

            if (!string.IsNullOrEmpty(dto.Content))
            {
                news.Content = dto.Content ?? string.Empty;
            }

            if (dto.Pinned.HasValue)
            {
                news.Pinned = dto.Pinned.Value;
            }

            if (dto.WebsiteReleases != null && dto.WebsiteReleases.Any())
            {
                news.WebsiteReleases = dto.WebsiteReleases;
            }

            if (dto.Schedule.HasValue)
            {
                news.Schedule = dto.Schedule.Value;
            }

            if (!string.IsNullOrEmpty(dto.StartTime))
            {
                if (!DateTime.TryParse(dto.StartTime, out DateTime StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }
                news.StartTime = StartTime;
            }

            if (!string.IsNullOrEmpty(dto.EndTime))
            {
                if (!DateTime.TryParse(dto.EndTime, out DateTime EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }
                news.EndTime = EndTime;
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
