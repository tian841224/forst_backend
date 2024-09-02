using admin_backend.Data;
using admin_backend.DTOs.CaseReport;
using admin_backend.Entities;
using admin_backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class CaseReportService : ICaseReportService
    {
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;

        public CaseReportService(IDbContextFactory<MysqlDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<GroupByCountyResponse>> GroupByCounty(CaseGroupByCountyDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<CaseRecord> caseEntity = _context.CaseRecord;

            if (!string.IsNullOrEmpty(dto.StartTime) && !string.IsNullOrEmpty(dto.EndTime))
            {
                //處理時間格式
                if (!DateTime.TryParse(dto.StartTime, out var StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }
                if (!DateTime.TryParse(dto.EndTime, out var EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }
                caseEntity = caseEntity.Where(x => x.ApplicationDate >= StartTime && x.ApplicationDate < EndTime);
            }

            if (!string.IsNullOrEmpty(dto.County))
            {
                caseEntity = caseEntity.Where(x => x.County == dto.County);
            }

            var result = caseEntity
                .GroupBy(x => x.County)
                .Select(g => new GroupByCountyResponse
                {
                    County = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }
    }
}
