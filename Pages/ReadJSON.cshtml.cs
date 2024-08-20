using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Testing_ASP_.NET.Pages
{
    public class ReadJSONModel : PageModel
    {
        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public List<string> Conventions { get; set; }

        [BindProperty]
        public int? RemoveIndex { get; set; }

        public Dictionary<string, List<string>> NamingConventions { get; set; } = new();

        public void OnGet()
        {
            LoadJsonData();
        }

        public IActionResult OnPost()
        {
            /*            LoadJsonData();

                        if (RemoveIndex.HasValue)
                        {
                            // Handle item removal
                            Conventions.RemoveAt(RemoveIndex.Value);
                        }
                        else if (!string.IsNullOrEmpty(Category) && Conventions != null)
                        {
                            // Update or add the category
                            NamingConventions[Category] = Conventions;
                            SaveJsonData();
                        }

                        return RedirectToPage(); // To refresh the page and reflect changes*/


            LoadJsonData();

            if (!string.IsNullOrEmpty(Category) && Conventions != null)
            {
                if (RemoveIndex.HasValue)
                {
                    // Check if the index is within range before removing
                    if (RemoveIndex.Value >= 0 && RemoveIndex.Value < Conventions.Count)
                    {
                        Conventions.RemoveAt(RemoveIndex.Value);
                    }
                }

                // Update the category with the modified conventions
                NamingConventions[Category] = Conventions;
                SaveJsonData();
            }

            return RedirectToPage(); // Refresh the page to reflect the updated data

        }

        private void LoadJsonData()
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
            }
        }

        private void SaveJsonData()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mydata", "namingConventions.json");

            var jsonObject = new Dictionary<string, Dictionary<string, List<string>>>
            {
                { "namingConventions", NamingConventions }
            };

            var updatedJsonData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            System.IO.File.WriteAllText(jsonFilePath, updatedJsonData);
        }
    }
}
