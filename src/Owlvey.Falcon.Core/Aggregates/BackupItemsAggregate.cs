using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Models.Migrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Aggregates
{
    public class BackupItemsAggregate
    {
        public IEnumerable<SourceEntity> Sources { get; }
        public IEnumerable<SourceItemEntity> SourceItems { get; }
        public BackupItemsAggregate(
            IEnumerable<SourceEntity> sources,
            IEnumerable<SourceItemEntity> sourceItems) {
            Sources = sources;
            SourceItems = sourceItems;
        }

        public ICollection<SourceItemLiteModel> Execute() {
            var result = new List<SourceItemLiteModel>();

            foreach (var source in this.Sources)
            {
                source.SourceItems = this.SourceItems.Where(c => c.SourceId == source.Id).ToList();

                result.AddRange(SourceItemLiteModel.Loads(source.Name, source.SourceItems));                
            }

            return result;
        }


    }
}
