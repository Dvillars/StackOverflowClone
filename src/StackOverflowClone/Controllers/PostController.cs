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
            post.Author = currentUser.UserName;
            post.Rating = 0;
            post.Date = new DateTime();
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        [HttpPost]
        public IActionResult Edit(Post Post)
        {
            _db.Entry(Post).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            _db.Posts.Remove(thisPost);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpVote(int id, int userId)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            var foundId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(foundId);

            var currentRating = _db.Ratings.Where(rating => rating.UserId == userId).Where(rating => rating.PostId == id).FirstOrDefault();

            if(currentRating != null)
            {
                if (!currentRating.Upvote)
                {
                    currentRating.Upvote = true;
                    thisPost.Rating += 2;
                }
                return RedirectToAction("Details", "Home", new { id = thisPost.PostId });
            } else
            {
                var newRating = new Rating();
                newRating.UserId = userId;
                newRating.PostId = id;
                newRating.Upvote = true;
                _db.Ratings.Add(newRating);
                thisPost.Rating++;
                _db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = thisPost.PostId });
            }

        }

        public async Task<IActionResult> DownVote(int id, int userId)
        {
            var thisPost = _db.Posts.FirstOrDefault(posts => posts.PostId == id);
            var foundId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(foundId);

            var currentRating = _db.Ratings.Where(rating => rating.UserId == userId).Where(rating => rating.PostId == id).FirstOrDefault();

            if (currentRating != null)
            {
                if(currentRating.Upvote)
                {
                    currentRating.Upvote = false;
                    thisPost.Rating -= 2;
                }
                return RedirectToAction("Details", "Home", new { id = thisPost.PostId });
            }
            else
            {
                var newRating = new Rating();
                newRating.UserId = userId;
                newRating.PostId = id;
                newRating.Upvote = false;
                _db.Ratings.Add(newRating);
                thisPost.Rating--;
                _db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = thisPost.PostId });
            }
        }
    }
}