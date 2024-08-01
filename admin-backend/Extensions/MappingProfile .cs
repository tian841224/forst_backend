using admin_backend.Services;
using AutoMapper;

namespace admin_backend.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 自動映射所有屬性名稱相同的類
            CreateMap<CommonLibrary.Entities.AdminUser, CommonLibrary.DTOs.AdminUser.AdminUserResponse>()
                .ReverseMap();
            CreateMap<CommonLibrary.Entities.DamageClass, CommonLibrary.DTOs.DamageClass.DamageClassResponse>()
             .ReverseMap();
            CreateMap<CommonLibrary.Entities.DamageType, CommonLibrary.DTOs.DamageType.DamageTypeResponse>()
              .ReverseMap();
            CreateMap<CommonLibrary.Entities.Documentation, CommonLibrary.DTOs.Documentation.DocumentationResponse>()
              .ReverseMap(); 
            CreateMap<CommonLibrary.Entities.EpidemicSummary, CommonLibrary.DTOs.EpidemicSummary.EpidemicSummaryResponse>()
              .ReverseMap(); 
            CreateMap<CommonLibrary.Entities.ForestCompartmentLocation, CommonLibrary.DTOs.ForestCompartmentLocation.ForestCompartmentLocationResponse>()
              .ReverseMap();
            CreateMap<CommonLibrary.Entities.ForestDiseasePublications, CommonLibrary.DTOs.ForestDiseasePublications.ForestDiseasePublicationsResponse>()
              .ReverseMap(); 
            CreateMap<CommonLibrary.Entities.MailConfig, CommonLibrary.DTOs.MailConfig.MailConfigResponse>()
              .ReverseMap(); 
            CreateMap<CommonLibrary.Entities.OperationLog, CommonLibrary.DTOs.OperationLog.OperationLogResponse>()
              .ReverseMap();
            CreateMap<CommonLibrary.Entities.RolePermission, CommonLibrary.DTOs.RolePermission.RolePermissionResponse>()
              .ReverseMap(); 
            CreateMap<CommonLibrary.Entities.Role, CommonLibrary.DTOs.Role.RoleResponse>()
              .ReverseMap();
            CreateMap<CommonLibrary.Entities.TreeBasicInfo, CommonLibrary.DTOs.TreeBasicInfo.TreeBasicInfoResponse>()
              .ReverseMap();
            CreateMap<CommonLibrary.Entities.User, CommonLibrary.DTOs.User.UserResponse>()
              .ReverseMap();
            // 對某個命名空間下的所有類進行映射
            this.RecognizePrefixes("CommonLibrary.Entities", "CommonLibrary.DTOs");
            this.RecognizeDestinationPrefixes("CommonLibrary.DTOs");

            AllowNullCollections = true;
        }
    }
}
