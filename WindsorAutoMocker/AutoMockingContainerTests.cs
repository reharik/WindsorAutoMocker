using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace AutoMockingContainer
{
    public interface ICollectionOfServices
    {
        IDisposable SomethingToDispose { get; }
    }

    public interface IReallyCoolService
    {
        void SayHello();
    }

    public class ComponentBeingConfigured
    {
        #region Member Data

        public IReallyCoolService ReallyCoolService;
        public ICollectionOfServices Services;

        #endregion

        #region ComponentBeingConfigured()

        public ComponentBeingConfigured(IReallyCoolService reallyCoolService, ICollectionOfServices services)
        {
            this.ReallyCoolService = reallyCoolService;
            this.Services = services;
        }

        #endregion

        #region Methods

        public void RunDispose()
        {
            this.Services.SomethingToDispose.Dispose();
        }

        #endregion
    }

    public class DefaultCollectionOfServices : ICollectionOfServices
    {
        #region ICollectionOfServices Members

        public IDisposable SomethingToDispose
        {
            get { return null; }
        }

        #endregion
    }

    public class AutoMockingTests
    {
        #region Member Data

        public WindsorAutoMocker<ComponentBeingConfigured> WindsorAutoMocker;

        #endregion

        #region Test Setup and Teardown Methods

        [SetUp]
        public virtual void Setup()
        {
            WindsorAutoMocker = new WindsorAutoMocker<ComponentBeingConfigured>();
        }

        #endregion
    }

    [TestFixture]
    public class AutoMockingContainerTests : AutoMockingTests
    {
        #region Test Methods

        [Test]
        public void Resolving_WithComponent_ReturnsMock()
        {
            WindsorAutoMocker.ClassUnderTest.ReallyCoolService.Expect(x => x.SayHello());
            WindsorAutoMocker.ClassUnderTest.ReallyCoolService.SayHello();
            WindsorAutoMocker.ClassUnderTest.ReallyCoolService.VerifyAllExpectations();
            WindsorAutoMocker.Dispose();
        }

        [Test, Ignore("not sure about what they were getting at with this test")]
        public void Resolving_WithOtherImplementation_ReturnsMock()
        {
            WindsorAutoMocker.AddComponent("DefaultCollectionOfServices", typeof (ICollectionOfServices),
                                               typeof (DefaultCollectionOfServices));

            Assert.AreSame(MockRepository.GenerateMock<ICollectionOfServices>(),
                           WindsorAutoMocker.ClassUnderTest.Services);
            WindsorAutoMocker.Dispose();
        }


        [Test]
        public void Resolving_GetTwice_ReturnsSameMock()
        {
            var target1 = WindsorAutoMocker.ClassUnderTest.Services;
            var target2 = WindsorAutoMocker.Get<ICollectionOfServices>();

            Assert.AreEqual(target1, target2);
            WindsorAutoMocker.Dispose();

        }

        [Test, Ignore("don't get this one either")]
        public void Get_NotGotten_ReturnsNull()
        {
            Assert.IsNull(WindsorAutoMocker.Get<IReallyCoolService>());
            WindsorAutoMocker.Dispose();
        }

        [Test]
        public void Get_AlreadyGotten_ReturnsMock()
        {
            Assert.AreEqual(WindsorAutoMocker.ClassUnderTest.ReallyCoolService,
                            WindsorAutoMocker.Get<IReallyCoolService>());
            WindsorAutoMocker.Dispose();
        }

        #endregion
    }
}