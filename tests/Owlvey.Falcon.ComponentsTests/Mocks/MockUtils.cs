using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.ComponentsTests.Mocks
{
    public class MockUtils
    {
        public static string GenerateRandomName() {
            return Guid.NewGuid().ToString().Substring(13);
        }
    }
}
