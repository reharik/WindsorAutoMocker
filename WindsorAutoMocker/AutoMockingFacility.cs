using Castle.Core.Configuration;
using Castle.MicroKernel;

namespace AutoMockingContainer
{
    public class AutoMockingFacility<ACTOR> : IFacility
  {
    #region Member Data
      private IAutoMockingContainer<ACTOR> _autoMock;
    #endregion

    #region AutoMockingFacility()
      public AutoMockingFacility(IAutoMockingContainer<ACTOR> autoMock)
    {
      _autoMock = autoMock;
    }
    #endregion

    #region IFacility Members
    public void Init(IKernel kernel, IConfiguration facilityConfig)
    {
        kernel.Resolver.AddSubResolver(new AutoMockingDependencyResolver<ACTOR>(_autoMock));
    }

    public void Terminate()
    {
    }
    #endregion

    #region Methods
    
    #endregion
  }
}
