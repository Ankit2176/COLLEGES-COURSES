using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeAndCourses.Data;
using CollegeAndCourses.Models;
using CollegeAndCourses.Models.ViewModels;
using Newtonsoft.Json;

namespace CollegeAndCourses.Controllers
{
    public class CollegesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollegesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Colleges
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Colleges.ToListAsync());
        //}

        public async Task<IActionResult> Index(string searchString, string sortOrder, int pageNumber = 1, int pageSize = 5)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_asc" : sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["GradeSortParm"] = sortOrder == "grade_asc" ? "grade_desc" : "grade_asc";
            ViewData["AddressSortParm"] = sortOrder == "address_asc" ? "address_desc" : "address_asc";
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPageSize"] = pageSize;
            ViewData["PageSize"] = pageSize; // Add pageSize to ViewData to use it in the view
            var collegesQuery = _context.Colleges.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                collegesQuery = collegesQuery.Where(c => c.Name.Contains(searchString));
            }

            var totalRecords = await collegesQuery.CountAsync();

             switch (sortOrder)
            {
                case "name_desc":
                    collegesQuery = collegesQuery.OrderByDescending(c => c.Name);
                    break;
                case "grade_asc":
                    collegesQuery = collegesQuery.OrderBy(c => c.Grade);
                    break;
                case "grade_desc":
                    collegesQuery = collegesQuery.OrderByDescending(c => c.Grade);
                    break;
                case "address_asc":
                    collegesQuery = collegesQuery.OrderBy(c => c.Address);
                    break;
                case "address_desc":
                    collegesQuery = collegesQuery.OrderByDescending(c => c.Address);
                    break;
                case "date_asc":
                    collegesQuery = collegesQuery.OrderBy(c => c.dateTime);
                    break;
                case "date_desc":
                    collegesQuery = collegesQuery.OrderByDescending(c => c.dateTime);
                    break;
                default:
                    collegesQuery = collegesQuery.OrderBy(c => c.Name);
                    break;
            }

            var data = await collegesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatedList = new PaginatedList<College>(data, totalRecords, pageNumber, pageSize);
            ViewData["TotalRecords"] = totalRecords;

            return View(paginatedList);
        }






        // GET: Colleges/ViewDetails/5
        public async Task<IActionResult> ViewDetails(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var college = await _context.Colleges
            //    .FirstOrDefaultAsync(m => m.CollegeId == id);
            //if (college == null)
            //{
            //    return NotFound();
            //}


            var college = _context.Colleges.FirstOrDefault(c => c.CollegeId == id);
            if (college == null)
            {
                return NotFound();
            }

            var courses = _context.Courses.Where(course => course.CollegeId == id).ToList();

            var viewModel = new CollegesAndCoursesViewModel
            {
                College = college,
                Courses = courses
            };

            ViewBag.CollegeName = college.Name;
            return View(viewModel);
        }

        // GET: Colleges/Details/5
        public async Task<IActionResult> Details(int? id)
           {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var college = await _context.Colleges
            //    .FirstOrDefaultAsync(m => m.CollegeId == id);

            var college = _context.Colleges.Find(id);
            if (college == null)
            {
                return NotFound();
            }

            return PartialView("Details",college);
        }

        // GET: Colleges/Create
        public IActionResult Create()
        {
            // Initialize an empty view model with a new College object
            var viewModel = new CollegesAndCoursesViewModel
            {
                College = new College(),
                Courses = new List<Course>() // You can also initialize with a few empty courses if needed

            };
            
            var currentDate = DateTime.Now;

            // Pass the current date to the view using ViewData
            ViewData["CurrentDate"] = currentDate;
            return View(viewModel);
        }


        // POST: Colleges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CollegeId,Name,Grade,Address,dateTime,PreferredContactMethod,IsNAACAccrediet,RecevieNewsLetter,HowManyStudents,PhoneNo,Branches")] College college)
        {
            if (ModelState.IsValid)
            {
                if (college.Branches != null)
                {
                    // Convert the list of branches to a semicolon-separated string
                    college.BranchesString = string.Join("; ", college.Branches);
                }

                string courseJson = TempData["NewCourse"] as string;

                // Deserialize the JSON string back to Course object


                if (courseJson == " ")
                {
                    // Save the student to get the StudentId
                    _context.Colleges.Add(college);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Course newCourse = JsonConvert.DeserializeObject<Course>(courseJson);

                    _context.Colleges.Add(college);
                    _context.SaveChanges();

                    newCourse.CollegeId = college.CollegeId;

                    // Save the course to the database
                    _context.Courses.Add(newCourse);
                    _context.SaveChanges();

                    // Redirect to Index or another appropriate action
                    return RedirectToAction(nameof(Index));
                }





                //_context.Add(college);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }

            return View(college);
        }


        // GET: Colleges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var college = await _context.Colleges.FindAsync(id);
            //if (college == null)
            //{
            //    return NotFound();
            //}
            //return View(college);

            var college = _context.Colleges.FirstOrDefault(c => c.CollegeId == id);
            if (college == null)
            {
                return NotFound();
            }

            var courses = _context.Courses.Where(course => course.CollegeId == id).ToList();
             ViewBag.CollegeName = college.Name;
            var viewModel = new CollegesAndCoursesViewModel
            {
                College = college,
                Courses = courses
            };

            return View(viewModel);

        }

        // POST: Colleges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CollegeId,Name,Grade,Address,dateTime,PreferredContactMethod,IsNAACAccrediet,RecevieNewsLetter,HowManyStudents,PhoneNo,Branches")] College college)
        {

            if (id != college.CollegeId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(college);
                    await _context.SaveChangesAsync();

                    ViewBag.Message = "College details updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollegeExists(college.CollegeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
                }
                //var viewModel = new CollegesAndCoursesViewModel
                //{
                //    College = college,
                //    // Initialize other properties of the view model as needed
                //
                //};

                return View(college);
        }

        // GET: Colleges/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var college = await _context.Colleges
        //        .FirstOrDefaultAsync(m => m.CollegeId == id);
        //    if (college == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(college);
        //}

        public async Task<IActionResult> Delete(int? id)
        {
            var course = _context.Courses.Find(id)
        ;
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("Index");


        }

        // POST: Colleges/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var college = await _context.Colleges.FindAsync(id);
        //    if (college != null)
        //    {
        //        _context.Colleges.Remove(college);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CollegeExists(int id)
        //{
        //    return _context.Colleges.Any(e => e.CollegeId == id);
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            var college = await _context.Colleges.FindAsync(id);
            if (college == null) return Json(new { success = false });
            ViewBag.CollegeName = college.Name;
            _context.Colleges.Remove(college);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }


        private bool CollegeExists(int id)
        {
            return _context.Colleges.Any(e => e.CollegeId == id);
        }
    }
}
