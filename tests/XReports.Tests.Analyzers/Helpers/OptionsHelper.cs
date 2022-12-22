using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace XReports.Tests.Analyzers.Helpers
{
    public static class OptionsHelper
    {
        public static string GetValue(SymbolAnalysisContext context, ISymbol symbol, string key)
        {
            SyntaxTree syntaxTree = symbol.Locations[0].SourceTree;
            if (syntaxTree == null)
            {
                return null;
            }

            AnalyzerConfigOptions options = context.Options.AnalyzerConfigOptionsProvider.GetOptions(syntaxTree);

            return options.TryGetValue(key, out string value) ? value : null;
        }
    }
}
