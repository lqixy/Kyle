using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Identity.Application.Constructs
{
    public interface IIdentityClientAppService
    {
        Task<AccessTokenDto> Authorization(string userName, string password);
    }
}
