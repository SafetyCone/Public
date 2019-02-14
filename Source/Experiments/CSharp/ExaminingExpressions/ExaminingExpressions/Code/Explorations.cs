using System;
using System.Linq.Expressions;

using LinqKit;

using ExaminingClasses.Lib.School;


namespace ExaminingExpressions
{
    public static class Explorations
    {
        public static void SubMain()
        {
            //Explorations.BasicExpression();
            //Explorations.BuildBasicExpressionTree();
            //Explorations.ExplicateExpressionTree();
            //Explorations.CreateStudentPredicate();
            Explorations.CreateCompoudnStudentPredicate();
        }

        public static void CreateCompoudnStudentPredicate()
        {
            Expression<Func<Student, int>> studentIdentitySelectorExpression = s => s.ID;

            var studentIdentityEqual1 = Explorations.BuildExpressionForEqualsStudentIdentity(studentIdentitySelectorExpression, 1);
            var studentIdentityEqual2 = Explorations.BuildExpressionForEqualsStudentIdentity(studentIdentitySelectorExpression, 2);
            var studentIdentityEqual3 = Explorations.BuildExpressionForEqualsStudentIdentity(studentIdentitySelectorExpression, 3);

            var predicate = PredicateBuilder.New<Student>();

            predicate = predicate.Or(studentIdentityEqual1);
            predicate = predicate.Or(studentIdentityEqual2);
            predicate = predicate.Or(studentIdentityEqual3);

            var predicateFunc = predicate.Compile();

            var student = Utilities.GetExampleStudent();

            var isStudentInIdentitiesList = predicateFunc(student);

            Console.WriteLine($@"Is student in list: {isStudentInIdentitiesList}");
        }

        /// <summary>
        /// Create a predicate expression and test it.
        /// </summary>
        public static void CreateStudentPredicate()
        {
            var student = Utilities.GetExampleStudent();

            Func<Student, int> studentIdentitySelector = s => s.ID;

            Expression<Func<Student, int>> studentIdentitySelectorExpression = s => s.ID;

            var studentIdentity = studentIdentitySelector(student);

            // Build predicate expression.
            var studentIdentityEqual = Explorations.BuildExpressionForEqualsStudentIdentity(studentIdentitySelectorExpression, studentIdentity);

            var studentIdentityEqualFunc = studentIdentityEqual.Compile();

            var isStudentIdentityEqual = studentIdentityEqualFunc(student);
        }

        private static Expression<Func<Student, bool>> BuildExpressionForEqualsStudentIdentity(Expression<Func<Student, int>> studentIdentitySelectorExpression, int studentIdentity)
        {
            var studentIdentityConstant = Expression.Constant(studentIdentity);

            var studentParameter = Expression.Parameter(typeof(Student), @"s");

            var studentIdentityEqualBody = Expression.Equal(Expression.Invoke(studentIdentitySelectorExpression, studentParameter), studentIdentityConstant);

            var studentIdentityEqual = Expression.Lambda<Func<Student, bool>>(studentIdentityEqualBody, studentParameter);
            return studentIdentityEqual;
        }

        /// <summary>
        /// Shows the inner structure of an expression tree.
        /// Source: https://www.tutorialsteacher.com/linq/expression-tree
        /// </summary>
        private static void ExplicateExpressionTree()
        {
            Expression<Func<Student, bool>> isTeenagerExpression = s => s.Age > 12 && s.Age < 20;

            Console.WriteLine($@"Expression: {isTeenagerExpression}");
            Console.WriteLine($@"Expression Type: {isTeenagerExpression.NodeType}");

            var parameters = isTeenagerExpression.Parameters;
            foreach (var parameter in parameters)
            {
                Console.WriteLine($@"Parameter Name: {parameter.Name}");
                Console.WriteLine($@"Parameter Type: {parameter.Type.Name}");
            }

            var bodyExpression = isTeenagerExpression.Body as BinaryExpression;

            Console.WriteLine($@"Left side of body expression: {bodyExpression.Left}");
            Console.WriteLine($@"Binary Expression Type: {bodyExpression.NodeType}");
            Console.WriteLine($@"Right side of body expression: {bodyExpression.Right}");
            Console.WriteLine($@"Return Type: {isTeenagerExpression.ReturnType}");
        }

        /// <summary>
        /// Build a basic expression tree.
        /// Source: https://www.tutorialsteacher.com/linq/expression-tree
        /// </summary>
        private static void BuildBasicExpressionTree()
        {
            // Create a tree for isAdult: Func<Student, bool> isAdult = s => s.age >= 18;

            var studentParameter = Expression.Parameter(typeof(Student), @"s");
            var studentAgeMember = Expression.Property(studentParameter, @"Age");
            var age18 = Expression.Constant(18, typeof(int));
            var studentIsAdultBody = Expression.GreaterThanOrEqual(studentAgeMember, age18);
            var isAdult = Expression.Lambda<Func<Student, bool>>(studentIsAdultBody, studentParameter);

            Console.WriteLine($@"Expression Tree: {isAdult}");
            Console.WriteLine($@"Expression Tree Body: {isAdult.Body}");
            Console.WriteLine($@"Number of Parameters in Expression Tree: {isAdult.Parameters.Count}");
            Console.WriteLine($@"Parameters in Expression Tree: {isAdult.Parameters[0]}");
        }

        /// <summary>
        /// Construct and execute an expression.
        /// </summary>
        private static void BasicExpression()
        {
            var student = Utilities.GetExampleStudent();

#pragma warning disable IDE0039 // Use local function
            Func<Student, bool> isTeenagerFunc = s => s.Age > 12 && s.Age < 18;
#pragma warning restore IDE0039 // Use local function

            Console.WriteLine($@"{nameof(isTeenagerFunc)}: {isTeenagerFunc(student)}");

            Expression<Func<Student, bool>> isTeenagerExpression = s => s.Age > 12 && s.Age < 18;

            var isTeenagerExpressionToFunc = isTeenagerExpression.Compile();

            Console.WriteLine($@"{nameof(isTeenagerExpressionToFunc)}: {isTeenagerExpressionToFunc(student)}");

            var studentExpression = Expression.Constant(student);

            // This is not what I was thinking it would be...
            Console.WriteLine($@"{nameof(isTeenagerExpression)}: {Expression.Invoke(isTeenagerExpression, studentExpression)}");
        }
    }
}
