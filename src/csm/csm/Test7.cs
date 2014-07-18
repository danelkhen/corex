Console.WriteLine("Before");
@"
Console.WriteLine(""asdasdsa"");
".ExecuteAsCsmScript();
@"
csm \\e:Console.WriteLine(777);
".ExecuteAsBatchScript();
Console.WriteLine("After");
