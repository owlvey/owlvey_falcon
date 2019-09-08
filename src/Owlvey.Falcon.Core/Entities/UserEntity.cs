    using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Core.Entities
{
    public class UserCompare : IEqualityComparer<UserEntity>
    {
        public bool Equals(UserEntity x, UserEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(UserEntity obj)
        {
            return obj.GetHashCode();
        }
    }
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
