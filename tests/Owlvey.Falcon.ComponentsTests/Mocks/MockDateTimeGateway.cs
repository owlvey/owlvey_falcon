using System;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Gateways;

namespace Owlvey.Falcon.ComponentsTests.Mocks
{
    public class MockDateTimeGateway : IDateTimeGateway
    {
        public DateTime GetCurrentDateTime() {
            var target = DateTime.Now;
            return new DateTime(2019, 1, 1, target.Hour, target.Minute, target.Second, target.Millisecond);
        }        
    }
}
