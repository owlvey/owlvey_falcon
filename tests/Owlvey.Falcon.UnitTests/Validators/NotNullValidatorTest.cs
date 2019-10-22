using System;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Validators;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Validators
{
    public class NotNullValidatorTest
    {
        [Fact]
        public void ValidateSuccess()
        {
            try
            {
                NotNullValidator.Validate<SourceEntity>(null, c => c.Name, "test");
            }
            catch (ApplicationException ex)
            {
                Assert.Contains("Name", ex.Message);
            }
            

        }

    }
}
