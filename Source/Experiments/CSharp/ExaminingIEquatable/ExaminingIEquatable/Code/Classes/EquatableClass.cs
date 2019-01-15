using System;
using System.Collections.Generic;


namespace ExaminingIEquatable
{
    /// <summary>
    /// Sources:
    /// Adapted from: More Effective C#: 50 Specific Ways to Improve your C#, 2nd Edition (June 1st, 2017) - Item 9: Understand the Relationships Among the Many Different Concepts of Equality.
    /// </summary>
    public class EquatableClass : IEquatable<EquatableClass>
    {
        public bool Equals(EquatableClass other)
        {
            throw new NotImplementedException();
        }
    }
}
