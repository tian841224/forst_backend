using CommonLibrary.Data;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.RolePermission;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace admin_backend.Services
{
    public class RolePermissionService
    {
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        private readonly ILogger<RolePermissionService> _log;


        public RolePermissionService(MysqlDbContext context, OperationLogService operationLogService, ILogger<RolePermissionService> log)
        {
            _context = context;
            _operationLogService = operationLogService;
            _log = log;
        }

        public async Task<RolePermission> Add(AddRolePermissionDto dto)
        {
            var rolePermission = await _context.RolePermission.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();

            if (rolePermission != null)
            {
                throw new ApiException($"此身分權限已存在-{dto.Name}");

            }

            rolePermission = new RolePermission
            {
                Name = dto.Name,
                RoleId = dto.RoleId,
                View = dto.View,
                Add = dto.Add,
                Sign = dto.Sign,
                Edit = dto.Edit,
                Delete = dto.Delete
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.RolePermission.AddAsync(rolePermission);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Add,
                        Content = $"新增身分權限：{rolePermission.Id}/{rolePermission.Name}",
                    });
                };
                await transaction.CommitAsync();
                return rolePermission;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _log.LogError(ex.Message);
                throw;
            }
        }
    }
}
