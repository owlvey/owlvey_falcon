using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ClueEntityFactory
    {
        public static class Factory
        {
            public static ClueEntity Create(string name, decimal value, DateTime on, string user,
                SourceItemEntity sourceItem)
            {
                var entity = new ClueEntity()
                {
                    Name = name,
                    Value = value,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    SourceItem = sourceItem
                };
                sourceItem.Clues.Add(entity);
                return entity;
            }
        }
    }
}
