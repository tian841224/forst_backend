using CommonLibrary.Data;
using CommonLibrary.DTOs.MailConfig;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;

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
            var mailConfig = await _context.MailConfig.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return mailConfig;
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

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.MailConfig.AddAsync(mailConfig);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Add,
                        Content = "修改郵寄信件設定",
                    });
                }
                await transaction.CommitAsync();
                return mailConfig;
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
