/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var namingConventionData = JsonConvert.DeserializeObject<NamingConventionModel>(jsonData);

                NamingConventions = namingConventionData?.NamingConventions ?? new Dictionary<string, List<string>>();
            }
            else
            {
                NamingConventions = new Dictionary<string, List<string>>();
                ModelState.AddModelError(string.Empty, "The data file could not be found.");
            }
        }
    }
}
**/


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonData);

                if (jsonObject != null && jsonObject.ContainsKey("namingConventions"))
                {
                    NamingConventions = jsonObject["namingConventions"];
                }
                else
                {
                    NamingConventions = new Dictionary<string, List<string>>();
                    ModelState.AddModelError(string.Empty, "Invalid JSON structure.");
                }
            }
            else
            {
                NamingConventions = new Dictionary<string, List<string>>();
                ModelState.AddModelError(string.Empty, "The data file could not be found.");
            }
        }

        public IActionResult OnPostSaveCategory([FromBody] CategoryUpdateRequest request)
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mydata", "namingConventions.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonData);

                if (jsonObject == null)
                {
                    jsonObject = new Dictionary<string, Dictionary<string, List<string>>>();
                }

                if (!jsonObject.ContainsKey("namingConventions"))
                {
                    jsonObject["namingConventions"] = new Dictionary<string, List<string>>();
                }

                var namingConventions = jsonObject["namingConventions"];

                if (namingConventions.ContainsKey(request.Category))
                {
                    namingConventions[request.Category] = request.Conventions;
                }
                else
                {
                    namingConventions.Add(request.Category, request.Conventions);
                }

                // Save updated JSON
                var updatedJsonData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                System.IO.File.WriteAllText(jsonFilePath, updatedJsonData);

                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false });
        }
    }

    public class CategoryUpdateRequest
    {
        public string Category { get; set; }
        public List<string> Conventions { get; set; }
    }
}
