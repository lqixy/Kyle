using Kyle.Members.Application.Constructs;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kyle.Extensions.Exceptions;

namespace Kyle.Members.Application
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserQueryRepository repository; 
        public UserAppService(IUserQueryRepository repository)
        {
            this.repository = repository; 
        }

        public async Task<UserInfoDto> Get()
        {
            var entity = await repository.Get();
            if (entity == null) throw new UserFriendlyException("未找到用户信息");
            return new UserInfoDto(entity.UserId, entity.TenantId, entity.RegDate);
        }
    }
}
