using System;
using System.Collections.Generic;


namespace ExaminingCSharp
{
    public static class Experiments
    {
        public static void SubMain()
        {
            Experiments.CanSetInDictionary();
        }

        /// <summary>
        /// Result: False! You CAN set the value of a key that has not yet been added!
        /// Does the System dictionary allow setting values for keys that have not yet been added?
        /// Expected: Attempting to set the value of a not-yet added key will throw an error.
        /// 
        /// 
        /// </summary>
        private static void CanSetInDictionary()
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var key in dictionary.Keys)
            {
                // Do nothing, this is just to get over the initialization.
            }

            dictionary[@"one"] = 1;
        }
    }
}
