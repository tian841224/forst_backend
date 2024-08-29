using admin_backend.Data;
using admin_backend.DTOs.EpidemicSummary;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class EpidemicSummaryService : IEpidemicSummaryService
    {
        private readonly ILogger<EpidemicSummaryService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;
        public EpidemicSummaryService(ILogger<EpidemicSummaryService> log, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IMapper mapper)
        {
            _log = log;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<EpidemicSummaryResponse> Get()
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var epidemicSummary = await _context.EpidemicSummary.OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new EpidemicSummary { Title = null!, Content = null! };
            return _mapper.Map<EpidemicSummaryResponse>(epidemicSummary);
        }

        public async Task<EpidemicSummaryResponse> Add(AddEpidemicSummaryDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var epidemicSummary = await _context.EpidemicSummary.Where(x => x.Title == dto.Title).FirstOrDefaultAsync();

            epidemicSummary = new EpidemicSummary
            {
                Title = dto.Title,
                Content = dto.Content,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.EpidemicSummary.AddAsync(epidemicSummary);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增疫情簡介{epidemicSummary.Title}",
                });
            }
            scope.Complete();
            return _mapper.Map<EpidemicSummaryResponse>(epidemicSummary);
        }
    }
}
