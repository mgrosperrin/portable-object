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
        private const char PartsSeparator = ';';
        private const char NbPluralPartSeparator = '=';
        private const string PluralFuncPrefix = "plural=";

        private const string PluralFormsNamespace = "Plural";
        private const string PluralFormsClass = "Form";
        private const string PluralFormsMethod = "Compute";

        public IPluralForm Parse(string pluralFormsHeader)
        {
            var pluralFormsParts = pluralFormsHeader.Split(PartsSeparator);
            var nbPluralsParts = pluralFormsParts[0].Split(NbPluralPartSeparator);
            var pluralNumber = int.Parse(nbPluralsParts[1]);
            var pluralFormFunc = pluralFormsParts[1].Replace(PluralFuncPrefix, string.Empty);
            var method = GetCompiledMethod(pluralFormFunc);

            return new FuncBasedPluralForm(pluralNumber, method);
        }

        private Func<int, int> GetCompiledMethod(string pluralForm)
        {
            var code = $@"namespace {PluralFormsNamespace}
{{
    public static class {PluralFormsClass}
    {{
        public static object {PluralFormsMethod}(int n)
        {{
            return {pluralForm};
        }}
    }}
}}
";
            var c = CreateCompilation(code);
            var assembly = CompileAndLoadAssembly(c);
            var pluralFormType = assembly.GetType($"{PluralFormsNamespace}.{PluralFormsClass}");
            var computeMethod = pluralFormType.GetMethod(PluralFormsMethod, BindingFlags.Static | BindingFlags.Public) ?? throw new InvalidOperationException("Unable to find the generated compute method.");
            return n =>
            {
                var computationResult = computeMethod.Invoke(null, new object[] { n })
                        ?? throw new InvalidOperationException("The computation should return a value.");
                if (computationResult is bool result)
                {
                    return result ? 1 : 0;
                }

                return (int)computationResult;
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
                    throw new InvalidOperationException($"Compilation failed, first error is: {firstErrorMessage}");
                }
            }
        }
    }
}
