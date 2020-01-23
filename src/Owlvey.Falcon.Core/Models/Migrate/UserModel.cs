using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class UserModel
    {

        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string SlackMember { get; set; }

        public void Load(UserEntity entity)
        {
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Email = entity.Email;
            this.SlackMember = entity.SlackMember;
        }
        public static IEnumerable<UserModel> Load(IEnumerable<UserEntity> entities)
        {
            var result = new List<UserModel>();
            foreach (var item in entities)
            {
                var model = new UserModel();
                model.Load(item);
                result.Add(model);
            }
            return result;
        }

    }
}
