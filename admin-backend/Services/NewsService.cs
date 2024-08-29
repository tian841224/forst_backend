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

            news = news.Where(x => x.Id == Id);

            //將排程過期的資料改為停用
            var today = DateTime.Today;
            await news
                .Where(x => x.Schedule && (x.StartTime.Date > today || x.EndTime.Date < today))
                .ExecuteUpdateAsync(s => s.SetProperty(b => b.Status, b => StatusEnum.Stop));

            var newsResponse = _mapper.Map<List<NewsResponse>>(news);

            foreach (var item in newsResponse)
            {
                item.AdminUserName = await _commonServicecs.GetAdminUserNameAsync(item.AdminUserId);
                item.WebsiteReleases = news.FirstOrDefault(x => x.Id == item.Id)?.WebsiteReleases ?? new List<WebsiteEnum>();

                if (item.Schedule)
                {
                    item.StartTime = item.StartTime != DateTime.MinValue ? item.StartTime : null;
                    item.EndTime = item.EndTime != DateTime.MinValue ? item.EndTime : null;
                }
                else
                {
                    item.StartTime = null;
                    item.EndTime = null;
                }
            }

            //分頁處理
            return newsResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<NewsResponse>> Get(GetNewsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<News> news = _context.News;

            //將排程過期的資料改為停用
            var today = DateTime.Today;
            await news
                .Where(x => x.Schedule && (x.StartTime.Date > today || x.EndTime.Date < today))
                .ExecuteUpdateAsync(s => s.SetProperty(b => b.Status, b => StatusEnum.Stop));

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

            var newsList = await news.ToListAsync();

            if (dto.WebsiteReleases.HasValue)
            {
                var websiteRelease = dto.WebsiteReleases.Value;
                newsList = newsList.Where(x => x.WebsiteReleases.Any(w => w == dto.WebsiteReleases)).ToList();
            }

            var newsResponses = new List<NewsResponse>();

            foreach (var item in newsList)
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
                    StartTime = item.Schedule ? (item.StartTime != DateTime.MinValue ? item.StartTime : null) : null,
                    EndTime = item.Schedule ? (item.EndTime != DateTime.MinValue ? item.EndTime : null) : null,
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

            DateTime StartTime = DateTime.MinValue;
            DateTime EndTime = DateTime.MinValue;

            if (dto.Schedule)
            {
                if (string.IsNullOrEmpty(dto.StartTime) || string.IsNullOrEmpty(dto.EndTime))
                    throw new ApiException($"若需設定排程，請輸入起始結束時間");

                if (!DateTime.TryParse(dto.StartTime, out StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }

                if (!DateTime.TryParse(dto.EndTime, out EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }
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

            var StartTime = DateTime.MinValue;
            var EndTime = DateTime.MinValue;

            if (dto.Schedule.HasValue && dto.Schedule.Value)
            {
                if (string.IsNullOrEmpty(dto.StartTime) || string.IsNullOrEmpty(dto.EndTime))
                    throw new ApiException($"若需設定排程，請輸入起始結束時間");

                if (!DateTime.TryParse(dto.StartTime, out StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }

                if (!DateTime.TryParse(dto.EndTime, out EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }

                news.StartTime = StartTime;
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
