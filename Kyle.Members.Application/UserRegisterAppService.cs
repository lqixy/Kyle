using Kyle.Encrypt.Application.Constructs;
using Kyle.Extensions.Exceptions;
using Kyle.Members.Application.Constructs;
using Kyle.Members.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Application
{
    public class UserRegisterAppService : IUserRegisterAppService
    {
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IEncryptAppService _encryptAppService;
        private readonly IUserOperationRepository _userOperationRepository;

        public UserRegisterAppService(IUserQueryRepository userQueryRepository, IEncryptAppService encryptAppService, IUserOperationRepository userOperationRepository)
        {
            _userQueryRepository = userQueryRepository;
            _encryptAppService = encryptAppService;
            _userOperationRepository = userOperationRepository;
        }

        public async Task Register(RegisterInputDto input)
        {
            var user = await _userQueryRepository.Get(x => x.UserName == input.UserName);
            if (user != null) { throw new UserFriendlyException("用户名已存在"); }

            var entity = new UserInfo(input.UserName, _encryptAppService.CreatePasswordHash(input.Password), Guid.NewGuid());
            entity.AddRegisterRecord();

            await _userOperationRepository.Insert(entity);
        }
    }
}
