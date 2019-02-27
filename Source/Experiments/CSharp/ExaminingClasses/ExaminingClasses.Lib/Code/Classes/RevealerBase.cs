using System;


namespace ExaminingClasses.Lib
{
    /// <summary>
    /// The Revealer experiment tests whether a protected property value of an instances of a base-class can be revealed by a derived class that takes in an instance of the base class.
    /// </summary>
    public class RevealerBase
    {
        protected int Value { get; set; }


        public RevealerBase()
        {
        }

        public RevealerBase(int value)
        {
            this.Value = value;
        }
    }
}
