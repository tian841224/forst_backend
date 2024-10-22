﻿using admin_backend.DTOs.Case;
using admin_backend.DTOs.CaseDiagnosisResult;
using CommonLibrary.Extensions;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 案件回覆
    /// </summary>
    public interface ICaseDiagnosisResultService
    {
        /// <summary>取得單筆案件回覆</summary>
        Task<CaseDiagnosisResultResponse> Get(int Id);

        /// <summary>使用CaseID取得單筆案件回覆</summary>
        Task<CaseDiagnosisResultResponse> GetByCaseId(int CaseId);

        /// <summary>取得案件回覆</summary>
        Task<PagedResult<CaseDiagnosisResultResponse>> Get(GetCaseDiagnosisResultDto dto);

        /// <summary>新增案件回覆</summary>
        Task<CaseDiagnosisResultResponse> Add(AddCaseDiagnosisResultDto dto);

        /// <summary>修改案件回覆</summary>
        Task<CaseDiagnosisResultResponse> Update(int Id, UpdateCaseDiagnosisResultDto dto);

        /// <summary>上傳圖片</summary>
        Task<List<CaseRecordFileDto>> UploadPhoto(List<IFormFile> photo);

        /// <summary>刪除案件回覆</summary>
        Task<CaseDiagnosisResultResponse> Delete(int Id);
    }
}
