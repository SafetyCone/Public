using System;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    /// <summary>
    /// A blog-post.
    /// </summary>
    public class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogID { get; set; }
        public Blog Blog { get; set; }
    }
}
