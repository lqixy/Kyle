using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Application.Constructs
{
    public interface IUserAppService
    {
        Task<UserInfoDto> Get();
    }
}
