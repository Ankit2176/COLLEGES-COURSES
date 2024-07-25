namespace CollegeAndCourses.Models.ViewModels
{
    public class CollegeIndexViewModel
    {
        public List<College> Colleges { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

}
