using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Testing_ASP_.NET.Pages
{



    public class ReadJSONContactModel : PageModel
    {
        public Dictionary<string, List<ContactInfo>> data { get; set; } // Update the type

        public void OnGet()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mydata", "contactInfo.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<ContactInfo>>>>(jsonData);

                if (jsonObject != null && jsonObject.ContainsKey("data"))
                {
                    data = jsonObject["data"];
                }
                else
                {
                    data = new Dictionary<string, List<ContactInfo>>();
                    ModelState.AddModelError(string.Empty, "Invalid JSON structure");
                }
            }
            else
            {
                data = new Dictionary<string, List<ContactInfo>>();
                ModelState.AddModelError(string.Empty, "The data file could not be found");
            }
        }
/*        public IActionResult OnPostSaveCategory([FromBody] CategoryUpdateRequestContact request)
        {
            if (request == null || string.IsNullOrEmpty(request.CategoryContact))
            {
                Console.WriteLine("Invalid request data: request is null or CategoryContact is empty");
                return BadRequest(new { success = false, message = "Invalid request data" });
            }

            Console.WriteLine($"Received category: {request.CategoryContact}");
            Console.WriteLine($"Received conventions: {string.Join(", ", request.ConventionsContact)}");

            // Additional logic...
            return new JsonResult(new { success = true });
        }*/


        public IActionResult OnPostSaveCategory([FromBody] CategoryUpdateRequestContact request)
        {
            if (request == null || string.IsNullOrEmpty(request.CategoryContact))
            {
                Console.WriteLine("Invalid request data: request is null or CategoryContact is empty");
                return BadRequest(new { success = false, message = "Invalid request data" });
            }

            Console.WriteLine($"Received category: {request.CategoryContact}");
            Console.WriteLine($"Received conventions: {string.Join(", ", request.ConventionsContact)}");

            // Additional logic...
  
            Console.WriteLine($"Received category: {request.CategoryContact}");
            Console.WriteLine($"Received conventions: {string.Join(", ", request.ConventionsContact)}");

            // Path to the JSON file
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mydata", "contactInfo.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonData);

                if (jsonObject == null)
                {
                    jsonObject = new Dictionary<string, Dictionary<string, List<string>>>();
                }

                if (!jsonObject.ContainsKey("data"))
                {
                    jsonObject["data"] = new Dictionary<string, List<string>>();
                }

                var data = jsonObject["data"];

                if (data.ContainsKey(request.CategoryContact))
                {
                    data[request.CategoryContact] = request.ConventionsContact;
                }
                else
                {
                    data.Add(request.CategoryContact, request.ConventionsContact);
                }

                // Save updated JSON file
                var updatedJsonData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                System.IO.File.WriteAllText(jsonFilePath, updatedJsonData);

                return new JsonResult(new { success = true });
            }

            return BadRequest(new { success = false, message = "File not found" });

        }


        public class CategoryUpdateRequestContact
        {
            public string CategoryContact { get; set; }
            public List<string> ConventionsContact { get; set; }
        }
        public class ContactInfo
        {
            public string Contact { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Group { get; set; }
        }



    }
}
