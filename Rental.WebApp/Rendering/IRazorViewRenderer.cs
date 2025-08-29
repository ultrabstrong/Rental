using System.Threading.Tasks;

namespace Rental.WebApp.Rendering;

public interface IRazorViewRenderer
{
    Task<string> RenderAsync(string viewPathOrName, object model);
}
