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

        public async Task<List<RolePermission>> Add(List<AddRolePermissionDto> dto)
        {
            var result = new List<RolePermission>();
            foreach (var value in dto)
            {
                var rolePermission = await _context.RolePermission.Where(x => x.Name == value.Name).FirstOrDefaultAsync();

                if (rolePermission != null)
                {
                    continue;
                }

                rolePermission = new RolePermission
                {
                    Name = value.Name,
                    RoleId = value.RoleId,
                    View = value.View,
                    Add = value.Add,
                    Sign = value.Sign,
                    Edit = value.Edit,
                    Delete = value.Delete
                };
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
                result.Add(rolePermission);
            }
            return result;
        }

        public async Task<RolePermission> Update(UpdateRolePermissionDto dto)
        {
            var rolePermission = await _context.RolePermission.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (rolePermission == null)
            {
                throw new ApiException($"找不到此ID-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                rolePermission.Name = dto.Name;

            if (dto.View.HasValue)
                rolePermission.View = dto.View.Value;

            if (dto.Add.HasValue)
                rolePermission.Add = dto.Add.Value;

            if (dto.Sign.HasValue)
                rolePermission.Sign = dto.Sign.Value;

            if (dto.Edit.HasValue)
                rolePermission.Edit = dto.Edit.Value;

            if (dto.Delete.HasValue)
                rolePermission.Delete = dto.Delete.Value;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.RolePermission.Update(rolePermission);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改身分權限：{rolePermission.Id}/{rolePermission.Name}",
                    });
                }
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

        public async Task<List<RolePermission>> Update(List<UpdateRolePermissionDto> dto)
        {
            var result = new List<RolePermission>();

            foreach (var value in dto)
            {
                var rolePermission = await _context.RolePermission.Where(x => x.Id == value.Id).FirstOrDefaultAsync();

                if (rolePermission == null)
                {
                    await Add(new AddRolePermissionDto
                    {
                        Name = value.Name,
                        RoleId = value.RoleId,
                        View = value.View.Value,
                        Add = value.Add.Value,
                        Sign = value.Sign.Value,
                        Edit = value.Edit.Value,
                        Delete = value.Delete.Value,
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(value.Name))
                        rolePermission.Name = value.Name;

                    if (value.View.HasValue)
                        rolePermission.View = value.View.Value;

                    if (value.Add.HasValue)
                        rolePermission.Add = value.Add.Value;

                    if (value.Sign.HasValue)
                        rolePermission.Sign = value.Sign.Value;

                    if (value.Edit.HasValue)
                        rolePermission.Edit = value.Edit.Value;

                    if (value.Delete.HasValue)
                        rolePermission.Delete = value.Delete.Value;

                    _context.RolePermission.Update(rolePermission);

                    //新增操作紀錄
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        await _operationLogService.Add(new AddOperationLogDto
                        {
                            Type = ChangeTypeEnum.Edit,
                            Content = $"修改身分權限：{rolePermission.Id}/{rolePermission.Name}",
                        });
                    }
                }
            }
            return result;
        }

        public async Task<RolePermission> Delete(DeleteRolePermissionDto dto)
        {
            var rolePermission = await _context.RolePermission.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (rolePermission == null)
            {
                throw new ApiException($"此ID不存在-{dto.Id}");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.RolePermission.Remove(rolePermission);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Delete,
                        Content = $"移除身分權限：{rolePermission.Id}/{rolePermission.Name}",
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

        public async Task<List<RolePermission>> Delete(List<DeleteRolePermissionDto> dto)
        {
            var result = new List<RolePermission>();

            foreach (var value in dto)
            {
                var rolePermission = await _context.RolePermission.Where(x => x.Id == value.Id).FirstOrDefaultAsync();

                if (rolePermission == null)
                {
                    continue;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.RolePermission.Remove(rolePermission);

                    //新增操作紀錄
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        await _operationLogService.Add(new AddOperationLogDto
                        {
                            Type = ChangeTypeEnum.Delete,
                            Content = $"移除身分權限：{rolePermission.Id}/{rolePermission.Name}",
                        });
                    };
                    await transaction.CommitAsync();
                    result.Add(rolePermission);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _log.LogError(ex.Message);
                    throw;
                }
            }

            return result;
        }
    }
}
