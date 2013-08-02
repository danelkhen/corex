using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Corex.Reflection
{
    public interface IInstanceProperty
    {
        object Instance { get; }
        object Value { get; set; }
    }
    public interface IInstanceProperty<T> : IInstanceProperty
    {
        new T Instance { get; }
        new object Value { get; set; }
    }
    public interface IInstanceProperty<T, V> : IInstanceProperty<T>
    {
        new V Value { get; set; }
    }
    public class InstanceProperty : IInstanceProperty
    {
        public InstanceProperty(object obj, PropertyInfo pi)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (pi == null)
                throw new ArgumentNullException("pi");
            Instance = obj;
            Property = pi;
        }
        public object Instance { get; private set; }
        public PropertyInfo Property { get; private set; }
        public object Value
        {
            get
            {
                return Property.GetValue(Instance);
            }
            set
            {
                Property.SetValue(Instance, value);
            }
        }
    }
    public class InstanceProperty<T> : InstanceProperty, IInstanceProperty<T>
    {

        public InstanceProperty(T obj, PropertyInfo pi) : base(obj, pi)
        {
            Instance = obj;
        }
        public new T Instance { get; private set; }
    }

    public class InstanceProperty<T, V> : InstanceProperty<T>, IInstanceProperty<T, V>
    {
        public InstanceProperty(T obj, PropertyInfo pi) : base(obj, pi)
        {
        }
        public new V Value
        {
            get
            {
                return (V)Property.GetValue(Instance);
            }
            set
            {
                Property.SetValue(Instance, value);
            }
        }

    }
}
