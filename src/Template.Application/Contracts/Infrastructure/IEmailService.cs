using System.Threading.Tasks;
using Template.Application.Model.Mail;

namespace Template.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendForgotPasswordMail(string address, string url);
        Task<bool> SendMail(Email email);
        Task<bool> SendRegistrationMail(string address, string code);
    }
}
