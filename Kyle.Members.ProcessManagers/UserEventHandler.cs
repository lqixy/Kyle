using Kyle.Extensions;
using Kyle.Infrastructure.Events;
using Kyle.Members.Domain;
using Kyle.Members.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.ProcessManagers
{
    public class UserEventHandler
        : IAsyncEventHandler<UserRegisteredMessage>
        , ITransientDependency
    {
        private readonly IUserRegisterRecordRepository repository;

        public UserEventHandler(IUserRegisterRecordRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleEventAsync(UserRegisteredMessage eventData)
        {
            await repository.Insert(new UserRegisterRecord(eventData.UserId, eventData.TenantId));
        }
    }
}
