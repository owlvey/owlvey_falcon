using Owlvey.Falcon.Builders;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class ReliabilityThreatModel
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

        public static IEnumerable<ReliabilityThreatPutRp> Build(SheetRowAdapter adapter) {
            var items = new List<ReliabilityThreatPutRp>();
            for (int row = 2; row <= adapter.getRows(); row++)
            {
                var result = new ReliabilityThreatPutRp();
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
                items.Add(result);
            }
            return items;
        }
    }
}
