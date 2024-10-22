using admin_backend.Data;
using admin_backend.DTOs.CaseReport;
using admin_backend.Entities;
using admin_backend.Interfaces;
using Microsoft.CodeAnalysis;
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

        public async Task<List<GroupByDamageClassResponse>> GroupByDamageClass(CaseGroupByDamageClassDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var query = from caseRecord in _context.CaseRecord
                        join diagnosis in _context.CaseDiagnosisResult on caseRecord.Id equals diagnosis.CaseId into caseDiagnosisGroup
                        from caseDiagnosis in caseDiagnosisGroup.DefaultIfEmpty()
                        join commonDamage in _context.CommonDamage on caseDiagnosis.CommonDamageId equals commonDamage.Id into commonDamageGroup
                        from commonDamage in commonDamageGroup.DefaultIfEmpty()
                        join damageClass in _context.DamageClass on commonDamage.DamageClassId equals damageClass.Id into damageClassGroup
                        from damageClass in damageClassGroup.DefaultIfEmpty()
                        select new
                        {
                            caseRecord,
                            commonDamage,
                            DamageClassId = commonDamage.DamageClassId,
                            DamageClassName = damageClass.Name
                        };

            if (!string.IsNullOrEmpty(dto.StartTime) && !string.IsNullOrEmpty(dto.EndTime))
            {
                // 處理時間格式
                if (!DateTime.TryParse(dto.StartTime, out var StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }
                if (!DateTime.TryParse(dto.EndTime, out var EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }
                query = query.Where(x => x.caseRecord.ApplicationDate >= StartTime && x.caseRecord.ApplicationDate < EndTime);
            }

            if (dto.DamageClassId.HasValue)
            {
                query = query.Where(x => x.commonDamage.DamageClassId == dto.DamageClassId.Value);
            }

            var result = await query
                  .GroupBy(x => new { x.DamageClassId, x.DamageClassName })
                  .Select(g => new GroupByDamageClassResponse
                  {
                      DamageClassId = g.Key.DamageClassId,
                      DamageClassName = g.Key.DamageClassName,
                      Count = g.Count()
                  })
                  .ToListAsync();

            return result;
        }

        //public async Task<List<GroupByDamageTypeResponse>> GroupByDamageType(CaseGroupByDamageTypeDto dto)
        //{
        //    await using var _context = await _contextFactory.CreateDbContextAsync();

        //    var query = from caseRecord in _context.CaseRecord
        //                join diagnosis in _context.CaseDiagnosisResult on caseRecord.Id equals diagnosis.CaseId into caseDiagnosisGroup
        //                from caseDiagnosis in caseDiagnosisGroup.DefaultIfEmpty()
        //                join commonDamage in _context.CommonDamage on caseDiagnosis.CommonDamageId equals commonDamage.Id into commonDamageGroup
        //                from commonDamage in commonDamageGroup.DefaultIfEmpty()
        //                join damageType in _context.DamageType on commonDamage.DamageTypeId equals damageType.Id into damageTypeGroup
        //                from damageType in damageTypeGroup.DefaultIfEmpty()
        //                select new
        //                {
        //                    caseRecord,
        //                    commonDamage,
        //                    DamageTypeId = commonDamage.DamageTypeId,
        //                    DamageTypeName = damageType.Name
        //                };

        //    if (!string.IsNullOrEmpty(dto.StartTime) && !string.IsNullOrEmpty(dto.EndTime))
        //    {
        //        // 處理時間格式
        //        if (!DateTime.TryParse(dto.StartTime, out var StartTime))
        //        {
        //            throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
        //        }
        //        if (!DateTime.TryParse(dto.EndTime, out var EndTime))
        //        {
        //            throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
        //        }
        //        query = query.Where(x => x.caseRecord.ApplicationDate >= StartTime && x.caseRecord.ApplicationDate < EndTime);
        //    }

        //    if (dto.DamageTypeId.HasValue)
        //    {
        //        query = query.Where(x => x.commonDamage.DamageTypeId == dto.DamageTypeId.Value);
        //    }

        //    var result = await query
        //          .GroupBy(x => new { x.DamageTypeId, x.DamageTypeName })
        //          .Select(g => new GroupByDamageTypeResponse
        //          {
        //              DamageTypeId = g.Key.DamageTypeId,
        //              DamageTypeName = g.Key.DamageTypeName,
        //              Count = g.Count()
        //          })
        //          .ToListAsync();

        //    return result;
        //}

        public async Task<List<GroupByDamageLocationResponse>> GroupByDamageLocation(CaseGroupByDamageLocationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var query = from caseRecord in _context.CaseRecord
                        join diagnosis in _context.CaseDiagnosisResult on caseRecord.Id equals diagnosis.CaseId into caseDiagnosisGroup
                        from caseDiagnosis in caseDiagnosisGroup.DefaultIfEmpty()
                        join commonDamage in _context.CommonDamage on caseDiagnosis.CommonDamageId equals commonDamage.Id into commonDamageGroup
                        from commonDamage in commonDamageGroup.DefaultIfEmpty()
                        join damageType in _context.DamageType on commonDamage.DamageTypeId equals damageType.Id into damageTypeGroup
                        from damageType in damageTypeGroup.DefaultIfEmpty()
                        join damageClass in _context.DamageClass on commonDamage.DamageClassId equals damageClass.Id into damageClassGroup
                        from damageClass in damageClassGroup.DefaultIfEmpty()
                        select new
                        {
                            caseRecord,
                            commonDamage,
                            DamageTypeId = commonDamage.DamageTypeId,
                            DamageTypeName = damageType.Name,
                            DamageClassId = commonDamage.DamageClassId,
                            DamageClassName = damageClass.Name
                        };

            if (!string.IsNullOrEmpty(dto.StartTime) && !string.IsNullOrEmpty(dto.EndTime))
            {
                // 處理時間格式
                if (!DateTime.TryParse(dto.StartTime, out var StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }
                if (!DateTime.TryParse(dto.EndTime, out var EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }
                query = query.Where(x => x.caseRecord.ApplicationDate >= StartTime && x.caseRecord.ApplicationDate < EndTime);
            }

            if (dto.DamageTypeId.HasValue)
            {
                query = query.Where(x => x.commonDamage.DamageTypeId == dto.DamageTypeId.Value);
            }

            if (dto.DamageClassId.HasValue)
            {
                query = query.Where(x => x.commonDamage.DamageClassId == dto.DamageClassId.Value);
            }

            if (!string.IsNullOrEmpty(dto.County))
            {
                query = query.Where(x => x.caseRecord.County == dto.County);
            }

            var result = await query
                  .GroupBy(x => new { x.caseRecord.County })
                  .Select(g => new GroupByDamageLocationResponse
                  {
                      County = g.Key.County,
                      Count = g.Count()
                  })
                  .ToListAsync();

            return result;
        }

        public async Task<List<GroupByCountyDamageResponse>> GroupByCountyDamage(CaseGroupByCountyDamageDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var query = from caseRecord in _context.CaseRecord
                        join diagnosis in _context.CaseDiagnosisResult on caseRecord.Id equals diagnosis.CaseId into caseDiagnosisGroup
                        from caseDiagnosis in caseDiagnosisGroup.DefaultIfEmpty()
                        select new
                        {
                            caseRecord,
                            caseDiagnosis
                        };

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
                query = query.Where(x => x.caseRecord.ApplicationDate >= StartTime && x.caseRecord.ApplicationDate < EndTime);
            }

            if (dto.CommonDamageId.HasValue)
            {
                query = query.Where(x => x.caseDiagnosis.CommonDamageId == dto.CommonDamageId.Value);
            }

            if (!string.IsNullOrEmpty(dto.County))
            {
                query = query.Where(x => x.caseRecord.County == dto.County);
            }

            var result = query
                .GroupBy(x => x.caseRecord.County)
                .Select(g => new GroupByCountyDamageResponse
                {
                    County = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }

        public async Task<List<GroupByMonthResponse>> GroupByMonth(CaseGroupByMonthDto dto)
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

            if (dto.Type.HasValue && dto.Type.Value == 1)
            {
                //年統計
                return caseEntity
                        .GroupBy(x => new { x.ApplicationDate.Year })
                        .Select(g => new GroupByMonthResponse
                        {
                            Year = g.Key.Year,
                            Count = g.Count()
                        })
                        .ToList();
            }

            var result = caseEntity
                .GroupBy(x => new { x.ApplicationDate.Year, x.ApplicationDate.Month })
                .Select(g => new GroupByMonthResponse
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }

        public async Task<List<GroupByCommonamageResponse>> GroupByCommonamage(CaseGroupByCommonamageDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var query = from caseRecord in _context.CaseRecord
                        join diagnosis in _context.CaseDiagnosisResult on caseRecord.Id equals diagnosis.CaseId into caseDiagnosisGroup
                        from caseDiagnosis in caseDiagnosisGroup.DefaultIfEmpty()
                        join commonDamage in _context.CommonDamage on caseDiagnosis.CommonDamageId equals commonDamage.Id into commonDamageGroup
                        from commonDamage in commonDamageGroup.DefaultIfEmpty()
                        select new
                        {
                            caseRecord,
                            caseDiagnosis,
                            commonDamage,
                        };

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
                query = query.Where(x => x.caseRecord.ApplicationDate >= StartTime && x.caseRecord.ApplicationDate < EndTime);
            }

            var result = query
                .GroupBy(x => new { x.commonDamage.Id, x.commonDamage.Name})
                .Select(g => new GroupByCommonamageResponse
                {
                    CommonDamageId = g.Key.Id,
                    CommonDamageName = g.Key.Name,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }
    }
}
