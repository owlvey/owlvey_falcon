using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Exports
{
    public class AnnualAvailabilityInteractionsItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        

        public int JanGood { get; set; }
        public int JanTotal { get; set; }
        public decimal JanProportion { get; set; }
        public int FebGood { get; set; }
        public int FebTotal { get; set; }
        public decimal FebProportion { get; set; }
        public int MarGood { get; set; }
        public int MarTotal { get; set; }
        public decimal MarProportion { get; set; }
        public int AprGood { get; set; }
        public int AprTotal { get; set; }
        public decimal AprProportion { get; set; }
        public int MayGood { get; set; }
        public int MayTotal { get; set; }
        public decimal MayProportion { get; set; }
        public int JunGood { get; set; }
        public int JunTotal { get; set; }
        public decimal JunProportion { get; set; }
        public int JulGood { get; set; }
        public int JulTotal { get; set; }
        public decimal JulProportion { get; set; }

        public int AugGood { get; set; }
        public int AugTotal { get; set; }
        public decimal AugProportion { get; set; }

        public int SepGood { get; set; }
        public int SepTotal { get; set; }
        public decimal SepProportion { get; set; }

        public int OctGood { get; set; }
        public int OctTotal { get; set; }
        public decimal OctProportion { get; set; }

        public int NovGood { get; set; }
        public int NovTotal { get; set; }
        public decimal NovProportion { get; set; }

        public int DecGood { get; set; }
        public int DecTotal { get; set; }
        public decimal DecProportion { get; set; }

        public AnnualAvailabilityInteractionsItemModel(int id , string name) {
            this.Id = id;
            this.Name = name;            
        }
                
        public void LoadData(int month, int good, int total) {
            switch (month)
            {
                case 1:
                    this.JanGood = good;
                    this.JanTotal = total;
                    this.JanProportion = QualityUtils.CalculateProportion(total, good); 
                    break;
                case 2:
                    this.FebGood = good;
                    this.FebTotal = total;
                    this.FebProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 3:
                    this.MarGood = good;
                    this.MarTotal = total;
                    this.MarProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 4:
                    this.AprGood = good;
                    this.AprTotal = total;
                    this.AprProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 5:
                    this.MayGood = good;
                    this.MayTotal = total;
                    this.MayProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 6:
                    this.JunGood = good;
                    this.JunTotal = total;
                    this.JunProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 7:
                    this.JulGood = good;
                    this.JulTotal = total;
                    this.JulProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 8:
                    this.AugGood = good;
                    this.AugTotal = total;
                    this.AugProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 9:
                    this.SepGood = good;
                    this.SepTotal = total;
                    this.SepProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 10:
                    this.OctGood = good;
                    this.OctTotal = total;
                    this.OctProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 11:
                    this.NovGood = good;
                    this.NovTotal = total;
                    this.NovProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                case 12:
                    this.DecGood = good;
                    this.DecTotal = total;
                    this.DecProportion = QualityUtils.CalculateProportion(total, good);
                    break;
                default:
                    break;
            }

        }
        
    }
}
