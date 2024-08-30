using admin_backend.Data;
using admin_backend.DTOs.Case;
using admin_backend.DTOs.CaseDiagnosisResult;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    /// <summary>
    /// 案件回覆
    /// </summary>
    public class CaseDiagnosisResultService : ICaseDiagnosisResultService
    {
        private readonly ILogger<CaseDiagnosisResultService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IMapper _mapper;
        private readonly Lazy<IFileService> _fileService;
        private readonly ICommonDamageService _commonDamageService;

        public CaseDiagnosisResultService(ILogger<CaseDiagnosisResultService> log, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IMapper mapper, Lazy<IFileService> fileService, ICommonDamageService commonDamageService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _mapper = mapper;
            _fileService = fileService;
            _commonDamageService = commonDamageService;
        }

        public async Task<CaseDiagnosisResultResponse> Get(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var caseDiagnosis = await _context.CaseDiagnosisResult.FirstOrDefaultAsync(x => x.Id == Id);

            if (caseDiagnosis == null)
            {
                throw new ApiException($"找不到此案件回覆資料 - {Id}");
            }

            var commonDamage = (await _commonDamageService.Get(caseDiagnosis.CommonDamageId)).Items.FirstOrDefault();

            var caseDiagnosisResultResponse = _mapper.Map<CaseDiagnosisResultResponse>(caseDiagnosis);

            caseDiagnosisResultResponse.CommonDamageName = commonDamage?.Name;
            caseDiagnosisResultResponse.DamageClassName = commonDamage?.DamageClassName;
            caseDiagnosisResultResponse.DamageTypeName = commonDamage?.DamageTypeName;
            caseDiagnosisResultResponse.SubmissionMethod = caseDiagnosis.SubmissionMethod;
            caseDiagnosisResultResponse.DiagnosisMethod = caseDiagnosis.DiagnosisMethod;

            return caseDiagnosisResultResponse;
        }

        public async Task<PagedResult<CaseDiagnosisResultResponse>> Get(GetCaseDiagnosisResultDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<CaseDiagnosisResult> caseDiagnoses = _context.CaseDiagnosisResult;

            if (dto.CaseId.HasValue)
            {
                caseDiagnoses = caseDiagnoses.Where(x => x.CaseId == dto.CaseId.Value);
            }

            if (dto.CommonDamageId.HasValue)
            {
                caseDiagnoses = caseDiagnoses.Where(x => x.CommonDamageId == dto.CommonDamageId.Value);
            }

            var caseDiagnosisResultResponse = _mapper.Map<List<CaseDiagnosisResultResponse>>(caseDiagnoses);


            foreach (var caseDto in caseDiagnosisResultResponse)
            {
                var commonDamage = (await _commonDamageService.Get(caseDto.CommonDamageId)).Items.FirstOrDefault();


                caseDto.CommonDamageName = commonDamage?.Name;
                caseDto.DamageClassName = commonDamage?.DamageClassName;
                caseDto.DamageTypeName = commonDamage?.DamageTypeName;
            }

            return caseDiagnosisResultResponse.GetPaged(dto.Page);
        }

        public async Task<CaseDiagnosisResultResponse> Add(AddCaseDiagnosisResultDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseEntity = await _context.Case.FirstOrDefaultAsync(x => x.Id == dto.CaseId);
            if (caseEntity == null)
                throw new ApiException($"找不到案件-{dto.CaseId}");

            var commonDamage = await _context.CommonDamage.FirstOrDefaultAsync(x => x.Id == dto.CommonDamageId);
            if (commonDamage == null)
                throw new ApiException($"找不到常見病蟲害-{dto.CommonDamageId}");

            var caseDiagnosisResult = new CaseDiagnosisResult
            {
                CaseId = dto.CaseId,
                SubmissionMethod = dto.SubmissionMethod,
                DiagnosisMethod = dto.DiagnosisMethod,
                HarmPatternDescription = dto.HarmPatternDescription,
                CommonDamageId = dto.CommonDamageId,
                PreventionSuggestion = dto.PreventionSuggestion,
                OldCommonDamageName = dto.OldCommonDamageName,
                ScientificName = dto.ScientificName,
                ReportingSuggestion = dto.ReportingSuggestion,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await _context.CaseDiagnosisResult.AddAsync(caseDiagnosisResult);

            // 新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增案件回覆 {caseDiagnosisResult.Id}",
                });
            }
            scope.Complete();

            return _mapper.Map<CaseDiagnosisResultResponse>(caseDiagnosisResult);
        }

        public async Task<CaseDiagnosisResultResponse> Update(int Id, UpdateCaseDiagnosisResultDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseDiagnosis = await _context.CaseDiagnosisResult.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseDiagnosis == null)
            {
                throw new ApiException($"找不到此案件資料 - {Id}");
            }

            // 更新資料
            if (dto.SubmissionMethod != null && dto.SubmissionMethod.Any())
                caseDiagnosis.SubmissionMethod = dto.SubmissionMethod;

            if (dto.DiagnosisMethod != null && dto.DiagnosisMethod.Any())
                caseDiagnosis.DiagnosisMethod = dto.DiagnosisMethod;

            if (!string.IsNullOrEmpty(dto.HarmPatternDescription))
                caseDiagnosis.HarmPatternDescription = dto.HarmPatternDescription;

            if (dto.CommonDamageId.HasValue)
                caseDiagnosis.CommonDamageId = dto.CommonDamageId.Value;

            if (!string.IsNullOrEmpty(dto.PreventionSuggestion))
                caseDiagnosis.PreventionSuggestion = dto.PreventionSuggestion;

            if (!string.IsNullOrEmpty(dto.OldCommonDamageName))
                caseDiagnosis.OldCommonDamageName = dto.OldCommonDamageName;

            if (!string.IsNullOrEmpty(dto.ScientificName))
                caseDiagnosis.ScientificName = dto.ScientificName;

            if (!string.IsNullOrEmpty(dto.ReturnReason))
                caseDiagnosis.ReturnReason = dto.ReturnReason;

            if (!string.IsNullOrEmpty(dto.ReportingSuggestion))
                caseDiagnosis.ReportingSuggestion = dto.ReportingSuggestion;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseDiagnosisResult.Update(caseDiagnosis);

            // 新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改案件回覆 {caseDiagnosis.Id}",
                });
            }
            scope.Complete();

            return _mapper.Map<CaseDiagnosisResultResponse>(caseDiagnosis);
        }

        public async Task<List<CaseFileDto>> UploadPhoto(List<IFormFile> photo)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var fileUploadList = new List<CaseFileDto> { };

            foreach (var file in photo)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new CaseFileDto { File = _fileService.Value.GetFile(fileName, "image") });
            }

            return fileUploadList;
        }

        public async Task<CaseDiagnosisResultResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var data = await _context.CaseDiagnosisResult.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (data == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseDiagnosisResult.Remove(data);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除案件回覆-{data.Id}",
                });
            }
            scope.Complete();
            return _mapper.Map<CaseDiagnosisResultResponse>(data);
        }
    }
}
