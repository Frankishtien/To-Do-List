using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace YourNamespace
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public TaskModel NewTask { get; set; }

        public List<string> Tasks { get; set; } = new List<string>();

        public IActionResult OnGet()
        {
            // Load tasks from session or database if needed
            var tasksJson = HttpContext.Session.GetString("Tasks");
            Tasks = tasksJson != null ? JsonConvert.DeserializeObject<List<string>>(tasksJson) : new List<string>();
            return Page();
        }

        public IActionResult OnPostAddTask()
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(NewTask.Name))
                {
                    Tasks.Add(NewTask.Name.Trim());
                    HttpContext.Session.SetString("Tasks", JsonConvert.SerializeObject(Tasks));
                }
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteTask(string taskToDelete)
        {
            Tasks.Remove(taskToDelete);
            HttpContext.Session.SetString("Tasks", JsonConvert.SerializeObject(Tasks));
            return RedirectToPage();
        }
    }

    public class TaskModel
    {
        [Required(ErrorMessage = "Task description is required.")]
        public string Name { get; set; }
    }
}
