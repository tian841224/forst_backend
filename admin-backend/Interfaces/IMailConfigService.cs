using admin_backend.DTOs.MailConfig;

namespace admin_backend.Interfaces
{
    public interface IMailConfigService
    {
        Task<MailConfigResponse> Get();
        Task<MailConfigResponse> Add(AddMailConfigDto dto);
        Task TestSendEmail(string email);
    }
}
