using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace AutoMockingContainer
{
    public class WindsorAutoMocker<ACTOR> : IAutoMockingContainer<ACTOR>
    {
        #region Member Data

        private readonly WindsorContainer _container;
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        #endregion

        public WindsorAutoMocker()
        {
            _container = new WindsorContainer();
            _container.Kernel.AddFacility("AutoMockingFacility", new AutoMockingFacility<ACTOR>(this));
            Type targetType = typeof(ACTOR);
            _container.AddComponent(targetType.FullName, targetType);
            ClassUnderTest = _container.Resolve<ACTOR>();
        }

        public ACTOR ClassUnderTest { get; set; }
      
        public virtual void AddService(Type type, object service)
        {
            _services[type] = service;
        }

        public virtual object Get(Type type)
        {
            if (_services.ContainsKey(type))
                return _services[type];
            return null;
        }

        public virtual T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public void AddComponent(string defaultcollectionofservices, Type abstractType, Type concrete)
        {
            _container.AddComponent(defaultcollectionofservices, abstractType, concrete);
            _services[abstractType] = null;
        }
       
    }
}