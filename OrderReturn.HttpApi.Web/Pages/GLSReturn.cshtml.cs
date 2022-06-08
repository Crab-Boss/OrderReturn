using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrderReturn.HttpApi.Web.Pages
{
    public class GLSReturnModel : PageModel
    {
        public void OnGet(string orderNumber)
        {
            ViewData["OrderNumber"] = orderNumber;
        }
    }
}
