using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class AppSettingEntity
    {
        public static class Factory
        {
            /// <summary>
            /// Factory to create the initial instace.
            /// </summary>
            /// <param name="key">App Setting Key</param>
            /// <param name="value">App Setting Value</param>
            /// <param name="isReadOnly">Is ReadOnly</param>
            /// <param name="createdBy">Created By</param>
            /// <returns></returns>
            public static AppSettingEntity Create(
                string key,
                string value,
                bool isReadOnly, DateTime on, string user)
            {
                var entity = new AppSettingEntity()
                {
                    Key = key,
                    Value = value,
                    IsReadOnly = isReadOnly,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                entity.Validate();
                return entity;
            }

        }
    }
}
