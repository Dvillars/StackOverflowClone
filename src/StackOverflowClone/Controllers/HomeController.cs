using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StackOverflowClone.Controllers
{
    public class HomeController : Controller
    {
        private StackOverflowContext db = new StackOverflowContext();
        public IActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        public IActionResult Details(int id)
        {
            var thisPost = db.Posts
                .Include(p => p.Comments)
                .FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }
    }
}
