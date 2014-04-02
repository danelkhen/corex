using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using Corex.Reflection;

namespace System.Reflection
{
    public static partial class Extensions
    {
        public static InstanceProperty<T> ForInstance<T>(this PropertyInfo pe, T obj)
        {
            return new InstanceProperty<T>(obj, pe);
        }
        public static IEnumerable<InstanceProperty<T>> ForInstance<T>(this IEnumerable<PropertyInfo> list, T obj)
        {
            return list.Select(t => t.ForInstance(obj));
        }
        public static IEnumerable<PropertyInfo> Properties<T>(this IEnumerable<InstanceProperty<T>> list, T obj)
        {
            return list.Select(t => t.Property);
        }
        public static bool Implements<T>(this Type type)
        {
            return type.GetInterface(typeof(T).FullName) != null;
        }
        public static IEnumerable<T> CustomAttributes<T>(this ICustomAttributeProvider target) where T : Attribute
        {
            return target.GetCustomAttributes(typeof(T), false).Cast<T>();
        }

        public static T CustomAttribute<T>(this ICustomAttributeProvider target) where T : Attribute
        {
            return target.CustomAttributes<T>().FirstOrDefault();
        }

        public static InstanceProperty<T, R> InstancePropertyOf<T, R>(this T obj, Expression<Func<T, R>> prop)
        {
            return new InstanceProperty<T, R>(obj, (PropertyInfo)((MemberExpression)prop.Body).Member);
        }
        public static IEnumerable<InstanceProperty<T>> InstanceProperties<T>(this T obj)
        {
            foreach (var pi in obj.GetType().GetProperties())
            {
                yield return new InstanceProperty<T>(obj, pi);
            }
        }

        public static Type GetEnumerableItemType(this Type type)
        {
            var iface = type.GetInterface("System.Collections.Generic.IEnumerable`1");
            if (iface != null)
            {
                var args = iface.GetGenericArguments();
                return args[0];
            }
            return null;
        }

    }
}
