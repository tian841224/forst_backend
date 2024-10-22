namespace admin_backend.DTOs.CommonDamage
{
    public class UpdateCommonDamagePhotoDto
    {
        public IFormFile Photo { get; set; } = null!;
        public int Sort { get; set; }
    }
}
