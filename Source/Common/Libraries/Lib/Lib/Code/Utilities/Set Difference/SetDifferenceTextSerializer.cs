using System;
using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib
{
    public class SetDifferenceTextSerializer<T>
    {
        #region Static

        public static IEnumerable<string> Serialize(SetDifference<T> setDifference)
        {
            if (setDifference.NoDifference)
            {
                yield return String.Format(@"Sets {0} and {1} are identical.", setDifference.Set1Name, setDifference.Set2Name);
            }
            else
            {
                yield return String.Format(@"Items only in {0}:", setDifference.Set1Name);
                yield return String.Empty;

                foreach (T value in setDifference.InSet1Only)
                {
                    yield return value.ToString();
                }
                yield return String.Empty;

                yield return String.Format(@"Items only in {0}:", setDifference.Set2Name);
                yield return String.Empty;

                foreach (T value in setDifference.InSet2Only)
                {
                    yield return value.ToString();
                }
            }
        }

        public static void SerializeToRootedPath(SetDifference<T> setDifference, string fileRootedPath)
        {
            using (StreamWriter writer = new StreamWriter(fileRootedPath))
            {
                foreach (string line in SetDifferenceTextSerializer<T>.Serialize(setDifference))
                {
                    writer.WriteLine(line);
                }
            }
        }

        #endregion
    }
}
