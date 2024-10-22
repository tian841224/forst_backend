using admin_backend.Data;
using admin_backend.DTOs.Documentation;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class DocumentationService : IDocumentationService
    {
        private readonly ILogger<DocumentationService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IOperationLogService> _operationLogService;

        public DocumentationService(ILogger<DocumentationService> log, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IMapper mapper)
        {
            _log = log;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<List<DocumentationResponse>> Get()
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var result = new List<DocumentationResponse>();
            var consentForm = await _context.Documentation.Where(x => x.Type == DocumentationEnum.ConsentForm).OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new Documentation { Type = DocumentationEnum.ConsentForm, Content = null! };
            var userGuide = await _context.Documentation.Where(x => x.Type == DocumentationEnum.UserGuide).OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new Documentation { Type = DocumentationEnum.UserGuide, Content = null! };

            result.Add(_mapper.Map<DocumentationResponse>(consentForm));
            result.Add(_mapper.Map<DocumentationResponse>(userGuide));

            return result;
        }
        public async Task<DocumentationResponse> Add(AddDocumentationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

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
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增說明文件：{documentation.Type}",
                });
            };
            scope.Complete();
            return _mapper.Map<DocumentationResponse>(documentation);
        }
    }
}
