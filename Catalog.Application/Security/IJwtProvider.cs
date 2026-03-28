using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Security
{
    public interface IJwtProvider
    {
        string GenerateToken(string userId, string username, string role);
    }
}
