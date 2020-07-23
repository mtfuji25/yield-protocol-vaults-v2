using Analogueweb.Mvc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DataManager.Models
{

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public static class HtmlHelperExtensions
    {
        #region Methods
        private static void MergeClass(ref IDictionary<string, object> attributes, string value)
        {
            if (attributes.ContainsKey("class"))
            {
                attributes["class"] = (attributes["class"] + " " + value).Trim();
            }
            else
            {
                attributes.Add("class", value);
            }
        }

        public static MvcHtmlString SortLink(this HtmlHelper html, string sort, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, object htmlAttributes)
        {
            IDictionary<string, object> attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            RouteValueDictionary dictionary = new RouteValueDictionary(routeValues);
            Regex regex = new Regex("[^a-zA-Z0-9-]");
            string key = regex.Replace(linkText, "-").ToLower();
            if (Equals(key, dictionary["sort"]))
            {
                MergeClass(ref attributes, "active");
                MergeClass(ref attributes, dictionary["order"].ToString());
                dictionary["order"] = dictionary["order"].ToString() == "asc" ? "dsc" : "asc";
                return html.ActionLink(linkText, actionName, controllerName, dictionary, attributes);
            }
            dictionary.Replace("sort", key);
            return html.ActionLink(linkText, actionName, controllerName, dictionary, attributes);
        }

        public static MvcHtmlString ColumnChoser(this HtmlHelper html, List<SelectListItem> list)
        {
            StringBuilder builder = new StringBuilder();
            int i = 1;
            foreach (SelectListItem item in list as List<SelectListItem>)
            {
                builder.Append(html.ColumnCheckbox(item.Text, item.Value, item.Selected, null, i));
                i++;
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString ColumnCheckbox(this HtmlHelper html, string text, string value, bool selected)
        {
            return ColumnCheckbox(html, text, value, selected, null, null);
        }

        public static MvcHtmlString ColumnCheckbox(this HtmlHelper html, string text, string value, bool selected, object htmlAttributes, int? index)
        {
            IDictionary<string, object> attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            TagBuilder input = new TagBuilder("input");
            if (index == null)
            {
                input.MergeAttribute("data-field", value.ToLower());
            }
            else
            {
                input.MergeAttribute("data-index", index.Value.ToString());
            }
            input.MergeAttribute("name", "Columns");
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("value", value);
            if (selected)
            {
                input.MergeAttribute("checked", "checked");
            }
            TagBuilder label = new TagBuilder("label");
            label.InnerHtml = string.Concat(input.ToString(TagRenderMode.SelfClosing), " ", text);
            return MvcHtmlString.Create(label.ToString());
        }
        #endregion
    }

    public static class ObjectExtensions
    {
        #region Methods
        public static RouteValueDictionary ToDictionary(this object obj)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (obj != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                {
                    dictionary.Add(property.Name.Replace('_', '-'), property.GetValue(obj));
                }
            }
            return dictionary;
        }
        #endregion
    }

    public static class SecurityExtensions
    {
        #region Methods
        public static bool IsMasquerading(this IPrincipal principal)
        {
            if (principal == null)
            {
                return false;
            }
            ClaimsPrincipal claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }
            return claimsPrincipal.HasClaim("IsMasquerading", "true");
        }

        public static string GetMasqueradeData(this IPrincipal principal, string data)
        {
            if (principal == null)
            {
                return string.Empty;
            }
            ClaimsPrincipal claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return string.Empty;
            }
            if (!claimsPrincipal.IsMasquerading())
            {
                return string.Empty;
            }
            Claim adminId = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == data);
            if (adminId == null)
            {
                return string.Empty;
            }
            return adminId.Value;
        }

        public static List<string> Roles(this ClaimsIdentity identity)
        {
            return identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
        #endregion
    }
}