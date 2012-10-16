using Castle.Core;
using Castle.MicroKernel;
using Rhino.Mocks;

namespace AutoMockingContainer
{
    public class AutoMockingDependencyResolver<ACTOR> : ISubDependencyResolver
  {
    #region Member Data
      private IAutoMockingContainer<ACTOR> _autoMock;
    #endregion

    #region Properties
      public IAutoMockingContainer<ACTOR> AutoMock
    {
      get { return _autoMock; }
    }
    #endregion

    #region AutoMockingDependencyResolver()
      public AutoMockingDependencyResolver(IAutoMockingContainer<ACTOR> autoMock)
    {
      _autoMock = autoMock;
    }
    #endregion

    #region ISubDependencyResolver Members
    public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model,
                           DependencyModel dependency)
    {
      return dependency.DependencyType == DependencyType.Service;
    }

    public object Resolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model,
                          DependencyModel dependency)
    {
        var target = AutoMock.Get(dependency.TargetType);
        if (target != null)
        {
            return target;
        }
        
        target = new MockRepository().DynamicMock(dependency.TargetType);
        target.Replay();
        this.AutoMock.AddService(dependency.TargetType, target);
        return target;
    }
    #endregion
  }
}
