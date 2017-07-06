using System;


namespace Public.Common.Lib.Math
{
    public class Utilities
    {
        #region Static

        public static readonly Random SingletonRandom = new Random(Constants.DefaultRandomSeed);

        #endregion
    }
}
