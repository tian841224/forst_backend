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

            // 對某個命名空間下的所有類進行映射
            this.RecognizePrefixes("CommonLibrary.Entities", "CommonLibrary.DTOs");
            this.RecognizeDestinationPrefixes("CommonLibrary.DTOs");

            AllowNullCollections = true;
        }
    }
}
