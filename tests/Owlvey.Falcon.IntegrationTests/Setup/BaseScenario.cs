using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    public abstract class BaseScenario
    {
        public object GetAdminToken()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.sub = "C675A96B-851E-49C2-BDB8-A6BB5AA41556";
            data.name = "C675A96B-851E-49C2-BDB8-A6BB5AA41556";
            data.fullname = "admin admin";
            data.email = "admin@owlvey.com";
            data.role = new List<string>() { "admin" };

            return data;
        }

        public object GetGuestToken()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.sub = "A9EC0802-AEF9-4D72-A686-9D4C110A3B94";
            data.name = "A9EC0802-AEF9-4D72-A686-9D4C110A3B94";
            data.fullname = "admin admin";
            data.email = "admin@owlvey.com";
            data.role = new List<string>() { "admin" };

            return data;
        }
    }
}
