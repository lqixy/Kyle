using Kyle.Infrastructure.Events;
using Kyle.Members.Domain.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain
{
    [Table("UserBaseInfo")]
    public class UserInfo : AggregateRoot<Guid>
    {
        //public override string Id { get { return UserId.ToString(); } set { } }


        public UserInfo(string userName, string password, Guid tenantId)
        {
            UserId = Guid.NewGuid();
            UserName = userName;
            Password = password;
            TenantId = tenantId;
            RegDate = DateTime.Now;
        }

        public UserInfo() { }

        [Key]
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid TenantId { get; set; }

        public DateTime RegDate { get; set; }

        public override string AggregateRootId => $"{UserId}";

        //public DateTime CreationTime { get; set; } = DateTime.Now;

        public void AddRegisterRecord()
        {
            ApplyEvent(new UserRegisteredHaveRecord(this.UserId, this.TenantId));
        }

    }
}
