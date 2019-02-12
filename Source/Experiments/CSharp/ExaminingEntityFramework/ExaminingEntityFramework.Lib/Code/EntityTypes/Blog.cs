using System;
using System.Collections.Generic;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    public class Blog
    {
        public int BlogID { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }
}
