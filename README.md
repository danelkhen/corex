corex
=====

A set of power APIs and extensions to the core .NET framework for better productivity.

Extension Methods

System
bool IsNullOrEmpty(this string s)
bool IsNullOrEmpty(this IEnumerable<T> list)

System.Linq
void ForEach(this IEnumerable<T> list, Action<T> action)
void ForEachJoin(this IEnumerable<T> list, Action<T> action, Action actionBetweenItems)

Corex.Collections.Generic
CompositionList<T>

Corex.IO
Command-line Tool Arguments Parser / Generator
FsPath

Corex.Text
CodeWriter, LineWriter, LineWriterProxy



