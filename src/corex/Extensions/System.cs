using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System
{
    public static class Extensions
    {
        public static string[] Split(this string s, string separator)
        {
            return s.Split(new []{separator}, StringSplitOptions.None);
        }

        public static Uri CombineWith(this Uri uri, string relativeUrl)
        {
            return new Uri(uri, relativeUrl);
        }
        public static UriBuilder ToUriBuilder(this Uri uri)
        {
            return new UriBuilder(uri);
        }
        public static Uri SetPath(this Uri uri, string path)
        {
            var x = uri.ToUriBuilder();
            x.Path = path;
            return x.Uri;
        }
        public static bool StartsAndEndsWith(this string s, string edge)
        {
            return s.StartsWith(edge) && s.EndsWith(edge);
        }
        public static bool StartsWithAnyChar(this string s, string chars)
        {
            if (s.IsEmpty())
                return false;
            foreach (var ch in chars)
                if (s[0] == ch)
                    return true;
            return false;
        }
        public static int IndexOfAnyChar(this string s, string chars)
        {
            return s.IndexOfAny(chars.ToArray());
        }
        public static bool ContainsIgnoreCase(this IEnumerable<string> list, string s)
        {
            return list.Any(t => t.EqualsIgnoreCase(s));
        }
        public static bool ContainsAnyChar(this string s, string chars)
        {
            if (s.IsEmpty())
                return false;
            foreach (var ch in chars)
                if (s.Contains(ch))
                    return true;
            return false;
        }
        public static string Quote(this string s)
        {
            return s.Wrap("\"");
        }
        public static string Wrap(this string s, string prefixSuffix)
        {
            return s.Wrap(prefixSuffix, prefixSuffix);
        }
        public static string Wrap(this string s, string prefix, string suffix)
        {
            return new StringBuilder(prefix).Append(s).Append(suffix).ToString();
        }
        public static R IfNotNull<T, R>(this T obj, Func<T, R> func) where T : class
        {
            if (obj != null)
                return func(obj);
            return default(R);
        }


        //
        // Summary:
        //     Replaces one or more format items in a specified string with the string representation
        //     of a specified object.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The object to format.
        //
        // Returns:
        //     A copy of format in which any format items are replaced by the string representation
        //     of arg0.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format is null.
        //
        //   System.FormatException:
        //     The format item in format is invalid.-or- The index of a format item is not
        //     zero.
        public static string FormatWith(this string format, object arg0)
        {
            return String.Format(format, arg0);
        }
        //
        // Summary:
        //     Replaces the format item in a specified string with the string representation
        //     of a corresponding object in a specified array.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string
        //     representation of the corresponding objects in args.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format or args is null.
        //
        //   System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than or equal to the length of the args array.
        public static string FormatWith(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        //
        // Summary:
        //     Replaces the format item in a specified string with the string representation
        //     of a corresponding object in a specified array. A specified parameter supplies
        //     culture-specific formatting information.
        //
        // Parameters:
        //   provider:
        //     An object that supplies culture-specific formatting information.
        //
        //   format:
        //     A composite format string (see Remarks).
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string
        //     representation of the corresponding objects in args.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format or args is null.
        //
        //   System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than or equal to the length of the args array.
        public static string FormatWith(this IFormatProvider provider, string format, params object[] args)
        {
            return String.Format(format, provider, args);
        }

        //
        // Summary:
        //     Replaces the format items in a specified string with the string representation
        //     of two specified objects.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The first object to format.
        //
        //   arg1:
        //     The second object to format.
        //
        // Returns:
        //     A copy of format in which format items are replaced by the string representations
        //     of arg0 and arg1.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format is null.
        //
        //   System.FormatException:
        //     format is invalid.-or- The index of a format item is not zero or one.
        public static string FormatWith(this string format, object arg0, object arg1)
        {
            return String.Format(format, arg0, arg1);
        }

        //
        // Summary:
        //     Replaces the format items in a specified string with the string representation
        //     of three specified objects.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The first object to format.
        //
        //   arg1:
        //     The second object to format.
        //
        //   arg2:
        //     The third object to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string
        //     representations of arg0, arg1, and arg2.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format is null.
        //
        //   System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than two.
        public static string FormatWith(this string format, object arg0, object arg1, object arg2)
        {
            return String.Format(format, arg0, arg1, arg2);
        }


        public static T IfTrue<T>(this bool x, T value)
        {
            if (x)
                return value;
            return default(T);
        }
        public static T IfTrue<T>(this bool x, T value, T elseValue)
        {
            if (x)
                return value;
            return elseValue;
        }
        public static T IfFalse<T>(this bool x, T value)
        {
            if (!x)
                return value;
            return default(T);
        }

        public static string ToStringOrEmpty<T>(this T? value) where T : struct
        {
            if (!value.HasValue)
                return String.Empty;
            return value.ToString();
        }
        public static string ToStringOrEmpty<T>(this T value) where T : class
        {
            if (value == null)
                return String.Empty;
            return value.ToString();
        }
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return s == null || s.Length == 0;
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return s != null && s.Length > 0;
        }

        public static string ReplaceFirst(this string s, string search, string replace)
        {
            return ReplaceFirst(s, search, replace, StringComparison.CurrentCulture);
        }

        public static string ReplaceFirst(this string s, string search, string replace, StringComparison comparisonType)
        {
            int index = s.IndexOf(search, comparisonType);
            if (index != -1)
            {
                string final = String.Concat(s.Substring(0, index), replace, s.Substring(search.Length + index));
                return final;
            }
            return s;
        }

        public static string RemoveLast(this string s, int count)
        {
            return s.Substring(0, s.Length - count);
        }

        public static string TrimEnd(this string s, string trimText)
        {
            if (s.EndsWith(trimText))
                return RemoveLast(s, trimText.Length);
            return s;
        }

        public static bool EqualsIgnoreCase(this string s1, string s2)
        {
            return String.Compare(s1, s2, true) == 0;
        }

        public static List<string> SplitAt(this string text, int index)
        {
            return SplitAt(text, index, false);
        }

        public static List<string> SplitAt(this string text, int index, bool removeIndexChar)
        {
            string s1 = text.Substring(0, index);
            if (removeIndexChar)
                index++;
            string s2 = text.Substring(index);
            return new List<string> { s1, s2 };
        }


        public static string SubstringBetween(this string s, int fromIndex, int toIndex)
        {
            return s.Substring(fromIndex, toIndex - fromIndex);
        }
        public static string ReplaceBetween(this string s, string from, string to, string value)
        {
            int index1, index2;
            if (s.IndexesBetween(from, to, out index1, out index2))
            {
                var s2 = s.Remove(index1, index2 - index1);
                s2 = s2.Insert(index1, value);
                return s2;
            }
            return s;
        }
        public static string SubstringBetween(this string s, string from, string to)
        {
            int index1, index2;
            if (s.IndexesBetween(from, to, out index1, out index2))
                return s.SubstringBetween(index1, index2);
            return null;
        }
        public static bool IndexesBetween(this string s, string from, string to, out int index, out int index2)
        {
            index = s.IndexOf(from);
            if (index >= 0)
            {
                index += from.Length;
                index2 = s.IndexOf(to, index);
                if (index2 >= 0)
                    return true;
            }
            index2 = -1;
            return false;
        }
        public static IEnumerable<int> AllIndexesOf(this string s, string find, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            var index = 0;
            var list = new List<int>();
            while (index >= 0 && index < s.Length)
            {
                index = s.IndexOf(find, index, comparisonType);
                if (index >= 0)
                {
                    yield return index;
                    index++;
                }
            }
        }



        public static bool IsInRange(this int x, int from, int to)
        {
            return x >= from && x <= to;
        }
        public static R If<R>(this bool condition, R ifTrue)
        {
            if (condition)
                return ifTrue;
            return default(R);
        }
        public static R If<R>(this bool condition, R ifTrue, R ifNotTrue)
        {
            if (condition)
                return ifTrue;
            else
                return ifNotTrue;
        }

        public static T NotNull<T>(this T obj) where T : class, new()
        {
            if (obj == null)
                return new T();
            return obj;
        }
        public static T[] NotNull<T>(this T[] array)
        {
            if (array == null)
                return new T[0];
            return array;
        }



        public static TypeCode GetTypeCode(this Type type)
        {
            return Type.GetTypeCode(type);
        }

        public static IEnumerable<string> Lines(this string s)
        {
            using (var reader = new StringReader(s))
            {
                while (true)
                {
                    var ss = reader.ReadLine();
                    if (ss == null)
                        break;
                    yield return ss;
                }
            }
        }

    }
}
