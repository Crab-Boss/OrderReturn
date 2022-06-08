using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrderReturn.HttpApi.Web.Pages
{
    public class DHLReturnModel : PageModel
    {
        public void OnGet(string receiverId,string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(receiverId))
            {
                receiverId = "DE_RETOURE_ONLINE_MYTOY";
            }
            ViewData["ReceiverId"] = receiverId;
            ViewData["OrderNumber"] = orderNumber;
        }
    }
}
