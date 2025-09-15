using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Collections.Generic;
using SportsPro.Models;

namespace SportsPro.TagHelpers
{
    [HtmlTargetElement("custom-select", Attributes = "asp-for")]
    public class CustomDropDownTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewCtx { get; set; }

        // This will store the ModelExpression for the asp-for attribute
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (AspFor == null)
            {
                return;
            }

            var propertyName = AspFor.Name;

            output.TagName = "select";

            output.Attributes.SetAttribute("name", propertyName);
            output.Attributes.SetAttribute("id", propertyName);
            output.Attributes.SetAttribute("class", "form-select");

            var selectListItems = GetSelectListItems(propertyName);
            if (selectListItems != null)
            {
                foreach (var item in selectListItems)
                {
                    var optionTag = new TagBuilder("option");
                    optionTag.Attributes.Add("value", item.Value);
                    optionTag.InnerHtml.Append(item.Text);
                    output.Content.AppendHtml(optionTag);
                }
            }
        }

        private IEnumerable<SelectListItem> GetSelectListItems(string propertyName)
        {
            if (propertyName.Contains("CustomerID"))
            {
                var customers = ViewCtx.ViewData["Customers"] as IEnumerable<Customer>;
                if (customers != null)
                {
                    return customers.Select(c => new SelectListItem
                    {
                        Value = c.CustomerID.ToString(),
                        Text = c.LastName
                    }).ToList();
                }
            }
            else if (propertyName.Contains("ProductID"))
            {
                var products = ViewCtx.ViewData["Products"] as IEnumerable<Product>;
                if (products != null)
                {
                    return products.Select(p => new SelectListItem
                    {
                        Value = p.ProductID.ToString(),
                        Text = p.Name
                    }).ToList();
                }
            }
            else if (propertyName.Contains("TechnicianID"))
            {
                var technicians = ViewCtx.ViewData["Technicians"] as IEnumerable<Technician>;
                if (technicians != null)
                {
                    return technicians.Select(t => new SelectListItem
                    {
                        Value = t.TechnicianID.ToString(),
                        Text = t.Name
                    }).ToList();
                }
            }
            else if (propertyName.Contains("CountryID"))
            {
                var countries = ViewCtx.ViewData["Countries"] as IEnumerable<Country>;
                if (countries != null)
                {
                    return countries.Select(c => new SelectListItem
                    {
                        Value = c.CountryID.ToString(),
                        Text = c.Name
                    }).ToList();
                }
            }

            return null;
        }
    }
}