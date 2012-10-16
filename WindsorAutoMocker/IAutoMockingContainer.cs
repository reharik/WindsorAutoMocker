using System;

namespace AutoMockingContainer
{
    public interface IAutoMockingContainer<ACTOR>
    {
        ACTOR ClassUnderTest { get; set; }
        void AddService(Type type, object service);
        object Get(Type type);
        T Get<T>();
        void Dispose();
        void AddComponent(string defaultcollectionofservices, Type abstractType, Type concrete);
    }
}