using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace CollegeAndCourses.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Name { get; set; }

        public int Fees { get; set; }
        [Display(Name = "Duration")]
        public string DurationOfCourse { get; set; }


        public int CollegeId { get; set; }

        // Navigation property
        [ForeignKey("CollegeId")]
        public virtual College? College { get; set; } = default!;



        [Display(Name = "Publish on ")]

        public DateTime Publish { get; set; }
        [Display(Name = "For")]
        public string CoureseFor { get; set; }  //radio button
        
[Display(Name = "Published")]
        public bool PublicationAccrediet { get; set; } // checkbox

        
 [Display(Name = "Free")]
        public bool FreeCourese { get; set; }  // switch
       
[Display(Name = "Book")]
        public string BooksForCourse { get; set; } // dropdown 
        
   [Display(Name = "Barcode")]
        public int BracodeOfBook { get; set; }  //int
     

        public List<string> AvailablesON { get; set; }     // multidropdown

        [Display(Name="AvailableOn")]
        public string AvailablesONString { get; set; } = string.Empty;

    }
}
