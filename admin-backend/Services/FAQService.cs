using admin_backend.Data;
using admin_backend.DTOs.EpidemicSummary;
using admin_backend.DTOs.FAQ;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using CommonLibrary.Extensions;
using admin_backend.DTOs.DamageType;


namespace admin_backend.Services
{
    public class FAQService : IFAQService
    {
        private readonly ILogger<DocumentationService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IAdminUserServices _adminUserServices;

        public FAQService(ILogger<DocumentationService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IOperationLogService> operationLogService, IAdminUserServices adminUserServices)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _operationLogService = operationLogService;
            _adminUserServices = adminUserServices;
        }

        public async Task<PagedResult<FAQResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<FAQ> faq = _context.FAQ;

            if (Id != null)
                faq = faq.Where(x => x.Id == Id);

            var faqResponse = _mapper.Map<List<FAQResponse>>(faq);
            foreach (var item in faqResponse)
            {
                var adminUserName = await _context.AdminUser.Where(x => x.Id == item.AdminUserId).Select(x => x.Name).FirstOrDefaultAsync();
                if (adminUserName == null) continue;

                item.AdminUserName = adminUserName;
            }

            //分頁處理
            return faqResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<FAQResponse>> Get(GetFAQDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<FAQ> faq = _context.FAQ;

            if (!string.IsNullOrEmpty(dto.Question))
                faq = faq.Where(x => x.Question.Contains(dto.Question));

            if(dto.Status.HasValue)
                faq =faq.Where(x => x.Status == dto.Status);

            var faqResponse = _mapper.Map<List<FAQResponse>>(faq);
            //分頁處理
            return faqResponse.GetPaged(dto.Page!);
        }

        public async Task<FAQResponse> Add(AddFAQDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            //var adminUser = await _context.AdminUser.Where(x => x.Id == dto.AdminUserId).FirstOrDefaultAsync();
            //if (adminUser == null)
            //    throw new ApiException($"無此後台帳號-{dto.AdminUserId}");

            //取得當前使用者身分
            var adminUser = await _adminUserServices.Get();
            if (adminUser == null)
            { throw new ApiException($"無法取得當前使用者身分"); }

            var fAQ = new FAQ
            {
                AdminUserId = adminUser.Id,
                Question = dto.Question,
                Answer = dto.Answer,
                Status = dto.Status,
                Sort = dto.Sort,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.FAQ.AddAsync(fAQ);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增常見問題{fAQ.Question}",
                });
            }
            scope.Complete();
            return _mapper.Map<FAQResponse>(fAQ);
        }

        public async Task<FAQResponse> Update(int Id, UpdateFAQDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var faq = await _context.FAQ.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if(faq == null)
                throw new ApiException($"無此資料-{Id}");

            if(!string.IsNullOrEmpty(dto.Question))
                faq.Question = dto.Question;

            if (!string.IsNullOrEmpty(dto.Answer))
                faq.Answer = dto.Answer;

            if (dto.Status.HasValue)
                faq.Status = dto.Status.Value;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.FAQ.Update(faq);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改常見問題{faq.Question}",
                });
            }
            scope.Complete();
            return _mapper.Map<FAQResponse>(faq);
        }

        public async Task<List<FAQResponse>> UpdateSort(List<SortBasicDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var result = new List<FAQResponse>();

            foreach (var value in dto)
            {
                var faq = await _context.FAQ.Where(x => x.Id == value.Id).FirstOrDefaultAsync();
                if (faq == null) continue;

                if (value.Sort.HasValue)
                    faq.Sort = value.Sort.Value;

                _context.FAQ.Update(faq);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Value.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改常見問題排序：{faq.Answer}/{faq.Sort}",
                    });
                };

                result.Add(_mapper.Map<FAQResponse>(faq));
            }

            scope.Complete();
            return result;
        }
    }
}
