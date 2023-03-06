using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-action,page-model,page-class,page-class-normal,page-class-selected")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public required PagingInfo PageModel { get; set; }

        public required string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object?> PageUrlValues { get; set; } = new Dictionary<string, object?>();

        public string? PageClass { get; set; }
        public string? PageClassNormal { get; set; }
        public string? PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var divBuilder = new TagBuilder("div");
            for (int page = 1; page <= PageModel.TotalPages; page++)
            {
                var aBuilder = new TagBuilder("a");

                PageUrlValues["page"] = page;
                aBuilder.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

                if (PageClass is not null)
                    aBuilder.AddCssClass(PageClass);
                if (PageClassNormal is not null && page != PageModel.CurrentPage)
                    aBuilder.AddCssClass(PageClassNormal);
                else if (PageClassSelected is not null && page == PageModel.CurrentPage)
                    aBuilder.AddCssClass(PageClassSelected);

                aBuilder.InnerHtml.Append(page.ToString());
                divBuilder.InnerHtml.AppendHtml(aBuilder);
            }

            output.Content.AppendHtml(divBuilder.InnerHtml);
        }
    }
}
