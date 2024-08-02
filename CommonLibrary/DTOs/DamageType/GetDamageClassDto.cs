using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.DamageType
{
    public class GetDamageClassDto : PagedOperationDto
    {
        public int TypeId {  get; set; }

        //public PagedOperationDto? PagedOperation { get; set; } = null;
    }
}
