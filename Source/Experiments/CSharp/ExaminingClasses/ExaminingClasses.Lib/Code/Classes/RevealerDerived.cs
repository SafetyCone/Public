using System;


namespace ExaminingClasses.Lib
{
    /// <summary>
    /// Result: UNEXPECTED! Instances of a derived class are treated just like any other member of the public when it comes to being able to access protected properties.
    /// The Revealer experiments tests whether a protected property value of a base class instance can be revealed by a derived class.
    /// Expected: Yes, protected properties of 
    /// </summary>
    public class RevealerDerived : RevealerBase
    {
        public RevealerDerived()
        {
        }

        public RevealerDerived(int value)
            : base(value)
        {
        }

        public RevealerDerived(RevealerBase baseInstance)
            //: this(baseInstance.va)
        {
            //baseInstance.va   
        }

        //public int GetValue(RevealerBase revealerBase)
        //{
        //    var output = revealerBase.valu
        //}
    }
}
