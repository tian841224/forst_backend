using admin_backend.Data;
using admin_backend.DTOs.CaseHistory;
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
    public class CaseHistoryService : ICaseHistoryService
    {
        private readonly Lazy<IIdentityService> _identityService;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IOperationLogService> _operationLogService;


        public CaseHistoryService(Lazy<IIdentityService> identityService, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IOperationLogService> operationLogService)
        {
            _identityService = identityService;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _operationLogService = operationLogService;
        }

        public async Task<CaseHistoryResponse> Get(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var caseHistory = await _context.CaseHistory.FirstOrDefaultAsync(x => x.Id == Id);

            if (caseHistory == null)
                throw new ApiException($"找不到此案件歷程 - {Id}");

            string name = string.Empty;
            //使用者
            if (caseHistory.Role == 0)
            {
                var user = await _context.User.FirstOrDefaultAsync(x => x.Id == caseHistory.OperatorId);
                if (user == null)
                    throw new ApiException($"找不到使用者 - {caseHistory.OperatorId}");
                name = user.Name;
            }
            //管理員
            else
            {
                var adminUesr = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == caseHistory.OperatorId);
                if (adminUesr == null)
                    throw new ApiException($"找不到管理員 - {caseHistory.OperatorId}");

                name = adminUesr.Name;
            }

            var caseHistoryResponse = _mapper.Map<CaseHistoryResponse>(caseHistory);

            caseHistoryResponse.Operator = name;

            return caseHistoryResponse;
        }

        public async Task<PagedResult<CaseHistoryResponse>> Get(GetCaseHistoryDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseHistoryQuery = _context.CaseHistory.Where(x => x.CaseId == dto.CaseId);

            if (!caseHistoryQuery.Any())
                throw new ApiException($"找不到此案件歷程 - {dto.CaseId}");

            var caseHistoryList = await caseHistoryQuery.ToListAsync();
            var caseHistoryResponses = new List<CaseHistoryResponse>();

            foreach (var caseHistory in caseHistoryList)
            {
                string name = string.Empty;

                // 使用者
                if (caseHistory.Role == 0)
                {
                    var user = await _context.User.FirstOrDefaultAsync(x => x.Id == caseHistory.OperatorId);
                    if (user == null)
                        throw new ApiException($"找不到使用者 - {caseHistory.OperatorId}");
                    name = user.Name;
                }
                // 管理員
                else
                {
                    var adminUser = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == caseHistory.OperatorId);
                    if (adminUser == null)
                        throw new ApiException($"找不到管理員 - {caseHistory.OperatorId}");
                    name = adminUser.Name;
                }

                var caseHistoryResponse = _mapper.Map<CaseHistoryResponse>(caseHistory);
                caseHistoryResponse.Operator = name;

                caseHistoryResponses.Add(caseHistoryResponse);
            }


            return caseHistoryResponses.GetPaged(dto.Page!);
        }

        public async Task Add(AddCaseHistoryDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            //判斷當前身分
            var claimsDto = _identityService.Value.GetUser();
            int.TryParse(claimsDto.RoleId, out int roleId);
            int userId = 0;
            //使用者
            if (roleId == 0)
            {
                int.TryParse(claimsDto.UserId, out userId);
            }
            //管理員
            else
            {
                int.TryParse(claimsDto.UserId, out userId);
            }

            string description = string.Empty;

            switch (dto.ActionType)
            {
                case ActionTypeEnum.Assign:
                    description = $"指派給:{claimsDto.UserNane}";
                    break;

                case ActionTypeEnum.Return:
                    description = $"退回原因:{dto.Description}";
                    break;
            }


            var caseHistory = new CaseHistory
            {
                CaseId = dto.CaseId,
                ActionType = dto.ActionType,
                ActionTime = dto.ActionTime,
                Description = description,
                Role = roleId,
                OperatorId = userId
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await _context.CaseHistory.AddAsync(caseHistory);

            // 新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增案件歷程 {caseHistory.CaseId}",
                });
            }
            scope.Complete();
        }
    }
}
