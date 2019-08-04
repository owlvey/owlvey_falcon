using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class UserEntity: BaseEntity
    {
        [Required]
        public string Email { get; set; }        

        internal void Update(string email) {
            this.Email = email;
        }
        
    }
}
