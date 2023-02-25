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
        private readonly IUserTestQueryService testQueryService;
        public UserAppService(IUserQueryRepository repository, IUserTestQueryService testQueryService)
        {
            this.repository = repository;
            this.testQueryService = testQueryService;
        }

        public async Task<UserInfoDto> Get()
        {
            var entity = await repository.Get();
            var t = await testQueryService.Get();
            return new UserInfoDto(entity.UserId, entity.TenantId, entity.RegDate);
        }
    }
}
