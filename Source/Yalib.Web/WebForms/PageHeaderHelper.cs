using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Yalib.Web.WebForms
{
	/// <summary>
	/// Notice: You must call these methods after 
	/// </summary>
	public static class PageHeaderHelper
	{
		private static string GetHeaderChildID(Page page) 
		{
			return String.Format("{0}_Head_{1}", page.GetType().Name, page.Header.Controls.Count + 1);
		}

		public static void IncludeString(Page page, string str)
		{
			var child = new LiteralControl();
			child.ID = GetHeaderChildID(page);
			child.Text = str;
			page.Header.Controls.Add(child);
		}

		public static void IncludeStringAtTop(Page page, string str)
		{
			var child = new LiteralControl();
			child.ID = GetHeaderChildID(page);
			child.Text = str;
			page.Header.Controls.AddAt(0, child);
		}


		public static void IncludeCssFile(Page page, string cssFileName)
		{
			HtmlGenericControl child = new HtmlGenericControl("link");
			child.ID = GetHeaderChildID(page);
			child.Attributes.Add("rel", "stylesheet");
			child.Attributes.Add("href", cssFileName);
			child.Attributes.Add("type", "text/css");
			page.Header.Controls.Add(child);
		}

		public static void IncludeScriptFile(Page page, string jsFileName)
		{
			HtmlGenericControl child = new HtmlGenericControl("script");
			child.ID = GetHeaderChildID(page);
			child.Attributes.Add("type", "text/javascript");
			child.Attributes.Add("src", jsFileName);
			page.Header.Controls.Add(child);
		}

		public static void IncludeScriptCode(Page page, string script)
		{
			HtmlGenericControl child = new HtmlGenericControl("script");
			child.ID = GetHeaderChildID(page);
			child.Attributes.Add("type", "text/javascript");
			child.InnerHtml = script;
			page.Header.Controls.Add(child);
		}
	}
}
