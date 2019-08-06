using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class UserBaseRp
    {
        public string Email { get; set; }
        public int Id { get; set; }
    }

    public class UserGetRp : UserBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class UserGetListRp : UserBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class UserPostRp
    {
        [Required]
        public string Email { get; set; }        
    }

}
