using System;
using System.Collections.Generic;

#nullable disable

namespace ApiApp.Data
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumb { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool Active { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public int? View { get; set; }
        public int CategoryId { get; set; }
        public int? UserId { get; set; }
        public string Author { get; set; }

        public virtual Category Category { get; set; }
    }
}
