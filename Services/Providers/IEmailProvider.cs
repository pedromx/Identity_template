using DTOs;
using System.Threading.Tasks;

namespace Services.Providers
{
    public interface IEmailProvider
    {
        void Send(string emailAddress, string body, EmailProviderSettingsDTO options);
    }
}