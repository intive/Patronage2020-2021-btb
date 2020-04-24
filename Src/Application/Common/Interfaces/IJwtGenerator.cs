using System.Collections.Generic;

namespace BTB.Application.Common.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(string identifier, string email, string name, IList<string> roles);
    }
}
