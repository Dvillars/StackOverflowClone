using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StackOverflowClone.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StackOverflowClone.Controllers
{
    public class CommentController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StackOverflowContext _db;

        public CommentController(UserManager<ApplicationUser> userManager, StackOverflowContext db)
        {
            _userManager = userManager;
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int id)
        {
            ViewBag.PostId = id;
            Console.WriteLine(ViewBag.PostId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int PostId, string Body)
        {
            var comment = new Comment();
            comment.Body = Body;
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            comment.Post = _db.Posts
                .FirstOrDefault(posts => posts.PostId == PostId);
            comment.User = currentUser;
            comment.Author = currentUser.UserName;
            comment.Rating = 0;
            comment.Date = new DateTime();
            _db.Comments.Add(comment);
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { id = PostId });
        }
    }
}
