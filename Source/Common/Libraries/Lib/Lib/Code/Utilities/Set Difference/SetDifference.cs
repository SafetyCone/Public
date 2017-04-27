using System;
using System.Collections.Generic;


namespace Public.Common.Lib
{
    public class SetDifference<T>
    {
        public const string DefaultSet1Name = @"Set1";
        public const string DefaultSet2Name = @"Set2";


        #region Static

        public static SetDifference<T> Calculate(IEnumerable<T> set1, IEnumerable<T> set2)
        {
            SetDifference<T> output = SetDifference<T>.Calculate(set1, SetDifference<T>.DefaultSet1Name, set2, SetDifference<T>.DefaultSet2Name);
            return output;
        }

        public static SetDifference<T> Calculate(IEnumerable<T> set1, string set1Name, IEnumerable<T> set2, string set2Name)
        {
            SetDifference<T> output = new SetDifference<T>(set1Name, set2Name);

            HashSet<T> set1Only = new HashSet<T>(set1);
            set1Only.ExceptWith(set2);

            HashSet<T> set2Only = new HashSet<T>(set2);
            set2Only.ExceptWith(set1);

            output.InSet1Only.AddRange(set1Only);
            output.InSet2Only.AddRange(set2Only);

            return output;
        }

        #endregion


        public string Set1Name { get; set; }
        public string Set2Name { get; set; }
        public List<T> InSet1Only { get; set; }
        public List<T> InSet2Only { get; set; }
        public bool NoDifference
        {
            get
            {
                bool output = 0 == this.InSet1Only.Count && 0 == this.InSet2Only.Count;
                return output;
            }
        }


        public SetDifference()
        {
            this.Setup();
        }

        public SetDifference(string set1Name, string set2Name)
        {
            this.Setup();

            this.Set1Name = set1Name;
            this.Set2Name = set2Name;
        }

        private void Setup()
        {
            this.InSet1Only = new List<T>();
            this.InSet2Only = new List<T>();
        }
    }
}
