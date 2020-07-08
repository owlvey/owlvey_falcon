using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API
{
    public static class WebMetrics
    {
        public static readonly Counter ExceptionCounter = Metrics.CreateCounter(
            "owlvey_web_exception_counter",
            "Count web server exceptions", configuration: new CounterConfiguration() { 
                 
            });
    }
}
