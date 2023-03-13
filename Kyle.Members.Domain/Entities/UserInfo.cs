using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kyle.EntityFrameworkExtensions;
using Kyle.Members.Domain.Events;

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
            Pwd = password;
            TenantId = tenantId;
            RegDate = DateTime.Now;
        }

        public UserInfo() { }

        [Key]
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }

        public Guid TenantId { get; set; }

        public DateTime RegDate { get; set; }

        // public override string AggregateRootId => $"{UserId}";

        //public DateTime CreationTime { get; set; } = DateTime.Now;

        public void AddRegisterRecord()
        {
            ApplyEvent(new UserRegistered(this.UserId, this.TenantId));
        }

        // public override string AggregateRootId => $"";
    }
}
