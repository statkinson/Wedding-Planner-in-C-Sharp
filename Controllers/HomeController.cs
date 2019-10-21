using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wedding_planner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;

namespace wedding_planner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController (MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult index()
        {
            return View();
        }
        [HttpGet("login")]
        public IActionResult login()
        {
            return View("login");
        }
        [HttpPost("register")]
        public IActionResult register(User newuser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newuser.Email))
                {
                  ModelState.AddModelError("Email", "Email already in use!");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newuser.Password = Hasher.HashPassword(newuser, newuser.Password);

                    dbContext.Add(newuser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("my_val", newuser.UserId);
                    return RedirectToAction("dashboard");
                }
            }
            return View("index");
        }
        [HttpPost("loginprocess")]
        public IActionResult loginprocess(LoginUser user_id)
        {
            System.Console.WriteLine("We're outside the valid check.");
            if(ModelState.IsValid)
            {
                System.Console.WriteLine("We're inside the valid check.");
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user_id.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }

                PasswordHasher<User> loginHash = new PasswordHasher<User>();
                var result = loginHash.VerifyHashedPassword(userInDb, userInDb.Password, user_id.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                HttpContext.Session.SetInt32("my_val", userInDb.UserId);
                return RedirectToAction("dashboard");
            }
            return View("login");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }
        [HttpGet("dashboard")]
        public IActionResult dashboard(int weddingId)
        {
            int? user_id = HttpContext.Session.GetInt32("my_val");

            if (user_id == null)
                return RedirectToAction("index");

            List<Wedding> Aloha = dbContext.Weddings.Include(Wedding => Wedding.Guests).ThenInclude(rsvp => rsvp.Wedding).ToList();
            ViewBag.CurrentUser = user_id;
            return View(Aloha);
        }
        [HttpGet("new")]
        public IActionResult newwedding()
        {
            int? user_id = HttpContext.Session.GetInt32("my_val");

            return View();
        }
        [HttpPost("Bob")]
        public IActionResult create(Wedding Aloha)
        {
            int? IntVariable = HttpContext.Session.GetInt32("my_val");
            Aloha.UserId = (int)IntVariable;
            dbContext.Add(Aloha);
            dbContext.SaveChanges();
            return RedirectToAction("weddingdetails", new {WeddingId = Aloha.WeddingId});
        }
       [HttpGet("details/{WeddingId}")]
        public IActionResult weddingdetails(int weddingId)
        {
            int? user_id = HttpContext.Session.GetInt32("my_val");
            Wedding dawedding = dbContext.Weddings.Include(Wedding => Wedding.Guests).ThenInclude(rsvp => rsvp.User).FirstOrDefault(wedding => wedding.WeddingId == weddingId);
            

            return View(dawedding);
        }
        [HttpGet("delete/{Id}")]
        public IActionResult delete(int Id)
        {
            Wedding thiswedding = dbContext.Weddings.FirstOrDefault(wedding => wedding.WeddingId == Id);
            dbContext.Remove(thiswedding);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard");
        }
        [HttpGet("AddUsertoWedding/{Id}")]
        public IActionResult AddUsertoWedding(int Id)
        {
            RSVP attending = new RSVP();
            attending.WeddingId = Id;
            attending.UserId = (int)HttpContext.Session.GetInt32("my_val");
            dbContext.Add(attending);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard");
        }
        [HttpGet("RemoveUsertoWedding/{Id}/{UserId}")]
        public IActionResult RemoveUsertoWedding(int Id, int UserId)
        {
            RSVP thisrsvp = dbContext.RSVPs.FirstOrDefault(attending => attending.WeddingId == Id && attending.UserId == UserId);
            dbContext.Remove(thisrsvp);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
