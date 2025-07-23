using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ApartmentWeb.Controllers;

public abstract class ControllerWithPdfRenderingBase : Controller
{
    protected async Task<string> RenderRazorViewToStringAsync(string viewName, object model)
    {
        ViewData.Model = model;

        using var sw = new StringWriter();
        var viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        
        if (viewEngine == null)
        {
            throw new InvalidOperationException("Could not resolve ICompositeViewEngine service");
        }

        var viewResult = viewEngine.FindView(ControllerContext, viewName, false);

        if (!viewResult.Success)
        {
            throw new ArgumentNullException($"Unable to find view '{viewName}'");
        }

        var viewContext = new ViewContext(
            ControllerContext,
            viewResult.View,
            ViewData,
            TempData,
            sw,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }

    // Synchronous version for backward compatibility, but async is preferred
    protected string RenderRazorViewToString(string viewName, object model)
    {
        return RenderRazorViewToStringAsync(viewName, model).GetAwaiter().GetResult();
    }
}