using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackOverflowClone.Models;

namespace StackOverflowClone.Controllers
{
    public class PostsController : Controller
    {
        private StackOverflowContext db = new StackOverflowContext();

        public IActionResult Index()
        {
            return View(db.Posts.ToList());
        }
        public IActionResult Details(int id)
        {
            var thisPost = db.Posts
                .FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Post Post)
        {
            db.Posts.Add(Post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        [HttpPost]
        public IActionResult Edit(Post Post)
        {
            db.Entry(Post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            db.Posts.Remove(thisPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}