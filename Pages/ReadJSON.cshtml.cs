using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Testing_ASP_.NET.Pages
{
    public class ReadJSONModel : PageModel
    {
        public Dictionary<string, List<string>> NamingConventions { get; set; }

        public void OnGet()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mydata", "namingConventions.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                try
                {
                    var jsonData = System.IO.File.ReadAllText(jsonFilePath);

                    // Deserialize JSON data into the appropriate model
                    var namingConventionsWrapper = JsonConvert.DeserializeObject<NamingConventionsWrapper>(jsonData);
                    NamingConventions = namingConventionsWrapper.NamingConventions;
                }
                catch (JsonSerializationException ex)
                {
                    ModelState.AddModelError(string.Empty, "Error deserializing JSON: " + ex.Message);
                    NamingConventions = new Dictionary<string, List<string>>();
                }
            }
            else
            {
                NamingConventions = new Dictionary<string, List<string>>();
                ModelState.AddModelError(string.Empty, "The data file could not be found.");
            }
        }

        public class NamingConventionsWrapper
        {
            public Dictionary<string, List<string>> NamingConventions { get; set; }
        }
    }
}

