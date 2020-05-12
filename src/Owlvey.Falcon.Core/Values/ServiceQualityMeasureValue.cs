using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class ServiceQualityMeasureValue : QualityMeasureValue
    {    
        public decimal? Slo { get; protected set; }

        public decimal ErrorBudget { 
            get {
                if (!this.Slo.HasValue)
                    throw new Exception("slo was not set");
                return QualityUtils.MeasureBudget(this.Quality, this.Slo.Value);
            } 
        }        
        public decimal Debt {
            get {
                return Math.Abs(this.ErrorBudget < 0 ? this.ErrorBudget : 0); 
            }
        }
        public decimal AvailabilityDebt
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Availability, this.Slo.Value);
                return Math.Abs(error < 0 ? error : 0);
            }
        }
        public decimal LatencyDebt
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Latency, this.Slo.Value);
                return Math.Abs(error < 0 ? error : 0);
            }
        }
        public decimal ExperienceDebt
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Experience, this.Slo.Value);
                return Math.Abs(error < 0 ? error : 0);
            }
        }
        public decimal Asset
        {
            get
            {
                return this.ErrorBudget > 0 ? this.ErrorBudget : 0;
            }
        }
        public ServiceQualityMeasureValue(decimal slo, decimal availability, decimal latency, decimal experience, bool hasdata = true) : base(availability, latency, experience,  hasdata)
        {
            this.Slo = slo;
        }
    }
}
