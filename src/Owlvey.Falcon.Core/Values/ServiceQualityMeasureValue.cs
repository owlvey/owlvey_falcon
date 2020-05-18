using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Values
{
    public class ServiceQualityMeasureValue : QualityMeasureValue
    {    
        public decimal AvailabilitySlo { get; protected set; }

        public decimal LatencySlo { get; protected set; }

        public decimal ExperienceSlo { get; protected set; }

        public decimal AvailabilityErrorBudget { 
            get {                
                return QualityUtils.MeasureBudget(this.Availability, this.AvailabilitySlo);
            } 
        }

        public decimal LatencyErrorBudget
        {
            get
            {
                return QualityUtils.MeasureBudget(this.Latency, this.LatencySlo);
            }
        }

        public decimal ExperienceErrorBudget
        {
            get
            {
                return QualityUtils.MeasureBudget(this.Experience, this.ExperienceSlo);
            }
        }        
        public decimal AvailabilityDebt
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Availability, this.AvailabilitySlo);
                return Math.Abs(error < 0 ? error : 0);
            }
        }
        public decimal AvailabilityAsset
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Availability, this.AvailabilitySlo);
                return Math.Abs(error > 0 ? error : 0);
            }
        }
        public decimal LatencyAsset
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Latency, this.LatencySlo);
                return Math.Abs(error > 0 ? error : 0);
            }
        }
        public decimal ExperienceAsset
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Experience, this.ExperienceSlo);
                return Math.Abs(error > 0 ? error : 0);
            }
        }
        public decimal LatencyDebt
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Latency, this.LatencySlo);
                return Math.Abs(error < 0 ? error : 0);
            }
        }
        public decimal ExperienceDebt
        {
            get
            {
                var error = QualityUtils.MeasureBudget(this.Experience, this.ExperienceSlo);
                return Math.Abs(error < 0 ? error : 0);
            }
        }
        
        public ServiceQualityMeasureValue(
            decimal availabilitySlo,
            decimal latencySlo,
            decimal experienceSlo,
            decimal availability, 
            decimal latency, 
            decimal experience, 
            bool hasdata = true) : base(availability, latency, experience,  hasdata)
        {
            this.AvailabilitySlo = availabilitySlo;
            this.ExperienceSlo = experienceSlo;
            this.LatencySlo = latencySlo;
        }

        public ServiceQualityMeasureValue(
           decimal availabilitySlo,
           decimal latencySlo,
           decimal experienceSlo,           
           bool hasdata = true) : this (availabilitySlo, latencySlo, experienceSlo, 1 , 1 ,1 , hasdata)
        {
            
        }


    }
}
