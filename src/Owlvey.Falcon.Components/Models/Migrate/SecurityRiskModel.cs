using Owlvey.Falcon.Builders;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Migrate
{
    public class SecurityRiskModel
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

        public string Organization { get; set; }
        public string Product { get; set; }
        public string Source { get; set; }

        public  static List<(SecurityRiskPost , SecurityRiskPut)> Build(SheetRowAdapter adapter, IEnumerable<SourceEntity> sources)
        {
            var items = new List<(SecurityRiskPost, SecurityRiskPut)>();
            for (int row = 2; row <= adapter.getRows(); row++)
            {
                var result = new SecurityRiskPut()
                {
                    Name = adapter.get<string>(row, 1),
                    Avatar = adapter.get<string>(row, 2),
                    Description = adapter.get<string>(row, 3),
                    Reference = adapter.get<string>(row, 4),
                    Tags = adapter.get<string>(row, 5),
                    AgentSkillLevel = adapter.get<int>(row, 6),
                    Motive = adapter.get<int>(row, 7),
                    Opportunity = adapter.get<int>(row, 8),
                    Size = adapter.get<int>(row, 9),
                    EasyDiscovery = adapter.get<int>(row, 10),
                    EasyExploit = adapter.get<int>(row, 11),
                    Awareness = adapter.get<int>(row, 12),
                    IntrusionDetection = adapter.get<int>(row, 13),
                    LossConfidentiality = adapter.get<int>(row, 14),
                    LossIntegrity = adapter.get<int>(row, 15),
                    LossAvailability = adapter.get<int>(row, 16),
                    LossAccountability = adapter.get<int>(row, 17),
                    FinancialDamage = adapter.get<int>(row, 18),
                    ReputationDamage = adapter.get<int>(row, 19),
                    NonCompliance = adapter.get<int>(row, 20),
                    PrivacyViolation = adapter.get<int>(row, 21)
                };

                var created = new SecurityRiskPost();
                created.Name = result.Name;
                var organization = adapter.get<string>(row, 22);
                var product = adapter.get<string>(row, 23);
                var source = adapter.get<string>(row, 24);
                created.SourceId = sources.Where(c => c.Name == source && 
                                    c.Product.Name == product &&
                                    c.Product.Customer.Name == organization).Single().Id.Value;
                items.Add( ( created, result ) );
            }
            return items;
        }
    }
}
