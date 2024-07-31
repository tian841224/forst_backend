using CommonLibrary.Data;
using CommonLibrary.DTOs.Documentation;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class DocumentationService
    {
        private readonly ILogger<DocumentationService> _log;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;

        public DocumentationService(ILogger<DocumentationService> log, MysqlDbContext context, OperationLogService operationLogService)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
        }

        public async Task<Documentation> Get()
        {
            return await _context.Documentation.OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new Documentation();
        }

        public async Task<Documentation> Add(AddDocumentationDto dto)
        {

            var documentation = new Documentation
            {
                Type = dto.Type,
                Content = dto.Content,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.Documentation.AddAsync(documentation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增說明文件：{documentation.Type}",
                });
            };
            scope.Complete();
            return documentation;
        }
    }
}
