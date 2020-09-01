using Microsoft.EntityFrameworkCore.Storage;
using Owlvey.Falcon.Builders;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SecurityThreatModel
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Tags { get; set; }

        #region Likehood 

        #region threat agent factors
        public int AgentSkillLevel { get; set; } = 0;
        public int Motive { get; set; } = 0;
        public int Opportunity { get; set; } = 0;
        public int Size { get; set; } = 0;

        #endregion

        #region vulnerability factors
        public int EasyDiscovery { get; set; } = 0;
        public int EasyExploit { get; set; } = 0;
        public int Awareness { get; set; } = 0;
        public int IntrusionDetection { get; set; } = 0;

        #endregion

        #endregion

        #region Impact

        #region Technical Impact
        public int LossConfidentiality { get; set; } = 0;
        public int LossIntegrity { get; set; } = 0;
        public int LossAvailability { get; set; } = 0;
        public int LossAccountability { get; set; } = 0;
        #endregion

        #region Business Impact

        public int FinancialDamage { get; set; } = 0;
        public int ReputationDamage { get; set; } = 0;
        public int NonCompliance { get; set; } = 0;
        public int PrivacyViolation { get; set; } = 0;

        #endregion
        #endregion


        public static List<SecurityThreatPutRp> Build(SheetRowAdapter adapter) {
            var items = new List<SecurityThreatPutRp>();
            for (int row = 2; row <= adapter.getRows(); row++)
            {
                var result = new SecurityThreatPutRp();
                result.Name = adapter.get<string>(row, 1);
                result.Avatar = adapter.get<string>(row, 2);
                result.Description = adapter.get<string>(row, 3);
                result.Reference = adapter.get<string>(row, 4);
                result.Tags = adapter.get<string>(row, 5);
                result.AgentSkillLevel = adapter.get<int>(row, 6);
                result.Motive = adapter.get<int>(row, 7);
                result.Opportunity = adapter.get<int>(row, 8);
                result.Size = adapter.get<int>(row, 9);
                result.EasyDiscovery = adapter.get<int>(row, 10);
                result.EasyExploit = adapter.get<int>(row, 11);
                result.Awareness = adapter.get<int>(row, 12);
                result.IntrusionDetection = adapter.get<int>(row, 13);
                result.LossConfidentiality = adapter.get<int>(row, 14);
                result.LossIntegrity = adapter.get<int>(row, 15);
                result.LossAvailability = adapter.get<int>(row, 16);
                result.LossAccountability = adapter.get<int>(row, 17);
                result.FinancialDamage = adapter.get<int>(row, 18);
                result.ReputationDamage = adapter.get<int>(row, 19);
                result.NonCompliance = adapter.get<int>(row, 20);                
                result.PrivacyViolation = adapter.get<int>(row, 21);
                items.Add(result);
            }
            return items;
        }
    }
}
