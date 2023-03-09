using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Domain
{
    [Table("UserRegisterRecord")]
    public class UserRegisterRecord
    {
        public UserRegisterRecord()
        {
            RecordId = Guid.NewGuid();
            AddDate = DateTime.Now;
        }

        public UserRegisterRecord(Guid userId, Guid tenantId) : base()
        {
            UserId = userId;
            TenantId = tenantId;
        }

        public Guid RecordId { get; set; }

        public DateTime AddDate { get; set; }

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }
    }
}
