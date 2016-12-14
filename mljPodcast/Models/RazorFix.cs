using System.Web.Mvc;
using System.Web.WebPages;

namespace MyWebsite.Helpers
{
    public class RazorXmlViewPage<T> : WebViewPage<T>
    {
        public override void ExecutePageHierarchy()
        {
            Response.ContentType = "text/xml";
            Layout = null;
            PushContext(new WebPageContext(), Response.Output); // push a new page context to the stack
            base.ExecutePageHierarchy();
        }

        public override void Execute()
        {

        }
    }

    public class RazorXmlViewPage : RazorXmlViewPage<object>
    {

    }
}