using Owlvey.Falcon.Builders;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class ReliabilityRiskModel 
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public decimal ETTD { get; set; }
        public decimal ETTE { get; set; }
        public decimal ETTF { get; set; }
        public decimal UserImpact { get; set; }
        public decimal ETTFail { get; set; }
        public string Organization { get; set; }
        public string Product { get; set; }
        public string Source { get; set; }
        public static IEnumerable<(ReliabilityRiskPostRp, ReliabilityRiskPutRp)> Build(SheetRowAdapter adapter, IEnumerable<SourceEntity> sources)
        {
            var items = new List<(ReliabilityRiskPostRp, ReliabilityRiskPutRp)>();
            for (int row = 2; row <= adapter.getRows(); row++)
            {
                var result = new ReliabilityRiskPutRp();
                result.Name = adapter.get<string>(row, 1);
                result.Avatar = adapter.get<string>(row, 2);
                result.Reference = adapter.get<string>(row, 3);
                result.Description = adapter.get<string>(row, 4);
                result.Tags = adapter.get<string>(row, 5);
                result.ETTD = adapter.get<int>(row, 6);
                result.ETTE = adapter.get<int>(row, 7);
                result.ETTF = adapter.get<int>(row, 8);
                result.UserImpact = adapter.get<decimal>(row, 9);
                result.ETTFail = adapter.get<int>(row, 10);

                var organization = adapter.get<string>(row, 11);
                var product = adapter.get<string>(row, 12);
                var source = adapter.get<string>(row, 13); 

                var created = new ReliabilityRiskPostRp() {
                    Name = result.Name
                };
                created.SourceId = sources.Where(c => c.Name == source
                    && c.Product.Name == product && c.Product.Customer.Name == organization).Single().Id.Value;
                items.Add( (created, result ));
            }
            return items;
        }
    }
}
