using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// A configuration-platform pair (example: Debug-Any CPU).
    /// </summary>
    public class BuildConfiguration : IEquatable<BuildConfiguration>
    {
        public const char BuildConfigurationTokenSeparator = '|';


        #region Static

        public static bool operator ==(BuildConfiguration lhs, BuildConfiguration rhs)
        {
            if (object.ReferenceEquals(null, lhs))
            {
                if (object.ReferenceEquals(null, rhs))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs); // Equals() handles a null right side.
        }

        public static bool operator !=(BuildConfiguration lhs, BuildConfiguration rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region IEquatable<BuildConfiguration> Members

        public bool Equals(BuildConfiguration other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

#if DEBUG
            bool output = true;
            output = output && (this.Configuration == other.Configuration);
            output = output && (this.Platform == other.Platform);

            return output;
#else
            return
                (this.Configuration == other.Configuration) &&
                (this.Platform == other.Platform);
#endif
        }

        #endregion


        public Configuration Configuration { get; set; }
        public Platform Platform { get; set; }


        public BuildConfiguration()
        {
        }

        public BuildConfiguration(Configuration configuration, Platform platform)
        {
            this.Configuration = configuration;
            this.Platform = platform;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BuildConfiguration);
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.Configuration, this.Platform);
        }

        public override string ToString()
        {
            string output = String.Format(@"{0} {1} {2}", this.Configuration.ToDefaultString(), BuildConfiguration.BuildConfigurationTokenSeparator, this.Platform.ToDefaultString());
            return output;
        }
    }
}
