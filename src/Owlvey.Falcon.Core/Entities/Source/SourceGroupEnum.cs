using System;
namespace Owlvey.Falcon.Core.Entities
{
    public enum SourceGroupEnum
    {
        Availability, // The proportion of valid requests serverd successfully 
        Latency, // The proportion of valid requests serverd faster than a threshold        
        Quality, // The proportion of valid requests serverd without degrading quality
        Freshness, // The proportion of valid data updated more recently than a threshold
        Correctness, // The proportion of valid data producing correct output
        Coverage, // The proportion of valida data processed successfully
        Experience
    }
}
