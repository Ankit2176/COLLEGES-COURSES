using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CollegeAndCourses.Models
{
    public class College
    {
        [Key]
 
        public int CollegeId { get; set; }
       
        public string Name { get; set; }
      
        public string Grade { get; set; }
      

        public string Address { get; set; } // textarea

        [Display(Name = "Date")]

        public DateTime dateTime { get; set; }
        [Display(Name = "Contact Method")]
        public string PreferredContactMethod { get; set; }  //radio button
        [Display(Name = "NAAC")]

        public bool IsNAACAccrediet{ get; set; } // checkbox

        [Display(Name = "News Letter")]

        public bool RecevieNewsLetter{ get; set; }  // switch
        [Display(Name = "Student")]

        public string HowManyStudents { get; set; } // dropdown 

        public int PhoneNo { get; set; }  //int

        public  List<string> Branches { get; set; }     // multidropdown

        [Display(Name="Branchs")]
        public string BranchesString {  get; set; } = string.Empty;
      

    }
}
