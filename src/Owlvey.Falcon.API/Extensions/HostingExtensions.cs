using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Extensions
{
    public static class HostingExtensions
    {
        public static bool IsDocker(this IHostingEnvironment hosting)
        {
            return hosting.EnvironmentName.Equals("docker", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
