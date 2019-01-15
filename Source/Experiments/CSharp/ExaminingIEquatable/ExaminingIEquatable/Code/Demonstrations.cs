using System;
using System.Collections.Generic;


namespace ExaminingIEquatable
{
    public static class Demonstrations
    {
        public static void SubMain()
        {
            //Demonstrations.WhatTypeIsTheDefaultEqualityComparer();
            Demonstrations.IsDictionaryComparerTheDefaultComparer();
        }

        /// <summary>
        /// Result: True.
        /// There are many claims that <see cref="Dictionary{TKey, TValue}"/> uses the <see cref="EqualityComparer{T}.Default"/> comparer for TKey if none is specified in its constructor.
        /// Here we demonstrate whether or not that's the case.
        /// 
        /// * An example of the claim: https://social.msdn.microsoft.com/Forums/en-US/0cb998e8-6c2c-4e42-acb7-a1d9de57c684/dictionary-with-iequatable-key-how-to-implement?forum=netfxbcl
        /// </summary>
        private static void IsDictionaryComparerTheDefaultComparer()
        {
            var integersByName = new Dictionary<string, int>();

            var isDefaulComparer = integersByName.Comparer == EqualityComparer<string>.Default;

            Console.WriteLine($@"The dictionary uses the default comparer: {isDefaulComparer}");
        }

        /// <summary>
        /// Result: System.Collections.Generic.GenericEqualityComparer`1[System.String]
        /// What is the type of <see cref="EqualityComparer{T}.Default"/>?
        /// The <see cref="EqualityComparer{T}"/> is actually an abstract base-class, so the question arises, what exactly IS the type of the instance returned by <see cref="EqualityComparer{T}.Default"/>?
        /// 
        /// * Reference source for the internal GenericEqualityComparer&lt;T&gt;: https://referencesource.microsoft.com/#mscorlib/system/collections/generic/equalitycomparer.cs,e59de2b23f38e633
        /// </summary>
        private static void WhatTypeIsTheDefaultEqualityComparer()
        {
            var defaultStringComparer = EqualityComparer<string>.Default;

            Console.WriteLine($@"EqualityComparer<string>.Default: {defaultStringComparer}");
        }
    }
}
