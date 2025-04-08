using Microsoft.AspNetCore.Mvc.Razor;

namespace MilkTeaMe.Web.Infrastructure.ViewLocationExpanders
{
	public class ManagerViewLocationExpander : IViewLocationExpander
	{
		public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
		{
			var customerLocations = new[]
			{
				"/Views/Manager/{1}/{0}.cshtml"
			};
			return customerLocations.Concat(viewLocations);
		}

		public void PopulateValues(ViewLocationExpanderContext context) {}
	}
}
