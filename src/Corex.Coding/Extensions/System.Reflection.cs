﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Reflection
{
    public static partial class Extensions
	{

		public static T GetCustomAttribute<T>(this ICustomAttributeProvider target) where T : Attribute
		{
			return (T)target.GetCustomAttributes(typeof(T), false).FirstOrDefault();
		}

        public static Type GetEnumerableItemType(object typedEnumerable)
        {
            var type = typedEnumerable.GetType();
            return GetEnumerableItemType(type);
        }
        public static Type GetEnumerableItemType(Type type)
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
