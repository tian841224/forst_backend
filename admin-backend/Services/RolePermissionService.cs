using CommonLibrary.Data;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.RolePermission;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;

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

        public async Task<GetRolePermission> Get(int Id)
        {
            IQueryable<RolePermission> query = _context.RolePermission.AsQueryable();

            query = query.Where(x => x.RoleId == Id);

            return new GetRolePermission
            {
                RoleId = Id,
                Permissions = await query.Select(x => new GetRolePermission.Permission
                {
                    Id = x.Id,
                    Name = x.Name,
                    View = x.View,
                    Add = x.Add,
                    Sign = x.Sign,
                    Edit = x.Edit,
                    Delete = x.Delete
                }).ToListAsync()
            };
        }
        public async Task<RolePermission> Add(AddRolePermissionDto dto)
        {
            var rolePermission = await _context.RolePermission.Where(x => x.RoleId == dto.RoleId && x.Name == dto.Name).FirstOrDefaultAsync();

            if (rolePermission != null)
            {
                return rolePermission;
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

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
            scope.Complete();
            return rolePermission;
        }

        public async Task<List<RolePermission>> Add(AddRolePermissionRequestDto dto)
        {
            var result = new List<RolePermission>();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var value in dto.Permissions)
            {
                var rolePermission = await _context.RolePermission.Where(x => x.Name == value.Name).FirstOrDefaultAsync();

                if (rolePermission != null)
                {
                    continue;
                }

                rolePermission = new RolePermission
                {
                    RoleId = dto.RoleId,
                    Name = value.Name,
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
            scope.Complete();
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

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
            scope.Complete();
            return rolePermission;
        }

        public async Task<List<RolePermission>> Update(UpdateRolePermissionRequestDto dto)
        {
            var result = new List<RolePermission>();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var value in dto.Permissions)
            {
                var rolePermission = await _context.RolePermission.Where(x => x.Id == value.Id).FirstOrDefaultAsync();

                if (rolePermission == null)
                {
                    result.Add(await Add(new AddRolePermissionDto
                    {
                        Name = value.Name ?? string.Empty,
                        RoleId = dto.RoleId,
                        View = value.View,
                        Add = value.Add,
                        Sign = value.Sign,
                        Edit = value.Edit,
                        Delete = value.Delete,
                    }));
                }
                else
                {
                    rolePermission.Name = value.Name ?? string.Empty;
                    rolePermission.View = value.View;
                    rolePermission.Add = value.Add;
                    rolePermission.Sign = value.Sign;
                    rolePermission.Edit = value.Edit;
                    rolePermission.Delete = value.Delete;

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

                    result.Add(rolePermission);
                }
            }
            scope.Complete();
            return result;
        }

        //public async Task<RolePermission> Delete(DeleteRolePermissionDto dto)
        //{
        //    var rolePermission = await _context.RolePermission.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

        //    if (rolePermission == null)
        //    {
        //        throw new ApiException($"此ID不存在-{dto.Id}");
        //    }

        //    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        //    _context.RolePermission.Remove(rolePermission);

        //    //新增操作紀錄
        //    if (await _context.SaveChangesAsync() > 0)
        //    {
        //        await _operationLogService.Add(new AddOperationLogDto
        //        {
        //            Type = ChangeTypeEnum.Delete,
        //            Content = $"移除身分權限：{rolePermission.Id}/{rolePermission.Name}",
        //        });
        //    };
        //    scope.Complete();
        //    return rolePermission;
        //}

        //public async Task<List<RolePermission>> Delete(List<DeleteRolePermissionDto> dto)
        //{
        //    var result = new List<RolePermission>();

        //    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        //    foreach (var value in dto)
        //    {
        //        var rolePermission = await _context.RolePermission.Where(x => x.Id == value.Id).FirstOrDefaultAsync();

        //        if (rolePermission == null)
        //        {
        //            continue;
        //        }
        //        _context.RolePermission.Remove(rolePermission);

        //        //新增操作紀錄
        //        if (await _context.SaveChangesAsync() > 0)
        //        {
        //            await _operationLogService.Add(new AddOperationLogDto
        //            {
        //                Type = ChangeTypeEnum.Delete,
        //                Content = $"移除身分權限：{rolePermission.Id}/{rolePermission.Name}",
        //            });
        //        };
        //        result.Add(rolePermission);
        //    }
        //    scope.Complete();
        //    return result;
        //}
    }
}
