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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Avatar { get; set; }

        internal void Update(string email) {
            this.Email = email;
        }
        
    }
}
