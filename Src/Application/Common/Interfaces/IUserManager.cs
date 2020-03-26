using BTB.Domain.Entities;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<ApplicationUser> FindByNameAsync(string userName);
    }
}