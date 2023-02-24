using Kyle.Members.Application.Constructs;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return new UserInfoDto(entity.UserId, entity.CreationTime);
        }
    }
}
