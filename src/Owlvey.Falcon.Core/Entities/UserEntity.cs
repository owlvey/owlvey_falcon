using System;
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
        public string SlackMember { get; set; }

        public void Update(string email, string avatar, string name, string slackmember) {
            this.Email =  !string.IsNullOrWhiteSpace(email) ? email : this.Email;
            this.Avatar = !string.IsNullOrWhiteSpace(avatar) ? avatar : this.Avatar;
            this.Name = !string.IsNullOrWhiteSpace(avatar) ? name : this.Name;
            this.SlackMember = !string.IsNullOrWhiteSpace(slackmember) ? slackmember : this.SlackMember;
        }
    }
}
