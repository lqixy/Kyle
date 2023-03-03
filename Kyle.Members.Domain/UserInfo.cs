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
    public class UserInfo //: BaseModel<string>
    {
        //public override string Id { get { return UserId.ToString(); } set { } }

        [Key]
        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public DateTime RegDate { get; set; }

        //public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
