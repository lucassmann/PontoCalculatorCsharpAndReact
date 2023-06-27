using PontoCalculator.Dtos;

namespace PontoCalculator.Helpers.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto emailDto);
    }
}
