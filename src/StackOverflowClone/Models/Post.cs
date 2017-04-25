using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflowClone.Models
{ 
    [Table("Posts")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
