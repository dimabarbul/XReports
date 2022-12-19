using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using XReports.Tests.Analyzers.Helpers;

namespace XReports.Tests.Analyzers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestMethodNameAnalyzer : DiagnosticAnalyzer
    {
        private const string PatternConfigKey = "dotnet_diagnostic.CUSTOM2.pattern";
        private const string DefaultPattern = @"^[a-zA-Z]+Should[a-zA-Z0-9]+(When[a-zA-Z0-9]+)?$";

        private readonly DiagnosticDescriptor diagnostic = new DiagnosticDescriptor(
            "CUSTOM2",
            "Test method name",
            "Test method '{0}' should follow pattern '{1}'",
            "Naming",
            DiagnosticSeverity.Error,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(this.diagnostic);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(this.AnalyzeMethod, SymbolKind.Method);
        }

        private void AnalyzeMethod(SymbolAnalysisContext context)
        {
            IMethodSymbol methodSymbol = (IMethodSymbol)context.Symbol;
            if (!TestMethodHelper.IsTestMethod(context, methodSymbol))
            {
                return;
            }

            string pattern = OptionsHelper.GetValue(context, methodSymbol, PatternConfigKey) ?? DefaultPattern;
            if (!new Regex(pattern).IsMatch(methodSymbol.Name))
            {
                context.ReportDiagnostic(Diagnostic.Create(this.diagnostic, methodSymbol.Locations[0],
                    methodSymbol.Name, pattern));
            }
        }
    }
}
