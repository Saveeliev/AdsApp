using AdsApp.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Text;

namespace AdsApp.Helpers
{
    public static class PaginationHelper
    {
        public static HtmlString PageLinks(this IHtmlHelper html, PageViewModel pageViewModel, Func<int, string> pageUrl)
        {
            int j;
            int k;

            if (pageViewModel.CurrentPageNumber <= 3)
            {
                j = 1;
            }
            else
            {
                j = pageViewModel.CurrentPageNumber - 3;
            }

            if (pageViewModel.TotalPages < 6)
            {
                k = pageViewModel.TotalPages;
            }
            else
            {
                k = 6;
            }

            StringBuilder result = new StringBuilder();

            for (int i = j; i < j + k; i++)
            {
                if (pageViewModel.CurrentPageNumber == i)
                {
                    result.Append("<a class='currentPage' disabled>" + i + "</a>");
                }
                else
                {
                    result.Append("<a href='" + pageUrl(i) + "'>" + i + "</a>");
                }
            }

            return new HtmlString(result.ToString());
        }
    }
}