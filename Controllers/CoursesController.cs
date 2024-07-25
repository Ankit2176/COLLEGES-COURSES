using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeAndCourses.Data;
using CollegeAndCourses.Models;
using Newtonsoft.Json;

namespace CollegeAndCourses.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Courses.Include(c => c.College);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> CoursesIndex()
        {
            var applicationDbContext = _context.Courses.Include(c => c.College);
            return PartialView(await applicationDbContext.ToListAsync());
        }


        //public async Task<IActionResult> CoursesIndex(string sortOrder)
        //{
        //    var coursesQuery = _context.Courses.AsQueryable();

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            coursesQuery = coursesQuery.OrderByDescending(c => c.Name);
        //            break;
        //        default:
        //            coursesQuery = coursesQuery.OrderBy(c => c.Name); 
        //            break;
        //    }

        //    var courses = await coursesQuery.ToListAsync();
        //    return PartialView(courses);
        //}

        //public async Task<IActionResult> CoursesIndex(int collegeId, string sortOrder)
        //{
        //    var coursesQuery = _context.Courses.Where(c => c.CollegeId == collegeId).AsQueryable();

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            coursesQuery = coursesQuery.OrderByDescending(c => c.Name);
        //            break;
        //        default:
        //            coursesQuery = coursesQuery.OrderBy(c => c.Name);
        //            break;
        //    }

        //    var courses = await coursesQuery.ToListAsync();
        //    return PartialView("_CoursesIndexPartial", courses);
        //}




        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.College)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

              return PartialView("Details", course);
        }

        // GET: Courses/Create
        public IActionResult Create(int collegeId)
        {
            // ViewData["CollegeId"] = new SelectList(_context.Colleges, "CollegeId", "CollegeId");

            var colleges = _context.Colleges.ToList(); // Ensure this returns a list of College objects

            // Create the SelectList with the selected value set to the collegeId
            ViewData["CollegeId"] = new SelectList(colleges, "CollegeId", "Name", collegeId);


            var currentDate = DateTime.Now;

            // Pass the current date to the view using ViewData
            ViewData["CurrentDate"] = currentDate;

            return PartialView();
        }




        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CourseId,Name,Fees,DurationOfCourse,CollegeId,Publish,CoureseFor,PublicationAccrediet,FreeCourese,BooksForCourse,BracodeOfBook,AvailablesON")] Course course)
        //{
        //    if (ModelState.IsValid)
        //        {
        //        if (course.AvailablesON != null)
        //        {
        //            // Convert the list of branches to a semicolon-separated string
        //            course.AvailablesONString = string.Join(", ", course.AvailablesON);
        //        }
        //        _context.Add(course);

        //         await _context.SaveChangesAsync();



        //        //return RedirectToAction(nameof(Index),"College",new {id = course.CollegeId});
        //        return RedirectToAction("ViewDetails", "Colleges", new { id = course.CollegeId });




        //    }
        //    ViewData["CollegeId"] = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Name,Fees,DurationOfCourse,CollegeId,Publish,CoureseFor,PublicationAccrediet,FreeCourese,BooksForCourse,BracodeOfBook,AvailablesON")] Course course)
        {
            if (ModelState.IsValid)
            {



                if (course.AvailablesON != null)
                {
                    // Convert the list of branches to a semicolon-separated string
                    course.AvailablesONString = string.Join(", ", course.AvailablesON);
                }




                if (course.CollegeId == 0)
                  {
                    string courseJson = JsonConvert.SerializeObject(course);
                    TempData["NewCourse"] = courseJson;
                    return RedirectToAction("Create", "Colleges");
                }
                else
                {

                    _context.Add(course);
                    await _context.SaveChangesAsync();

                    // Determine the current URL
                    var currentUrl = Request.Headers["Referer"].ToString().ToLower();

                    if (currentUrl.Contains("/colleges/edit/"))
                    {
                        return RedirectToAction("Edit", "Colleges", new { id = course.CollegeId });
                    }
                    else if (currentUrl.Contains("/colleges/viewdetails/"))
                    {
                        return RedirectToAction("ViewDetails", "Colleges", new { id = course.CollegeId });
                    }
                    else if (currentUrl.Contains("/colleges/create"))
                    {
                        return RedirectToAction("Create", "Colleges");
                    }

                    else
                    {
                        // Default redirection if the URL does not match any specific pattern
                        return RedirectToAction("Index", "Colleges", new { id = course.CollegeId });
                    }
                }
            }
            ViewData["CollegeId"] = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
            return View(course);
        }


        // GET: Courses/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    var course = _context.Courses.Find(id);
        //    if (course == null)
        //    {
        //        return NotFound("Error");

        //    }

        //    ViewBag.CollegeId = new SelectList(_context.Colleges, "CollegeId", "CollegeName", course.CollegeId);
        //    return PartialView("Edit", course); // Use a partial view for the modal content
        //}

        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound("Error");
            }

            // Ensure CollegeId is not null before creating the SelectList
            if (course.CollegeId != null)
            {
                ViewBag.CollegeId = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
            }
            else
            {
                ViewBag.CollegeId = new SelectList(_context.Colleges, "CollegeId", "CollegeId");
            }

            return PartialView("Edit", course); // Use a partial view for the modal content
        }

        public IActionResult EditOnMain(int id)
                                                        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound("Error");
            }

            // Ensure CollegeId is not null before creating the SelectList
            if (course.CollegeId != null)
            {
                ViewBag.CollegeId = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
            }
            else
            {
                ViewBag.CollegeId = new SelectList(_context.Colleges, "CollegeId", "CollegeId");
            }

            return PartialView("EditOnMain", course); // Use a partial view for the modal content
        }


        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var course = await _context.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CollegeId"] = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
        //    return View(course);
        //}


        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Name,Fees,DurationOfCourse,CollegeId,Publish,CoureseFor,PublicationAccrediet,FreeCourese,BooksForCourse,BracodeOfBook,AvailablesON")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = "Course details updated successfully.";
                    ViewBag.CourseName = course.Name;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var currentUrl = Request.Headers["Referer"].ToString().ToLower();

                if (currentUrl.Contains("/colleges/edit/"))
                {
                    return RedirectToAction("Edit", "Colleges", new { id = course.CollegeId });
                }
                else if (currentUrl.Contains("/colleges/viewdetails/"))
                {
                    return RedirectToAction("ViewDetails", "Colleges", new { id = course.CollegeId });
                }

                else
                {
                    // Default redirection if the URL does not match any specific pattern
                    return RedirectToAction("Index", "Colleges", new { id = course.CollegeId });
                }
                //return RedirectToAction("ViewDetails", "Colleges", new { id = course.CollegeId });
                //return View(course);  
                //return RedirectToAction(nameof(Index));
                //return RedirectToAction("Index", "Colleges");
            }
            ViewData["CollegeId"] = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
            return View(course);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOnMain(int id, [Bind("CourseId,Name,Fees,DurationOfCourse,CollegeId,Publish,CoureseFor,PublicationAccrediet,FreeCourese,BooksForCourse,BracodeOfBook,AvailablesON")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = "Course details updated successfully.";
                    ViewBag.CourseName = course.Name;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


                return RedirectToAction("Index", "Colleges", new { id = course.CollegeId });
                //return View(course);  
                //return RedirectToAction(nameof(Index));
                //return RedirectToAction("Index", "Colleges");
            }
            ViewData["CollegeId"] = new SelectList(_context.Colleges, "CollegeId", "CollegeId", course.CollegeId);
            return View(course);

        }
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.College)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return PartialView(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewDetails", "Colleges", new { id = course.CollegeId });

        }


        public async Task<JsonResult> DeleteData(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return Json(new { success = false });
            ViewBag.courseName = course.Name;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }



        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }



        public IActionResult ViewIndex(int collegeId)
        {
            // Retrieve courses based on the collegeId
            var courses = GetCoursesByCollegeId(collegeId); // Replace with your data retrieval logic
            return PartialView("_CoursesIndex", courses); // Replace "_CourseList" with your partial view name
        }

        //public IActionResult ViewIndex(int collegeId, int pageIndex = 1, int pageSize = 10)
        //{
        //    // Retrieve all courses based on the collegeId
        //    var allCourses = GetCoursesByCollegeId(collegeId); // Replace with your data retrieval logic

        //    // Count total courses
        //    var count = allCourses.Count();

        //    // Get the courses for the current page
        //    var courses = allCourses
        //                  .Skip((pageIndex - 1) * pageSize)
        //                  .Take(pageSize)
        //                  .ToList();

        //    // Create a PaginatedList object
        //    var paginatedCourses = new PaginatedList<Course>(courses, count, pageIndex, pageSize);

        //    ViewData["collegeId"] = collegeId;
        //    ViewData["pageSize"] = pageSize;

        //    return PartialView("_CoursesIndex", paginatedCourses); // Replace "_CoursesIndex" with your partial view name
        //}


        private List<Course> GetCoursesByCollegeId(int collegeId)
        {
            // Your logic to retrieve courses by collegeId
            // Example:
            return _context.Courses.Where(c => c.CollegeId == collegeId).ToList();
        }
    }
}



