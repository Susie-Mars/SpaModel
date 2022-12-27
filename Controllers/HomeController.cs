using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using CSharpProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        // HttpContext.Session.GetInt32("UserId");
        ViewBag.AllPosts = _context.Posts.OrderByDescending(p => p.CreatedAt).ToList();

        return View();
    }



    [HttpGet("loginreg")]
    public IActionResult LoginReg()
    {

        return View();
    }


    [HttpGet("bodytreatments")]
    public IActionResult BodyTreatments()
    {
        ViewBag.BodyTreatments = _context.Posts.Where(t => t.ServiceType == "Body Treatment").OrderByDescending(p => p.CreatedAt).ToList();

        return View();
    }

    [HttpGet("facials")]
    public IActionResult Facials()
    {
        ViewBag.Facials = _context.Posts.Where(t => t.ServiceType == "Facial").OrderByDescending(p => p.CreatedAt).ToList();

        return View();
    }

    [HttpGet("nailservices")]
    public IActionResult NailServices()
    {
        ViewBag.NailServices = _context.Posts.Where(t => t.ServiceType == "Nail Service").OrderByDescending(p => p.CreatedAt).ToList();

        return View();
    }

    [SessionCheck]
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        // MyViewModel MyModel = new MyViewModel
        // {
        //     AllPosts = _context.Posts.Where(p => p.UserId == (int)HttpContext.Session.GetInt32("UserId")).ToList()
        // };
        // List<Posting> AllUserPosts = _context.Posts.Where(p => p.UserId == (int)HttpContext.Session.GetInt32("UserId")).ToList();
        ViewBag.AllUserPosts = _context.Posts.Where(p => p.UserId ==(int)HttpContext.Session.GetInt32("UserId")).OrderByDescending(p => p.CreatedAt).ToList();
        return View();
    }

    [HttpPost("users/create")]
    public IActionResult CreateUser(User newUser)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("I'm in the if state");
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.AllPosts = _context.Posts.OrderByDescending(p => p.CreatedAt).ToList();
            return View("LoginReg");
        }
    }

    [HttpPost("users/login")]
    public IActionResult LogUser(Login loginUser)
    {
        if (ModelState.IsValid)
        {
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == loginUser.UEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("UEmail", "Invalid Email or Password");
                return View("LoginReg");
            }
            PasswordHasher<Login> hasher = new PasswordHasher<Login>();
            var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.UPassword);
            if (result == 0)
            {
                ModelState.AddModelError("UEmail", "Invalid Email or Password");
                return View("LoginReg");
            }
            else
            {
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                ViewBag.AllUserPosts = _context.Posts.Where(p => p.UserId ==(int)HttpContext.Session.GetInt32("UserId")).OrderByDescending(p => p.CreatedAt).ToList();

                return View("Dashboard");
            }
            
        }
        else
        {
            ViewBag.AllPosts = _context.Posts.OrderByDescending(p => p.CreatedAt).ToList();

            return View("LoginReg");
            
        }
    }

    [HttpPost("posts/create")]
    public IActionResult CreatePost(Posting newPost)
    {
        Console.WriteLine($"********TIME {newPost.Time}");
        if (ModelState.IsValid)
        {
            newPost.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newPost);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.AllPosts = _context.Posts.Where(p => p.UserId ==(int)HttpContext.Session.GetInt32("UserId")).OrderByDescending(p => p.CreatedAt).ToList();

            return RedirectToAction("Dashboard");
        }
    }

    [SessionCheck]
    [HttpGet("posts/{PostId}/edit")]
    public IActionResult EditPost(int postId)
    {
        Posting? postToEdit = _context.Posts.FirstOrDefault(i => i.PostingId == postId);
        return View(postToEdit);
    }

    [SessionCheck]
    [HttpPost("posts/{postId}/update")]
    public IActionResult UpdatePost(Posting newPost, int postId)
    {
        if (ModelState.IsValid)
        {
            Posting? PostToUpdate = _context.Posts.FirstOrDefault(i => i.PostingId == postId);
            if(PostToUpdate != null)
            {
                PostToUpdate.ServiceType = newPost.ServiceType;
                PostToUpdate.Location = newPost.Location;
                PostToUpdate.Website = newPost.Website;
                PostToUpdate.Date = newPost.Date;
                PostToUpdate.Time = newPost.Time;
                PostToUpdate.ContactInfo = newPost.ContactInfo;
                PostToUpdate.Description = newPost.Description;
                PostToUpdate.UpdatedAt = DateTime.Now;
                // _context.Add(newPost);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", new{postId = postId});
            }
        }
            return View("EditPost", newPost);

    }

    [SessionCheck]
    [HttpPost("posts/{postId}/destroy")]
    public IActionResult DestroyPost(int postId)
    {
        Posting? PostDestroy = _context.Posts.SingleOrDefault(u => u.PostingId == postId);
        if(PostDestroy == null)
        {
            return RedirectToAction("Dashboard");
        }
        _context.Remove(PostDestroy);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
