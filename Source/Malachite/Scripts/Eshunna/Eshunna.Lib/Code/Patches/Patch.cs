using System;


namespace Eshunna.Lib.Patches
{
    public class Patch
    {
        public Location3HomogenousDouble Location { get; }
        public Vector4Double Normal { get; }
        /// <summary>
        /// Ranges from -1 to 1, where -1 is bad, and 1 is good.
        /// </summary>
        public double PhotometricConsistencyScore { get; }
        public double Debugging1 { get; }
        public double Debugging2 { get; }
        /// <summary>
        /// Based on texture agreement.
        /// </summary>
        public int[] ImageIndicesWithGoodAgreement { get; }
        /// <summary>
        /// Point should be visible in these images, but the textures do not agree well.
        /// </summary>
        public int[] ImageIndicesWithSomeAgreement { get; }


        public Patch(Location3HomogenousDouble location, Vector4Double normal, double photometricConsistencyScore, double debugging1, double debugging2, int[] imageIndicesWithGoodAgreement, int[] imageIndicesWithSomeAgreement)
        {
            this.Location = location;
            this.Normal = normal;
            this.PhotometricConsistencyScore = photometricConsistencyScore;
            this.Debugging1 = debugging1;
            this.Debugging2 = debugging2;
            this.ImageIndicesWithGoodAgreement = imageIndicesWithGoodAgreement;
            this.ImageIndicesWithSomeAgreement = imageIndicesWithSomeAgreement;
        }
    }
}
