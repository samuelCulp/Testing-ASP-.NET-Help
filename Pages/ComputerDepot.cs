using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Testing_ASP_.NET.Pages
{
    public class NamingConventionModel
    {
        public Dictionary<string, List<string>> ConventionCategories { get; set; }
    }


/*    public class CategoryUpdateRequest
    {
        public string Category { get; set; } // Ensure there's only one 'Category' property
        public List<string> Conventions { get; set; }
    }*/

}
