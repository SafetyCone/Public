using System;

using ExaminingClasses.Lib.School;


namespace ExaminingExpressions
{
    public static class Utilities
    {
        /// <summary>
        /// ID = 1, Name = "Joe", Age = 15.
        /// </summary>
        /// <returns></returns>
        public static Student GetExampleStudent()
        {
            var student = new Student()
            {
                ID = 1,
                Name = @"Joe",
                Age = 15,
            };

            return student;
        }
    }
}
