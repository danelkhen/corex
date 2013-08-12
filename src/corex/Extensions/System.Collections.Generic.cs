using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class Extensions
    {
        public static T GetOrAdd<K, T>(this IDictionary<K, T> dic, K key, Func<K, T> func)
        {
            T value;
            if (dic.TryGetValue(key, out value))
                return value;
            value = func(key);
            dic.Add(key, value);
            return value;
        }

        public static void InsertRange<T>(this IList<T> set, int index, IEnumerable<T> list)
        {
            foreach (T item in list)
            {
                set.Insert(index, item);
                index++;
            }
        }
        public static void SetItems<T>(this IList<T> list, IEnumerable<T> items)
        {
            list.Clear();
            list.AddRange(items);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.IsEmpty();
        }
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && !list.IsEmpty();
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this IList<T> list)
        {
            return list != null && list.Count > 0;
        }
        public static T GetValue<K, T>(this Dictionary<K, T> dic, K key, Func<K, T> notFoundHandler)
        {
            T value;
            if (!dic.TryGetValue(key, out value))
            {
                return notFoundHandler(key);
            }
            return value;
        }

        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
                return false;
            dic.Add(key, value);
            return true;
        }

        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            if (dic.ContainsKey(key))
            {
                dic.Remove(key);
                return true;
            }
            return false;
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            dic.TryGetValue(key, out value);
            return value;
        }

        public static T TryGet<T>(this IList<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return default(T);
            return list[index];
        }
        public static T GetOrFirstOrDefault<T>(this IList<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return list.FirstOrDefault();
            return list[index];
        }
        public static T GetOrLastOrDefault<T>(this IList<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return list.LastOrDefault();
            return list[index];
        }
        public static T GetOrFirst<T>(this IList<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return list.First();
            return list[index];
        }
        public static T GetOrLast<T>(this IList<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return list.Last();
            return list[index];
        }

        public static T GetCreate<K, T>(this Dictionary<K, T> dic, K key) where T : class, new()
        {
            var value = dic.TryGetValue(key);
            if (value == null)
            {
                value = new T();
                dic[key] = value;
            }
            return value;
        }

        public static Dictionary<string, string> ToDictionary(this NameValueCollection list)
        {
            var dic = new Dictionary<string, string>();
            foreach(string key in list.Keys)
                dic.Add(key, list[key]);
            return dic;
        }


    }
}
