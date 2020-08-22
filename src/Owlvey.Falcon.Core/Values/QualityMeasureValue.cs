using Owlvey.Falcon.Core.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Owlvey.Falcon.Core.Values
{
    public class QualityMeasureValue
    {   
        public int Good { get; protected set; }
        public int Total { get; protected set; }

        public int Delta { get { return this.Total - this.Good;  } }
        public decimal Availability { get; protected set; }

        private decimal _Latency = 101;
        public decimal Latency {
            get {
                return _Latency;
            }
            protected set {
                if (value == 0) { return; }
                else { _Latency = value; }
            }
        }

        public decimal Experience { get; protected set; }

        public bool HasData { get; protected set; }

        public QualityMeasureValue(decimal availability, decimal latency, decimal experience, bool hasdata = true) {            
            this.Availability = availability;
            this.Latency = latency;
            this.HasData= hasdata;
            this.Experience = experience;
        }

        public QualityMeasureValue(InteractionMeasureValue availability, MeasureValue latency, MeasureValue experience)
        {
            this.HasData = availability.HasData || experience.HasData || latency.HasData;
            this.Availability = availability.Value;
            this.Total = availability.Total;
            this.Good = availability.Good;
            this.Latency = latency.Value;            
            this.Experience = experience.Value;
        }

        public QualityMeasureValue(bool hasdata = true)
        {
            this.Availability = 1;
            this.Latency = 0;
            this.HasData = hasdata;
            this.Experience = 1;
        }
        public QualityMeasureValue(decimal quality, bool hasdata = true) : this(quality, quality, quality, hasdata)
        {
            
        }
        public DebtMeasureValue MeasureDebt(SLOValue slo)
        {
            return new DebtMeasureValue()
            {
                Availability = QualityUtils.MeasureDebt(this.Availability, slo.Availability),
                Latency = QualityUtils.MeasureDebt(this.Latency, slo.Latency),
                Experience = QualityUtils.MeasureDebt(this.Experience, slo.Experience)
            };
        }


        public static QualityMeasureValue Merge(IEnumerable<QualityMeasureValue> measures) {
            var availability = QualityUtils.CalculateAverage(measures.Select(c => c.Availability));
            var latency = QualityUtils.CalculateAverage(measures.Select(c => c.Latency));
            var experience = QualityUtils.CalculateAverage(measures.Select(c => c.Experience));
            var total = measures.Sum(c => c.Total);
            var good = measures.Sum(c => c.Good);
            return new QualityMeasureValue(availability, latency, experience) { 
                 Total = total,
                 Good = good
            }; 
        }

    }
}
