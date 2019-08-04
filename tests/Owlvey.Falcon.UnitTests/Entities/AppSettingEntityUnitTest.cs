using FizzWare.NBuilder;
using Owlvey.Falcon.Core.Entities;
using System;
using Xunit;

namespace Owlvey.Falcon.UnitTests.Entities
{
    public class AppSettingEntityTest
    {
        public AppSettingEntityTest()
        {

        }

        [Fact]
        public void CreateAppSettingEntitySuccess()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var id = Guid.NewGuid().ToString("n");
            var value = Faker.RandomNumber.Next().ToString();
            var isReadOnly = true;

            var appSettingEntity = AppSettingEntity.Factory.Create(id, value, isReadOnly, DateTime.UtcNow, createdBy);

            Assert.Equal(id, appSettingEntity.Key);
            Assert.Equal(value, appSettingEntity.Value);
            Assert.Equal(isReadOnly, appSettingEntity.IsReadOnly);
            Assert.Equal(createdBy, appSettingEntity.CreatedBy);
        }

        [Fact]
        public void CreateAppSettingEntityFail()
        {
            var createdBy = Guid.NewGuid().ToString("n");
            var id = string.Empty;
            var value = Faker.RandomNumber.Next().ToString();
            var isReadOnly = true;

            Assert.Throws<ApplicationException>(() =>
            {
                AppSettingEntity.Factory.Create(id, value, isReadOnly, DateTime.UtcNow, createdBy);
            });

        }
    }
}
