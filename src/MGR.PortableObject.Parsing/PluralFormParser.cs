using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace MGR.PortableObject.Parsing
{
    internal class PluralFormParser
    {
        public IPluralForm Parse(string pluralForm)
        {
            var method = GetCompiledMethod(pluralForm);

            return new FuncBasedPluralForm(method);
        }

        private Func<int, int> GetCompiledMethod(string pluralForm)
        {
            var code = $@"namespace Plural {{
    public static class Form {{
        public static object Compute(int n){{
            return {pluralForm};
        }}
    }}
}}
";
            var c = CreateCompilation(code);
            var assembly = CompileAndLoadAssembly(c);
            var pluralFormType = assembly.GetType("Plural.Form");
            var computeMethod = pluralFormType.GetMethod("Compute", BindingFlags.Static|BindingFlags.Public) ?? throw new InvalidOperationException("Unable to find the generated compute method.");
            return n =>
            {
                var computationResult = computeMethod.Invoke(null, new object[] {n}) ?? throw new InvalidOperationException("The computation should return a value.");
                if (computationResult is bool result)
                {
                    return result ? 1 : 0;
                }

                return (int) computationResult;
            };
        }
        private CSharpCompilation CreateCompilation(string pluralForm)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(pluralForm);
            string assemblyName = Guid.NewGuid().ToString();
            var references = GetAssemblyReferences();
            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            return compilation;
        }
        private static IEnumerable<MetadataReference> GetAssemblyReferences()
        {
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location)
            };
            return references;
        }

        private Assembly CompileAndLoadAssembly(CSharpCompilation compilation)
        {
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            ThrowExceptionIfCompilationFailure(result);
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            return assembly;
        }

        private void ThrowExceptionIfCompilationFailure(EmitResult result)
        {
            if (!result.Success)
            {
                var compilationErrors = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)
                    .ToList();
                if (compilationErrors.Any())
                {
                    var firstError = compilationErrors.First();
                    var errorNumber = firstError.Id;
                    var errorDescription = firstError.GetMessage();
                    var firstErrorMessage = $"{errorNumber}: {errorDescription};";
                    throw new Exception($"Compilation failed, first error is: {firstErrorMessage}");
                }
            }
        }
    }
}
