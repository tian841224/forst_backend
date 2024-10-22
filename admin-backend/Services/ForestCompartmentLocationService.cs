﻿using admin_backend.Data;
using admin_backend.DTOs.ForestCompartmentLocation;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class ForestCompartmentLocationService : IForestCompartmentLocationService
    {
        private readonly ILogger<ForestCompartmentLocationService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;

        public ForestCompartmentLocationService(ILogger<ForestCompartmentLocationService> log, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IMapper mapper)
        {
            _log = log;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<PagedResult<ForestCompartmentLocationResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<ForestCompartmentLocation> forestCompartmentLocation = _context.ForestCompartmentLocation;

            if (Id.HasValue)
                forestCompartmentLocation = _context.ForestCompartmentLocation.Where(x => x.Id == Id).AsQueryable();

            var forestCompartmentLocationResponse = _mapper.Map<List<ForestCompartmentLocationResponse>>(forestCompartmentLocation);
            //分頁處理
            return forestCompartmentLocationResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<ForestCompartmentLocationResponse>> Get(GetForestCompartmentLocationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<ForestCompartmentLocation> forestCompartmentLocations = _context.ForestCompartmentLocation;

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                forestCompartmentLocations = forestCompartmentLocations.Where(x =>
                    x.AffiliatedUnit.ToLower().Contains(keyword) ||
                    x.Position.ToLower().Contains(keyword) ||
                    x.Id.ToString().Contains(keyword)
                );
            }

            var forestCompartmentLocationResponse = _mapper.Map<List<ForestCompartmentLocationResponse>>(forestCompartmentLocations);
            //分頁處理
            return forestCompartmentLocationResponse.GetPaged(dto.Page!);
        }

        public async Task<ForestCompartmentLocationResponse> Add(AddForestCompartmentLocationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestCompartmentLocation = new ForestCompartmentLocation
            {
                Position = dto.Position,
                AffiliatedUnit = dto.AffiliatedUnit,
                Sort = dto.Sort,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.ForestCompartmentLocation.AddAsync(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return _mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation);
        }

        public async Task<ForestCompartmentLocationResponse> Update(int Id, UpdateForestCompartmentLocationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();


            var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestCompartmentLocation == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (!string.IsNullOrEmpty(dto.Position))
                forestCompartmentLocation.Position = dto.Position;

            if (!string.IsNullOrEmpty(dto.AffiliatedUnit))
                forestCompartmentLocation.AffiliatedUnit = dto.AffiliatedUnit;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestCompartmentLocation.Update(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return _mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation);
        }

        public async Task<List<ForestCompartmentLocationResponse>> UpdateSort(List<SortBasicDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = new List<ForestCompartmentLocationResponse>();

            foreach (var value in dto)
            {
                var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == value.Id).FirstOrDefaultAsync();
                if (forestCompartmentLocation == null) continue;

                if (value.Sort.HasValue)
                    forestCompartmentLocation.Sort = value.Sort.Value;

                _context.ForestCompartmentLocation.Update(forestCompartmentLocation);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Value.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改林班位置排序：{forestCompartmentLocation.Id}/{forestCompartmentLocation.Sort}",
                    });
                };

                result.Add(_mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation));
            }

            scope.Complete();
            return result;
        }

        public async Task<ForestCompartmentLocationResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();


            var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestCompartmentLocation == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestCompartmentLocation.Remove(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"移除林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return _mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation);
        }
    }
}
