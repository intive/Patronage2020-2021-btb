
namespace BTB.Application.Common.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(string name);
    }
}
