using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Exceptions
{
    public class InvalidStateException: ApplicationException
    {
        public InvalidStateException(string message, Exception ex=null): base(message, ex) {

        }
    }
}
