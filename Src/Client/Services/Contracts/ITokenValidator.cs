using System.Threading.Tasks;

namespace BTB.Client.Services.Contracts
{
    public interface ITokenValidator
    {
        Task<bool> DoesUserHaveATokenAsync();
        Task<bool> IsTokenExpiredAsync();
        Task<string> GetTokenAsync();
    }
}
