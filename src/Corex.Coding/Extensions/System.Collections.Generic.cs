using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
        public static void ForEachJoin<T>(this IEnumerable<T> items, Action<T> action, Action actionBetweenItems)
        {
            var first = true;
            foreach (var item in items)
            {
                if (first)
                    first = false;
                else
                    actionBetweenItems();
                action(item);
            }
        }


        public static void ForEachTwice<T1, T2>(this IEnumerable<T1> items, IEnumerable<T2> items2, Action<T1, T2> action)
        {
            var i1 = items.GetEnumerator();
            var i2 = items2.GetEnumerator();
            var hasItems1 = true;
            var hasItems2 = true;
            while (hasItems1 || hasItems2)
            {
                if (hasItems1)
                    hasItems1 = i1.MoveNext();
                if (hasItems2)
                    hasItems2 = i2.MoveNext();
                action(i1.Current, i2.Current);
            }
        }

        public static bool TrueForAllTwice<T1, T2>(this IEnumerable<T1> items, IEnumerable<T2> items2, Func<T1, T2, bool> func)
        {
            var i1 = items.GetEnumerator();
            var i2 = items2.GetEnumerator();
            var hasItems1 = true;
            var hasItems2 = true;
            while (hasItems1 && hasItems2)
            {
                hasItems1 = i1.MoveNext();
                hasItems2 = i2.MoveNext();
                if (!hasItems1 || !hasItems2)
                    break;
                if (!func(i1.Current, i2.Current))
                    return false;
            }
            return hasItems1 == hasItems2;
        }

        public static T TryGet<T>(this List<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return default(T);
            return list[index];
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

        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            dic.TryGetValue(key, out value);
            return value;
        }

        public static TValue GetCreateValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key) where TValue : class, new()
        {
            var value = dic.TryGetValue(key);
            if (value==null)
            {
                value = new TValue();
                dic[key] = value;
            }
            return value;
        }


    }
}
