using AutoMapper;

namespace admin_backend.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //自動映射
            var assembly = typeof(CommonLibrary.Entities.AdminUser).Assembly;

            var entityTypes = assembly.GetTypes()
                .Where(t => t.Namespace == "CommonLibrary.Entities" && !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var dtoTypeName = $"CommonLibrary.DTOs.{entityType.Name}.{entityType.Name}Response";
                var dtoType = assembly.GetType(dtoTypeName);

                if (dtoType != null)
                {
                    CreateMap(entityType, dtoType).ReverseMap();
                }
            }

            // 對某個命名空間下的所有類進行映射
            this.RecognizePrefixes("CommonLibrary.Entities", "CommonLibrary.DTOs");
            this.RecognizeDestinationPrefixes("CommonLibrary.DTOs");

            AllowNullCollections = true;
        }
    }
}
