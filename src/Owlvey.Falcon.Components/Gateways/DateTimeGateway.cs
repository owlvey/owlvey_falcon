using System;
namespace Owlvey.Falcon.Gateways
{
    public class DateTimeGateway : IDateTimeGateway
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
