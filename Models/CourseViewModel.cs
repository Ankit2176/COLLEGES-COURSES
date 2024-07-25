using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class CourseViewModel
    {
        public int CollegeId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string Name { get; set; }

        // Add other properties as needed
    }
}
