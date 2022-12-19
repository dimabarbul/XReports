using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace XReports.Tests.Analyzers.Helpers
{
    internal static class TestMethodHelper
    {
        private static readonly string[] TestMethodAttributes = { "Xunit.FactAttribute", "Xunit.TheoryAttribute", };

        public static bool IsTestMethod(SymbolAnalysisContext context, ISymbol symbol)
        {
            INamedTypeSymbol[] attributes = TestMethodAttributes
                .Select(context.Compilation.GetTypeByMetadataName)
                .ToArray();

            return symbol is IMethodSymbol methodSymbol
                   && methodSymbol.GetAttributes()
                       .Any(a => attributes.Contains(a.AttributeClass, SymbolEqualityComparer.Default));
        }
    }
}
