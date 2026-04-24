using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

[assembly: Parallelize(Workers = 4, Scope = ExecutionScope.ClassLevel)]