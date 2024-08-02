using CommonLibrary.DTOs;

namespace admin_backend.DTOs.DamageType
{
    public class GetDamageClassDto : PagedOperationDto
    {
        public int TypeId { get; set; }

        //public PagedOperationDto? PagedOperation { get; set; } = null;
    }
}
