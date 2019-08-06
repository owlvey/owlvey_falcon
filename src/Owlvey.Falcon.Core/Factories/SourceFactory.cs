﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public static class Factory {

            public static SourceEntity Create(string name, DateTime on, string user)
            {
                string goodDefinition = "good definition of events";
                string totalDefinition = "total definition of events";
                var entity = new SourceEntity()
                {
                    Key = name,
                    GoodDefinition = goodDefinition,
                    TotalDefinition = totalDefinition,
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