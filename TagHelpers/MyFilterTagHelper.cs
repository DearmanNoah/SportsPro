using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using SportsPro.Models;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-controller, asp-action, asp-route-id")]
    public class MyFilterTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewCtx { get; set; } = null!;

        public string Action { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string SelectedIncidentStatus { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            var id = context.AllAttributes["asp-route-id"]?.Value.ToString();
            var model = ViewCtx.ViewData.Model as IncidentsViewModel;
            var selectedIncidentStatus = model?.SelectedIncidentStatus;

            if (id == selectedIncidentStatus)
            {
                var classAttribute = output.Attributes["class"];
                if (classAttribute != null)
                {
                    output.Attributes.AppendCssClass(classAttribute.Value + "active btn-primary");
                }
                else
                {
                    output.Attributes.AppendCssClass("active btn-primary");
                }
            }
            else
            {
                output.Attributes.AppendCssClass("btn-white");
            }

        }
    }
}
