using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Gateways;
using Owlvey.Falcon.Models;
using Owlvey.Falcon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Components
{
    public class SecurityRiskComponent: BaseComponent
    {
        public static void ConfigureMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SecurityRiskEntity, SecurityRiskGetRp>()
                .ForMember(c=>c.Source, c=>c.MapFrom(d=> d.Source != null? d.Source.Name : ""));
            cfg.CreateMap<SecurityThreatEntity, SecurityThreatGetRp>();
            
        }

        private readonly FalconDbContext _dbContext;

        public SecurityRiskComponent(FalconDbContext dbContext,
            IUserIdentityGateway identityGateway, IDateTimeGateway dateTimeGateway,
            IMapper mapper, ConfigurationComponent configuration) : base(dateTimeGateway, mapper, identityGateway, configuration)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<SecurityRiskGetRp>> GetRisks(int? sourceId) {
            if (sourceId.HasValue)
            {
                var response = await this._dbContext.SecurityRisks.Include(c=>c.Source)
                    .Where(c => c.SourceId == sourceId).ToListAsync();
                return this._mapper.Map<IEnumerable<SecurityRiskGetRp>>(response);
            }
            else {
                var response = await this._dbContext.SecurityRisks.Include(c=>c.Source).ToListAsync();
                return this._mapper.Map<IEnumerable<SecurityRiskGetRp>>(response);
            }
            
        }
        public async Task<SecurityRiskGetRp> GetRiskById(int id)
        {
            var response = await this._dbContext.SecurityRisks
                .Include(c=>c.Source)
                .Where(c => c.Id == id).SingleOrDefaultAsync();
            return this._mapper.Map<SecurityRiskGetRp>(response);
        }
        public async Task<SecurityRiskGetRp> Create(SecurityRiskPost model)
        {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            var item = SecurityRiskEntity.Factory.Create(
                model.SourceId,                 
                modifiedOn, modifiedBy, model.Name,
                model.Description, model.Tags, model.Reference, model.Avatar, 
                model.AgentSkillLevel, model.Motive, model.Opportunity, model.Size
                , model.EasyDiscovery, model.EasyExploit, model.Awareness, model.IntrusionDetection, 
                model.LossConfidentiality, model.LossIntegrity, model.LossAvailability, model.LossAccountability, model.FinancialDamage,
                model.ReputationDamage, model.NonCompliance, model.PrivacyViolation);

            this._dbContext.SecurityRisks.Add(item);
            await this._dbContext.SaveChangesAsync();
            return this._mapper.Map<SecurityRiskGetRp>(item);
        }
        public async Task<SecurityRiskGetRp> UpdateRisk(int id, SecurityRiskPut model)
        {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            
            var item = this._dbContext.SecurityRisks.Where(c => c.Id == id).SingleOrDefault();
            if (item != null) {

                item.Update(modifiedOn, modifiedBy, model.Name, model.Description, model.Tags, model.Reference, model.AgentSkillLevel,
                    model.Motive, model.Opportunity, model.Size, model.EasyDiscovery, model.EasyExploit, model.Awareness, model.IntrusionDetection, model.LossConfidentiality, model.LossIntegrity, model.LossAvailability, model.LossAccountability, model.FinancialDamage, model.ReputationDamage, model.NonCompliance, model.PrivacyViolation);
                this._dbContext.SecurityRisks.Update(item);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<SecurityRiskGetRp>(item);
        }
        public async Task DeleteRisk(int id)
        {
            var item = this._dbContext.SecurityRisks.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                this._dbContext.SecurityRisks.Remove(item);
                await this._dbContext.SaveChangesAsync();
            }            
        }

        public async Task<IEnumerable<SecurityThreatGetRp>> GetThreats()
        {
            var items = await  this._dbContext.SecurityThreats.ToListAsync();
            var result = this._mapper.Map<IEnumerable<SecurityThreatGetRp>>(items);
            return result;
        }
        public async Task<SecurityThreatGetRp> GetThreat(int id)
        {
            var item = await this._dbContext.SecurityThreats.Where(c => c.Id == id).SingleOrDefaultAsync();
            return this._mapper.Map<SecurityThreatGetRp>(item);
        }
        public async Task CreateDefault()
        {
            var spoofing = new SecurityThreatPostRp();
            spoofing.Name = "Spoofing";
            var spoofingCreated = await this.CreateThreat(spoofing);
            await this.UpdateThreat(spoofingCreated.Id, new SecurityThreatPutRp() { 
                Description = "spoofing attack is a situation in which a person or program successfully identifies as another by falsifying data, to gain an illegitimate advantage",
                Avatar = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAMAAAAJbSJIAAAAflBMVEX///8AAADu7u6QkJCWlpbMzMzY2Nj8/Pw3Nzfc3Nx7e3uqqqrz8/P5+fn29vZQUFC4uLjl5eXi4uK/v79WVlbS0tJjY2Nubm6mpqagoKAyMjKwsLA6OjrFxcUYGBhfX18hISFGRkaBgYEpKSlISEgZGRkQEBB0dHSIiIglJSVRX9kXAAAOOUlEQVR4nO1d63qiMBCteEEtKOKtSm1FW3Tf/wVXzWUmkIQkoNDv4/zZXZdLTi6TM5NJeHvr0KFDhw4dOnTo0KFDhw4dOjjDHx2i6KM/iKLl0Gu6MHXDH+2+eyLG1yhoulh1YXa49ORIB3HThasBi/5Zwe+B1bLpAlaEp2o+wD5qupAVEF5L+T04rpsuqCsORyOCN0xnTZfVBX6ug+6nu+XIu2MxTPob8T+Pw6aLa4+F0ICXg5+/IN6l+IpdE4WsgiEq/FxlL+P+F1y1LdRBq7GEkn/r7EiwgwtX4cuKVx0JL3ZWNsBmMFzTl5StFhx4oU8GV6+5JPh5eslqwpoTNBMsITes2yeXrCZ4GZsCjFVnn1EcPLNgteGTlvZs4TsMGMU2zIvhLLjN2sFMZflOtKx7K53CbarCc/RnM+/xWsvS2mIRnbZjMpUfx/PtdLAcLXKXsIkws/T+WEf9zv3uTdYfp+13+o+89ry67EbPmlYWfdb/BOzn1+WEX+Tv6c8TzZOk2NIbwdUIDu+bz1/JK/9tnuFzDecyehzzAdFlbEDZu0QhrZvs0UKL5JIpX3bHoOb+OvzRvo5gNRjF9K8Xh3dM6L19L7l8qd6BcK2xswbb8vcJ+HJ6eb/8wQKOtfnOkeWbXfroA7Ixp8WmHrV+Eh56TLf9G6ab+edZNVJcBeZB8byb4U43j9ee5nvxf0bV+c1W6Hnz3QT3v5sl301XxTHj/NpV4VHppr8cxdiqLA7CKK1sVX1kQk9yFRYskmuKSzV3ftsSP+brNvN50m4YRKgl353fRgBF32qncP8wGLMrD85v47Npdon0YfEIRkg1e8MnCZN4WLC83B2hfoX3xXflsvow6Ob+tFe9RkFnGIcY/NEhL+PsEK6NHwAm3j1yzl31djo1MeupY+cnMIJJneWqER6LDrhIqDuY0G5voG/BNIKbW/lB757WXKw6wbTs3kXcBLSXtzsCxprhw+FetqpSzTY+HdTc2zrcN3gVKueVYOW0N/fUlf20vC08pT/upje+pMolABVYP7V+maNeINLNTSvGG6e5yXFKi9zMDNMIXzvrcTHiuQyWN1K5vre8be7WhBCrygZWHNcoV8N2cqPreHaxrwW56Wz5rriHkF2Ng0VimGtj+daBi61J3AxUPt5xNcoMOuSilP8s3+q5GEXqbNuK9mLEalo6nS7Twk22sdaV/W0eiRNYy5liYW8ctdWUyKLMtrkZtMfZTN10fczalyUu+iYSQzdbZTsm/4QLL7SX25oaajVs1ub6bnVJGd7eFI2Fom9lTnv4LrbcZcEslfV0SryoX4soLR1P1oFzwuuxuJIbXps8x+BdbL/TvaEnjjVLA57mGY8+mWHsvQrCak/qMmciV7jvhQOxI19JR07ceimz4eZOwoxWq+2L3qbim3KrOSlrR38g/N7rs+5Fsxas4y7+4zaL6YIaGvswHa1LsFBDMcZLVbVA/BfET0B+sdUZ9/fcnNmxRcU4mjRwZdCQH+H8LhIywnHfbIcGO10Idom6BMO1zWoQdUgcUpVTSf+eIMn5CDagjMVEWCToudasNWgRHO5ksk209wumdYj+Z22YiQ7PjAa8j26FtgLpWJnLrWweFOJzARuNhJI/Fv5J4bH4+isyan/QoLEEXyD7BtPN1xiYseMDEamdJH/RU0Ekgq0TQwCrjdvlfSCvd5DnzfnAmsPmsQIzgouyl0S+/rkznEHKVwE/uwGFZj0bLqofHzxPhbzLYQ0pOOkTKBrHeedXYrhsOb87zpMKDBP9s9uCoTNDzQhsFxauDK1zRZrCxpHhu/6xbULsxpBKks9D4LUVTFX0nRjSzOB/rd5yNyLMUieGNMTSzhV/DiqQnRjS6qmS+fEC9KszbEOetgaMIQlE2a3f/y2GLr5Fx7Ad6Bjq8LcYunjAf4shCcfn93boMflTDElM2iYSFXg0upTEE4y2nQ7hxHCUTOefyl3b1+eW2BYDW4a+8kAIjlZlbrJYJ9WnpQkq3kBNjOMl4U9D8A1ybJFLf3lgtsXFOX+3fkCU1oihcvdHDlXy2muFj7ZuMbKay2emO6FWL2NQghhHOll/1VwuJiKcV9tT/12CpDVnegh7VcoZ4gMTepfIa/+5HbueFUNUH+O/cbjMtJdjSAODilUgILj/GwcEBZA3MDZhOOKXX//GWR0xCK4h0zR0rVpqJgJ+ecuDTgwwrf1OuC6l/VDmJ8DusmIL+0U8n0AZwMbMZ6C8qa2UjTI2ZrOi1yDuNW2JJQLd/MhhYQzpUJOUjuVafBVbUKHBG3UYkY0hqROMIU1sKboFLONFsg9WSIBGaPJElgWkEFCjwRjSvLZiEiSrEkn/nfTksE2hrxEgTH5Zl2MMQ/JnQTSzPipzawPFIQHN7Qv74GWY8+UiMlv8YwzzhWN7veQun3zX/m9ja1GgY1CeHMkm3r35JNiWD0UxY6nIAYx30zxOSVOnWyFfSUjPuh/ZdO+bJNiWMxJspLV8Ae2BuGBjOMg0Jw1jrHR9tF0AHXNWbFEgc9tR+I2lWrQ8InoH6JhUZQbIiMsEvy/tSQenKXzlsUu1A2yM2pDTBFOszFjDO+59j46vGsABJF5rtmjTZsZ9mDahfXb7A0QHbl7QjAvY5KDzXovuE9MHmtzIJB3nsd/QJqeidfz0KD/omExuY4LD4/di2sGgtAkV4VNCkaez13CejA6gY37kwaO7cz/33yS7c5LiyBTh9eT4xrf3npzjLNUxGGRCuPKtUkh/EtmpCe+OenLQzRMQX33eWk1YbmNIR8tufEhoA28G8+a9TGcMVQzZOmu/8EvdiA1sDPMtmGyzCVirGPLZBZT551NSuUHHfKlnNGBI2ttmaxdLGZtjrPDBS5B/mj0hFA6pkXONwQaGZNj8Whh3g3V8D+Rw7fYGbRHQhb+AIf2bRXcyylSAPVD12psQfCW9cAKGdNBY7B02y8UAe7OtUd/EcPRXSRQeGA6tO5NhtglMjJ+16RsUjylrEmA4MWlyAab5NMje1KRvwMZ8loZMgCHZlEm9j/VpW55vYJwxFMDaYy1ZDGBjDPYtAkOagPk4Pe8R4y/dCW6eEwVHP9fgT81gc6PJcVaIIZkQs7vlJT+WdSmbrC/YYllV3xRjvnoghvSvAdcKZTVkldcG9mZfyd7AoD6amX3EMOEVszWrIrvMPbS/poK+Qb6SYVgWMaQW+IPFso9lj7DMTVyAvXHWN0jHmN6CGNLRd2KdtNRO2WZf+mAi3PSND76SucHCDOmp3Sz+UFpy+/zSavYGfSHEIpsAM6Qm3SdrFVnpaq5DBi3Ym9Tan0K+ks2xNJghDcz0DTupU44wMoWW+gZ0TGpljDFDMYmofLJxyoJegGS2sjfQwS2/3YIZBpigweEDbnnevou+CV1sDAFmGOLUNQNr7JrJjvwpw+aoMtFghsKpVgZPcs7VR/6U0bS9hiRD668TiAw/EEOD1DxLhsj/tSsyqhBUKlN3WmCIUg9M1pusGEb73g+oNc/C3iAbAz8m597KzBgLDEMIP5r4Jfa+BcyBIegbvemQX5iYj0mB4Rs8zWRFzZwh02t4cc9M33hyX2lvUjkEIkPe442S0Y0Z8mIK8WYYXur1KYWvFPJ+W26MRYYLdqdRSowpQwgbfSh+V8VvQMf8iJXAO1t5cEtkyE7NNPOerWNt+Z6B3HXpkFL7SmATLWJtD7ANI0bTlBlDKGZxvCEzUlzj0uoY82B6jiGtG7PDNk0Yajm8CfxzQ6pEwBr7ijmGdKelWXKCAcNyuaUaauWD1DCmmGdI/23kg5czNInNwDXY7TMJXKHK0SiwPEO62H002U9RytAsnu9JliCkOqYAVIFqe5NnyBK9TGLTZQz7RsUUxup2chuNs2HKf9CPMTQIDNaAKegSlEkWrJ4h8gNLt3qhI/k+Vyv0RegyXTYrtzcFhjRKY9KIWoaelUunOI3JwPGAXAxFRykwtDjSXccQDREjF2AoOVHLzHlU6h6KIkO6BmVgTjV7uVGOgmHYaJH7bpV5AADmFWlwq8iQ65qyLsJOPZQwRHkmxuu++Q9Cmx/zrI+jShj69Hyrsb4OedUVGPolOkaFNTrb9WITMdTqGwlD3sVMpIKEIZrfbNdD42Q7T39W06VtMjwY40u+XWQM+bfQ1C0Qox22OYZrvkfhGTk0KkCIKZ8jLGXI3UTVHCPsWRcZgo2ptk5oC2V+lJQhFFOaILIUj+UWGNa31msLlbMpZwjz6DnvC8+i/NdJEcPQQsfUjhAKhvuegiH6LOEqgnYMh1Ox/USGXiqvxldB6k+pGOKPIPbS63uSfOw2SDDeMc0xRLmKDW1ZR/aGO0cqhrKPS4qYe7HIEGyM/dpgXQB9w51NNcPCfm8BX0lelyIb09TOpzdZvpuGoe6Dstf7pCMwfFYOoi2Ue0ilmMmbMbuSSRUx9GrO66oC0LcPf0rL8DbJXPO2M/uOmC4ChrCW9EodowLay+2VMryfZd3/Zjpsv+kPkVDhDJvSMSqAvTlPyhne4YdhPAnDvAGhDNeg4b4btDEYC/CnWemcnkMZgvVqzdE72J/aV2fI0biNwcgfhuD0kBzDFtgYjPe6GbbDxmAc6mVY00fda0V8rpFhe2wMxuynNoatsjEY05oYPnkPZRXs6mDYnK9kgkNWnWHLd+sbqTYFOobtQMdQh45hO9Ax1KFj2A50DHWg6/hu341/GaowpFH95o5nMwLdOOp074zGUlutvPm3gpxAI/nnQ9NfdVKDRWsctx+bHvDdAtjvRCEoZPm0Fa7HlSlPv2wbJIfLmuKPfP+wSphlWf74xqE6ZtAQgews4TYh21UP5R6iflsxiFq21tChQ4cOHTp06NChQ4cOHTo8Gf8BEHCxNsTWMCIAAAAASUVORK5CYII=",
                Tags = "#spoofing", 
                AgentSkillLevel = 3, FinancialDamage = 3, ReputationDamage = 4, NonCompliance = 5,
                PrivacyViolation = 3,
                Reference = "https://en.wikipedia.org/wiki/Spoofing_attack"  
            });

            var tampering = new SecurityThreatPostRp(); 



        }
        public async Task<SecurityThreatGetRp> CreateThreat(SecurityThreatPostRp model) {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            var exists = await this._dbContext.SecurityThreats.Where(c => c.Name == model.Name).SingleOrDefaultAsync();
            if (exists == null) {
                exists = SecurityThreatEntity.Create(model.Name, modifiedBy, modifiedOn);
                this._dbContext.Add(exists);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<SecurityThreatGetRp>(exists);
        }
        public async Task<SecurityThreatGetRp> UpdateThreat(int id, SecurityThreatPutRp model)
        {
            var modifiedBy = this._identityGateway.GetIdentity();
            var modifiedOn = this._datetimeGateway.GetCurrentDateTime();
            this._dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            var item = this._dbContext.SecurityThreats.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                item.Update(modifiedOn, modifiedBy, model.Name, model.Description, model.Tags, 
                    model.Reference, model.AgentSkillLevel, model.Motive, model.Opportunity, model.Size,
                    model.EasyDiscovery, model.EasyExploit, model.Awareness, model.IntrusionDetection, model.LossConfidentiality, model.LossIntegrity, 
                    model.LossAccountability, model.LossAccountability, model.FinancialDamage, model.ReputationDamage, model.NonCompliance, model.PrivacyViolation);
                this._dbContext.SecurityThreats.Update(item);
                await this._dbContext.SaveChangesAsync();
            }
            return this._mapper.Map<SecurityThreatGetRp>(item);
        }
        public async Task DeleteThreat(int id)
        {
            var item = this._dbContext.SecurityThreats.Where(c => c.Id == id).SingleOrDefault();
            if (item != null)
            {
                this._dbContext.SecurityThreats.Remove(item);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
