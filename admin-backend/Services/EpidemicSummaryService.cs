using CommonLibrary.Data;
using CommonLibrary.DTOs.EpidemicSummary;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class EpidemicSummaryService
    {
        private readonly ILogger<EpidemicSummaryService> _log;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        public EpidemicSummaryService(ILogger<EpidemicSummaryService> log, MysqlDbContext context, OperationLogService operationLogService)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
        }

        public async Task<EpidemicSummary> Get()
        {
            var epidemicSummary = await _context.EpidemicSummary.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return epidemicSummary;
        }

        public async Task<EpidemicSummary> Add(AddEpidemicSummaryDto dto)
        {
            var epidemicSummary = new EpidemicSummary
            {
                Title = dto.Title,
                Content = dto.Content,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.EpidemicSummary.AddAsync(epidemicSummary);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增疫情簡介{epidemicSummary.Title}",
                });
            }
            scope.Complete();
            return epidemicSummary;
        }
    }
}
