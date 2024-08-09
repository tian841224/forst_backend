using admin_backend.Data;
using admin_backend.DTOs.DamageType;
using admin_backend.DTOs.FAQ;
using admin_backend.DTOs.News;
using admin_backend.Entities;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class NewsService: INewsService
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
    }
}
