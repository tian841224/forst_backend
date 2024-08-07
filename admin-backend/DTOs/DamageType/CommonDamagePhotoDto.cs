using CommonLibrary.DTOs;

namespace admin_backend.DTOs.DamageType
{
    public class CommonDamagePhotoDto : SortDto
    {
        public IFormFile Photo { get; set; }

        //public SortDto SortDtos { get; set; } = new();
    }
}
