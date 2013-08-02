corex
=====

*.NET Core Extensions*

A set of power APIs and extensions to the core .NET framework for better productivity.

## Extension Methods

### System Namespace

Easy check for null or empty on Strings, Arrays and Collections, 
provided as an extension method, with no risk of NullReferenceException.

```
bool IsNullOrEmpty(this string s)
bool IsNullOrEmpty<T>(this IEnumerable<T> list)
```
### Example
```
void foo(string s, List<int> list)
{
  if(s.IsNullOrEmpty())
    throw new ArgumentNullException("s");
  if(list.IsNullOrEmpty())
    throw new ArgumentNullException("list");
}
```

### System.Linq

`void ForEach(this IEnumerable<T> list, Action<T> action)`

`void ForEachJoin(this IEnumerable<T> list, Action<T> action, Action actionBetweenItems)`

## Classes

### Corex.Collections.Generic
`CompositionList<T>(Action<T> itemAdding, Action<T> itemRemoving)`

### Corex.IO

Command-line Tool Arguments Parser / Generator

`FsPath`

### Corex.Text

`CodeWriter`

`LineWriter`

`LineWriterProxy`



