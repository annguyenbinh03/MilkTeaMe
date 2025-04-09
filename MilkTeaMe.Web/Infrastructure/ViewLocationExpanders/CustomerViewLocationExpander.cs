using Microsoft.AspNetCore.Mvc.Razor;

namespace MilkTeaMe.Web.Infrastructure.ViewLocationExpanders
{
	public class CustomerViewLocationExpander : IViewLocationExpander
	{
		public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
		{
			var customerLocations = new[]
			{
                "/Views/Customer/{1}/{0}.cshtml"
            };
			return customerLocations.Concat(viewLocations);
		}

		public void PopulateValues(ViewLocationExpanderContext context) {}
	}
}
