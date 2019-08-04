using FizzWare.NBuilder;
using MockQueryable.Moq;
using Moq;
using Owlvey.Falcon.Components.Gateways;
using Owlvey.Falcon.Components.Models;
using Owlvey.Falcon.Components.Services;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Owlvey.Falcon.ComponentsTests
{
    public class AppSettingComponentTest
    {
        public AppSettingComponentTest()
        {

        }

        [Fact]
        public void CreateAppSettingSuccess()
        {

           var appSettingModel = Builder<AppSettingPostRp>.CreateNew()
                                .With(x => x.Key = Guid.NewGuid().ToString("n"))
                                .With(x => x.Value = Guid.NewGuid().ToString("n"))
                                .Build();

            var mockAppSettingRepository = new Mock<IAppSettingRepository>();
            var appSettings = new List<AppSettingEntity>()
            {
                new AppSettingEntity(){ Key = "1", Value = "1" },
            };
            var mock = appSettings.AsQueryable().BuildMock();

            mockAppSettingRepository.Setup(x => x.GetAppSettingByKey(appSettingModel.Key)).Returns(Task.FromResult(mock.Object.FirstOrDefault(c => c.Key.Equals(appSettingModel.Key))));

            var mockIdentityService = new Mock<IUserIdentityService>();
            mockIdentityService.Setup(x => x.GetIdentity()).Returns("mock-user");

            var sut = new AppSettingComponent(mockAppSettingRepository.Object, mockIdentityService.Object);
            var result = sut.CreateAppSetting(appSettingModel).Result;

            Assert.True(!result.HasConflicts());
        }

        [Fact]
        public void CreateAppSettingFail()
        {

            var appSettingModel = Builder<AppSettingPostRp>.CreateNew()
                                 .With(x => x.Key = Guid.NewGuid().ToString("n"))
                                 .With(x => x.Value = Guid.NewGuid().ToString("n"))
                                 .Build();

            var mockAppSettingRepository = new Mock<IAppSettingRepository>();
            var appSettings = new List<AppSettingEntity>()
            {
                new AppSettingEntity(){ Key = appSettingModel.Key, Value = "1" },
            };
            var mock = appSettings.AsQueryable().BuildMock();

            mockAppSettingRepository.Setup(x => x.GetAppSettingByKey(appSettingModel.Key)).Returns(Task.FromResult(mock.Object.FirstOrDefault(c => c.Key.Equals(appSettingModel.Key))));

            var mockIdentityService = new Mock<IUserIdentityService>();
            mockIdentityService.Setup(x => x.GetIdentity()).Returns("mock-user");

            var sut = new AppSettingComponent(mockAppSettingRepository.Object, mockIdentityService.Object);
            var result = sut.CreateAppSetting(appSettingModel).Result;

            Assert.True(result.HasConflicts());
        }
    }
}
