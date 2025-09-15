
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SportsPro.TagHelpers
{
	[HtmlTargetElement("show-message")]
	public class ShowMessageTagHelper : TagHelper
	{
		[ViewContext]
		public ViewContext ViewCtx { get; set; }

		protected ITempDataDictionary TempData => ViewCtx.TempData;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (TempData.ContainsKey("message"))
			{
				output.TagName = "h4";
				output.Attributes.SetAttribute("class", "bg-success p-2 text-white text-center");

				output.Content.SetContent(TempData["message"].ToString());

				TempData.Remove("message");
			}
			else
			{
				output.SuppressOutput();
			}
		}
	}
}
