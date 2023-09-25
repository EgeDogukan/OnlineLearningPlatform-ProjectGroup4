using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Context _context;

       private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public CoursesController(Context context,SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchTitle)
        {
            var courses = _context.Courses.AsQueryable();
            var user = await _userManager.GetUserAsync(User);
            var enrolledCourses = _context.Enrollments.AsQueryable();
            enrolledCourses = enrolledCourses.Where(e => e.UserId == user.Id);
            var enrolledIds = enrolledCourses.Select(e => e.CourseId).ToList();
            ViewBag.enrolledIds = enrolledIds;
            if (!string.IsNullOrEmpty(searchTitle))
            {
                // Filter courses by title if a search query is provided
                courses = courses.Where(c => c.Title.ToLower().Contains(searchTitle.ToLower()));
            }

            var courseList = await courses.ToListAsync();

            return View(courseList);
            //var context = _context.Courses.Include(c => c.User);
            ////if (!_signInManager.IsSignedIn(User))
            ////{
            ////    return RedirectToRoute("/Identity/Account/Login");
            ////}
            ////var currentUser = await _userManager.GetUserAsync(User);
            ////var userId = currentUser.Id;
            ////ViewBag.InstructorId = userId;

            //return View(await context.ToListAsync());
        }

        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user.Role == "Student")
            {
                var myEnrollments = _context.Enrollments.AsQueryable();

                myEnrollments = myEnrollments.Where(e => e.UserId == user.Id);
                var enrollmentsList = await myEnrollments.ToListAsync();
                var courseList = new List<Course>();

                foreach (var item in enrollmentsList)
                {
                    courseList.Add(_context.Courses.Find(item.CourseId));
                }
                return View(courseList);
            }
            if (user.Role == "Teacher")
            {
                var myCourses = _context.Courses.AsQueryable();
                myCourses = myCourses.Where(e => e.InstructorId == user.Id);
                var courseList = await myCourses.ToListAsync();
                return View(courseList);

            }
            if(user.Role == "Admin")
            {
                var courseList = _context.Courses.AsQueryable();
                return View(courseList);

            }
            return RedirectToAction("Account/Login", "Identity");
           
            


            
        }

        //public async Task<IActionResult> Search(string searchTitle)
        //{
        //    var courses = _context.Courses.AsQueryable();

        //    if (!string.IsNullOrEmpty(searchTitle))
        //    {
        //        // Filter courses by title if a search query is provided
        //        courses = courses.Where(c => c.Title.ToLower().Contains(searchTitle.ToLower()));
        //    }

        //    var courseList = await courses.ToListAsync();

        //    return RedirectToAction("Index",courseList);
        //    //var context = _context.Courses.Include(c => c.User);
        //    ////if (!_signInManager.IsSignedIn(User))
        //    ////{
        //    ////    return RedirectToRoute("/Identity/Account/Login");
        //    ////}
        //    ////var currentUser = await _userManager.GetUserAsync(User);
        //    ////var userId = currentUser.Id;
        //    ////ViewBag.InstructorId = userId;

        //    //return View(await context.ToListAsync());
        //}


        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            var courseContents = _context.CourseContent.AsQueryable();
            courseContents = courseContents.Where(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.courseContents = await courseContents.ToListAsync();
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles ="Teacher,Admin")]
        
        public IActionResult Create()
        {
            ViewData["InstructorId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Course course)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            course.InstructorId = currentUser.Id;
            course.User = currentUser;

            //if (ModelState.IsValid)
            //{
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["InstructorId"] = new SelectList(_context.Users, "UserId", "UserId", course.InstructorId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["InstructorId"] = new SelectList(_context.Users, "UserId", "UserId", course.InstructorId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Description,InstructorId,Category,EnrollmentCount,ImageUrl")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }
       var user= await _userManager.GetUserAsync(User);
            course.InstructorId = user.Id;
        
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["InstructorId"] = new SelectList(_context.Users, "UserId", "UserId", course.InstructorId);
            //return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'Context.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
