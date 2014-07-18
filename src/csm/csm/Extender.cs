using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace csm
{
    public class ExpandoProvider
    {
        public static ExpandoProvider Current = new ExpandoProvider();
        public ExpandoProvider()
        {
            Table = new ConditionalWeakTable<object, Expando>();
        }
        internal ConditionalWeakTable<object, Expando> Table;
    }

    public static class ExpandoExtensions
    {
        public static Expando GetExtension(this object obj)
        {
            return ExpandoProvider.Current.Table.GetValue(obj, t => new Expando(obj));
        }
        public static T GetExpando<T>(this object obj, string name)
        {
            Expando ext;
            if (!ExpandoProvider.Current.Table.TryGetValue(name, out ext))
                return default(T);
            return (T)ext.Get<T>(name);
        }
        public static void SetExpando<T>(this object obj, string name, T value)
        {
            var ext = obj.GetExtension();
            ext.Set(name, value);
        }
    }


    public class Expando
    {
        public Expando()
        {

        }
        public Expando(object target)
        {
            Target = target;
        }
        public object this[string name]
        {
            get
            {
                if (Data == null)
                    return null;
                return Data.TryGetValue(name);
            }
            set
            {
                if (Data == null)
                    Data = new Dictionary<string, object>();
                Data[name] = value;
            }
        }
        public T Get<T>(string name)
        {
            if (Data == null)
                return default(T);
            return (T)Data.TryGetValue(name);
        }
        public void Set<T>(string name, T value)
        {
            if (Data == null)
                Data = new Dictionary<string, object>();
            Data[name] = value;
        }
        public object Target { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
