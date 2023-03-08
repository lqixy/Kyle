using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.EntityFrameworkExtensions.Test.Models
{
    [Table("UserBaseInfo")]
    public class UserInfo //: IBaseModel<int>
    {
        [Key]
        public Guid UserId { get; set; }

        public string? Mobile { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }


        public DateTime RegDate { get; set; }
    }
}
