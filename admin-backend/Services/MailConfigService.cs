﻿using CommonLibrary.Data;
using CommonLibrary.DTOs.MailConfig;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class MailConfigService
    {
        private readonly ILogger<MailConfigService> _log;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;

        public MailConfigService(ILogger<MailConfigService> log, MysqlDbContext context, OperationLogService operationLogService)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
        }

        public async Task<MailConfig> Get()
        {
            return await _context.MailConfig.OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new MailConfig();
        }

        public async Task<MailConfig> Add(AddMailConfigDto dto)
        {
            var mailConfig = new MailConfig
            {
                Host = dto.Host,
                Port = dto.Port,
                Encrypted = dto.Encrypted,
                Account = dto.Account,
                Password = dto.Password,
                Name = dto.Name,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.MailConfig.AddAsync(mailConfig);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = "修改郵寄信件設定",
                });
            }
            scope.Complete();
            return mailConfig;
        }
    }
}
