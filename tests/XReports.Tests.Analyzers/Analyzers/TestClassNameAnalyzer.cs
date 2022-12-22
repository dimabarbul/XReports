using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using XReports.Tests.Analyzers.Helpers;

namespace XReports.Tests.Analyzers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestClassNameAnalyzer : DiagnosticAnalyzer
    {
        private const string SuffixConfigKey = "dotnet_diagnostic.CustomTC1.suffix";
        private const string DefaultSuffix = "Test";

        private readonly DiagnosticDescriptor diagnostic = new DiagnosticDescriptor(
            "CustomTC1",
            "Test class name",
            "Test class '{0}' should end with '{1}', for example, 'MyClass{1}'",
            "Naming",
            DiagnosticSeverity.Error,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(this.diagnostic);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(this.AnalyzeMethod, SymbolKind.NamedType);
        }

        private void AnalyzeMethod(SymbolAnalysisContext context)
        {
            INamedTypeSymbol namedType = (INamedTypeSymbol)context.Symbol;
            if (namedType.TypeKind != TypeKind.Class)
            {
                return;
            }

            bool isTestClass = namedType.GetMembers()
                .OfType<IMethodSymbol>()
                .Any(s => TestMethodHelper.IsTestMethod(context, s));

            if (!isTestClass)
            {
                return;
            }

            string suffix = OptionsHelper.GetValue(context, namedType, SuffixConfigKey) ?? DefaultSuffix;
            if (!namedType.Name.EndsWith(suffix, StringComparison.Ordinal))
            {
                context.ReportDiagnostic(Diagnostic.Create(this.diagnostic, namedType.Locations[0], namedType.Name, suffix));
            }
        }
    }
}
