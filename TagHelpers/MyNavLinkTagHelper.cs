using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-controller, asp-action")]
    public class MyNavLinkTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewCtx { get; set; } = null!;

        public string Action { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            string controller = ViewCtx.RouteData.Values["controller"]?.ToString();

            var tagController = context.AllAttributes["asp-controller"]?.Value.ToString();

            if (controller == tagController)
            {
                var classAttribute = output.Attributes["class"];
                if (classAttribute != null )
                {
                    output.Attributes.AppendCssClass(classAttribute.Value + " active");
                }
                else
                {
                    output.Attributes.AppendCssClass("active");
                }
            }

        }
    }
}
