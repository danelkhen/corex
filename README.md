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

```
void ForEach(this IEnumerable<T> list, Action<T> action)
void ForEachJoin(this IEnumerable<T> list, Action<T> action, Action actionBetweenItems)
```
Provides iterating over any enumerable, performing action on each item, and between items, 
this is essentially a generaltization of String.Join.

## Classes

### Corex.Collections.Generic
`CompositionList<T>(Action<T> itemAdding, Action<T> itemRemoving)`

Provides interception notifications for the list's owner, to handle parent assignments of list items.

### Corex.IO
```
ToolArgs<T>
    Tokenizer
    Parser
    HelpGenerator
    Serializer
```

Command-line Tool Arguments Tokenizer, Parser, Generator and Serializer.

`FsPath`

### Corex.Text

```
LineWriter
LineWriterProxy
CodeWriter
```

LineWriter is a line based stream writer, optimized for per-line flush basis.
LineWriterProxy allows easy encpasulation for any extended LineWriter.
CodeWriter provides special addon methods for generating source code.


