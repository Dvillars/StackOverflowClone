using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StackOverflowClone.Models;
using Microsoft.AspNetCore.Identity;


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

        [HttpPost]
        public async Task<IActionResult> Create(string body, int id)
        {
            var comment = new Comment();
            comment.Body = body;
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            comment.Post = _db.Posts
                .FirstOrDefault(posts => posts.PostId == id);
            comment.User = currentUser;
            comment.Author = currentUser.UserName;
            comment.Rating = 0;
            comment.Date = new DateTime();
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
