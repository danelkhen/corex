using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace System.Linq
{
    public static class Extensions
    {
        public static string StringConcat(this IEnumerable<string> list)
        {
            return String.Concat(list);
        }
        public static string StringConcat(this IEnumerable<char> list)
        {
            return String.Concat(list);
        }
        [DebuggerStepThrough]
        public static string StringJoin(this IEnumerable<string> list, string delim)
        {
            var sb = new StringBuilder();
            list.ForEachJoin(t => sb.Append(t), ()=>sb.Append(delim));
            return sb.ToString();
        }
        public static int IndexOf<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            var i = 0;
            foreach (var item in list)
            {
                if (func(item))
                    return i;
                i++;
            }
            return -1;
        }
        /// <summary>
        /// Returns true if the collection is empty
        /// </summary>
        /// <typeparam name="T">Collection item type</typeparam>
        /// <param name="collection">a collection of items</param>
        /// <returns>true if the collection contains no elements</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            foreach (var item in collection)
                return false;
            return true;
        }
        /// <summary>
        /// Removes all items in Target that are not present in source, and Adds all items in Source that are not present in target
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void SetItems<T>(this IList<T> target, IEnumerable<T> source)
        {
            var targetHash = new HashSet<T>(target);
            var sourceHash = new HashSet<T>(source);
            var toDelete = targetHash.Except(sourceHash).ToList();
            var toAdd = sourceHash.Except(targetHash).ToList();
            foreach (var d in toDelete)
                target.Remove(d);
            foreach (var a in toAdd)
                target.Add(a);
        }
        public static void AddRange<T>(this ICollection<T> set, IEnumerable<T> list)
        {
            foreach (T item in list)
                set.Add(item);
        }
        public static Dictionary<K, List<T>> ToDictionary<K, T>(this IEnumerable<IGrouping<K, T>> source)
        {
            var dic = new Dictionary<K, List<T>>();
            foreach (var group in source)
            {
                var list = new List<T>(group);
                dic[group.Key] = list;
            }
            return dic;
        }
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
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
        public static void ForEachJoin<T>(this IEnumerable<T> items, Action<T> action, Action<T> first, Action<T, T> actionBetweenItems, Action<T> last)
        {
            var firstItem = true;
            T prev = default(T);

            foreach (var next in items)
            {
                if (firstItem)
                {
                    firstItem = false;
                    if (first != null) first(next);
                }
                else
                {
                    actionBetweenItems(prev, next);
                }
                action(next);
                prev = next;
            }
            if (last != null) last(prev);
        }
        
        #region Linq

        class DelegatedEqualityComparer<T> : IEqualityComparer<T>
        {
            public DelegatedEqualityComparer(Func<T, T, bool> compare)
            {
                CompareFunc = compare;
            }
            Func<T, T, bool> CompareFunc;

            #region IEqualityComparer<T> Members

            public bool Equals(T x, T y)
            {
                return CompareFunc(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }
        class PropertyEqualityComparer<T, V> : IEqualityComparer<T>
        {
            public PropertyEqualityComparer(Func<T, V> pe)
            {
                Property = pe;
                Comparer = EqualityComparer<V>.Default;
            }
            Func<T, V> Property;
            private EqualityComparer<V> Comparer;

            #region IEqualityComparer<T> Members

            public bool Equals(T x, T y)
            {
                return Comparer.Equals(Property(x), Property(y));
            }

            public int GetHashCode(T obj)
            {
                return Property(obj).GetHashCode();
            }

            #endregion
        }
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> first, Func<T, V> property)
        {
            return first.Distinct(new PropertyEqualityComparer<T, V>(property));
        }
        public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> compare)
        {
            return first.SequenceEqual(second, new DelegatedEqualityComparer<T>(compare));
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
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            var i = 0;
            foreach (var item in items)
            {
                action(item, i);
                i++;
            }
        }

        #endregion



    }
}
