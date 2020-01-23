using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class MemberModel
    {

        public string Organization { get; protected set; }
        public string Email { get; protected set; }
        public string Squad { get; protected set; }


        public void Load(string organization, string email, string squad)
        {
            this.Organization = organization;
            this.Email = email;
            this.Squad = squad;
        }
        public static IEnumerable<MemberModel> Load(string organization, string squad, IEnumerable<MemberEntity> entities)
        {
            var result = new List<MemberModel>();
            foreach (var item in entities)
            {
                var model = new MemberModel();
                model.Load(organization, item.User.Email, squad);
                result.Add(model);
            }
            return result;
        }
    }
}
