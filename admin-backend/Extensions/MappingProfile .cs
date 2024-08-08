using admin_backend.DTOs.CommonDamage;
using admin_backend.DTOs.ForestDiseasePublications;
using admin_backend.Entities;
using AutoMapper;
using AutoMapper.Internal;
using System.Collections;

namespace admin_backend.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 對某個命名空間下的所有類進行映射
            this.RecognizePrefixes("admin_backend.Entities", "admin_backend.DTOs");
            this.RecognizeDestinationPrefixes("admin_backend.DTOs");
            this.Internal().ForAllMaps((_, cfg) =>
               cfg.ForAllMembers(opts =>
               opts.Condition((_, _, srcMember) =>
                   !(srcMember is ICollection))));
            AllowNullCollections = true;

            //自動映射
            var assembly = typeof(AdminUser).Assembly;

            var entityTypes = assembly.GetTypes()
                .Where(t => t.Namespace == "admin_backend.Entities" && !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var dtoTypeName = $"admin_backend.DTOs.{entityType.Name}.{entityType.Name}Response";
                var dtoType = assembly.GetType(dtoTypeName);

                if (dtoType != null)
                {
                    CreateMap(entityType, dtoType).ReverseMap();
                }
            }

            //忽略File
            CreateMap<ForestDiseasePublications, ForestDiseasePublicationsResponse>()
             .ForMember(dest => dest.File, opt => opt.Ignore())
             .IgnoreAllPropertiesWithAnInaccessibleSetter()
             .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            //忽略File
            CreateMap<CommonDamage, CommonDamageResponse>()
             .ForMember(dest => dest.Photo, opt => opt.Ignore())
             .ForMember(dest => dest.DamageClassName, opt => opt.Ignore())
             .ForMember(dest => dest.DamageTypeName, opt => opt.Ignore());
        }
    }
}
