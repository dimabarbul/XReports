using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using XReports.Html;
using XReports.Table;

namespace XReports.Demos.XReports;

public class RazorWriter
{
    private readonly ICompositeViewEngine viewEngine;
    private readonly ITempDataProvider tempDataProvider;
    private readonly IServiceProvider serviceProvider;

    public RazorWriter(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.viewEngine = this.serviceProvider.GetRequiredService<ICompositeViewEngine>();
        this.tempDataProvider = this.serviceProvider.GetRequiredService<ITempDataProvider>();
    }

    public Task<string> WriteAsync(IReportTable<HtmlReportCell> report, string viewName)
    {
        return this.RenderSimpleViewToStringAsync(viewName, report);
    }

    private async Task<string> RenderSimpleViewToStringAsync<TModel>(string viewName, TModel model)
    {
        ViewEngineResult viewResult = this.viewEngine.GetView(
            executingFilePath: null,
            viewPath: viewName,
            isMainPage: true);
        if (!viewResult.Success)
        {
            throw new ArgumentException($"Could not find view: {viewName}", nameof(viewName));
        }

        await using StringWriter writer = new();
        ViewDataDictionary<TModel> viewDictionary = new(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model,
        };
        ViewContext viewContext = new(
            new ActionContext(
                new DefaultHttpContext()
                {
                    RequestServices = this.serviceProvider,
                },
                new RouteData(),
                new ActionDescriptor()),
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(new DefaultHttpContext(), this.tempDataProvider),
            writer,
            new HtmlHelperOptions()
        );
        await viewResult.View.RenderAsync(viewContext);

        return writer.ToString();
    }
}
