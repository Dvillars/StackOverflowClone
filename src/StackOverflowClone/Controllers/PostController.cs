using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackOverflowClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StackOverflowClone.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StackOverflowContext _db;

        public PostsController(UserManager<ApplicationUser> userManager, StackOverflowContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Posts.Where(x => x.User.Id == currentUser.Id));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            post.User = currentUser;
            post.Rating = 0;
            post.Date = new DateTime();
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public IActionResult Details(int id)
        //{
        //    var thisPost = db.Posts
        //        .FirstOrDefault(posts => posts.PostId == id);
        //    return View();
        //}

        //public IActionResult Edit(int id)
        //{
        //    var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
        //    return View(thisPost);
        //}
        //[HttpPost]
        //public IActionResult Edit(Post Post)
        //{
        //    db.Entry(Post).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //public ActionResult Delete(int id)
        //{
        //    var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
        //    return View(thisPost);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
        //    db.Posts.Remove(thisPost);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}