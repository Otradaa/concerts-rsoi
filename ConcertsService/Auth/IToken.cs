using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertsService.Auth
{
    public interface IToken
    {
        string GenerateToken(string userName);
        bool ValidateToken(string token);
    }
    
}
