using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflowClone.Models
{
    [Table("Ratings")]
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public bool Upvote { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
