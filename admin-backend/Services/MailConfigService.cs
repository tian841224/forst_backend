using admin_backend.Data;
using admin_backend.DTOs.MailConfig;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Transactions;

namespace admin_backend.Services
{
    public class MailConfigService : IMailConfigService
    {
        private readonly ILogger<MailConfigService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly Lazy<IEmailService> _emailService;

        public MailConfigService(ILogger<MailConfigService> log, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, Lazy<IEmailService> emailService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _emailService = emailService;
        }

        public async Task<MailConfigResponse> Get()
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var mailConfig = await _context.MailConfig.OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new MailConfig();
            return _mapper.Map<MailConfigResponse>(mailConfig);
        }

        public async Task<MailConfigResponse> Add(AddMailConfigDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var mailConfig = new MailConfig
            {
                Host = dto.Host,
                Port = dto.Port,
                Encrypted = dto.Encrypted,
                Account = dto.Account,
                Password = dto.pKey,
                Name = dto.Name,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.MailConfig.AddAsync(mailConfig);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = "修改郵寄信件設定",
                });
            }
            scope.Complete();
            return _mapper.Map<MailConfigResponse>(mailConfig);
        }

        public async Task TestSendEmail(string email)
        {
            var emailConfig = await Get();

            _emailService.Value.SendEmail(new SendEmailDto
            {
                Host = emailConfig.Host,
                Port = emailConfig.Port,
                Account = emailConfig.Account,
                Password = emailConfig.Password,
                EnableSsl = emailConfig.Encrypted == EncryptedEnum.SSL,
                MailMessage = new MailMessage
                {
                    From = new MailAddress($"{emailConfig.Account}@{emailConfig.Host}", emailConfig.Name),
                    Subject = "測試郵件",
                    Body = "此信件為郵寄信件設定",
                    IsBodyHtml = true,
                },
                Recipient = email,
            });
        }
    }
}
