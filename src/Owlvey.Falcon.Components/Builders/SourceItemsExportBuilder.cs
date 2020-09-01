using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Models.Migrate;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Owlvey.Falcon.Builders
{
    public class SourceItemsExportBuilder
    {
        public IEnumerable<SourceEntity> Sources { get; }
        public IEnumerable<SourceItemEntity> SourceItems { get; }
        public SourceItemsExportBuilder(
            IEnumerable<SourceEntity> sources,
            IEnumerable<SourceItemEntity> sourceItems)
        {
            Sources = sources;
            SourceItems = sourceItems;
        }

        public (ICollection<SourceLiteModel> sources, ICollection<SourceItemLiteModel> items) Execute()
        {
            var roots = new List<SourceLiteModel>();
            var items = new List<SourceItemLiteModel>();
            roots.AddRange(SourceLiteModel.Load(this.Sources));
            foreach (var source in this.Sources)
            {
                source.SourceItems = this.SourceItems.Where(c => c.SourceId == source.Id).ToList();
                items.AddRange(SourceItemLiteModel.Loads(source.Name, source.SourceItems));
            }

            return (roots, items);
        }


    }
}
