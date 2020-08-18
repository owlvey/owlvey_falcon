using Owlvey.Falcon.Core.Entities;

public class SLAValue {
    public decimal? Availability { get; protected set; }
    public decimal? Latency { get; protected set; }    
    public SLAValue() { }
    public SLAValue(decimal? availability, decimal? latency) {
        this.Availability = availability;
        this.Latency = latency;        
    }
}