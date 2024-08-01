using CommonLibrary.DTOs.Common;

namespace CommonLibrary.Interface
{
    public interface IEmailService
    {
        void SendEmail(SendEmailDto dto);
    }
}
