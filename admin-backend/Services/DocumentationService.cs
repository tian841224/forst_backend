using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;

        public DocumentationService(ILogger<DocumentationService> log, MysqlDbContext context, OperationLogService operationLogService, IMapper mapper)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<List<DocumentationResponse>> Get()
        {
            var result = new List<DocumentationResponse>();
            var consentForm = await _context.Documentation.Where(x => x.Type == DocumentationEnum.ConsentForm).OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new Documentation { Type = DocumentationEnum.ConsentForm, Content = null };
            var userGuide = await _context.Documentation.Where(x => x.Type == DocumentationEnum.UserGuide).OrderByDescending(x => x.Id).FirstOrDefaultAsync() ?? new Documentation { Type = DocumentationEnum.ConsentForm, Content = null };

            result.Add(_mapper.Map<DocumentationResponse>(consentForm));
            result.Add(_mapper.Map<DocumentationResponse>(userGuide));

            return result;
        }
        public async Task<DocumentationResponse> Add(AddDocumentationDto dto)
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
            return _mapper.Map<DocumentationResponse>(documentation);
        }
    }
}
