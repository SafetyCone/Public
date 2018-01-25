using System;


namespace Public.Common.Lib.Visuals
{
    public struct ImageID : IEquatable<ImageID>
    {
        #region Static

        public static bool operator ==(ImageID lhs, ImageID rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(ImageID lhs, ImageID rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion


        public string Title { get; set; }
        public string Location { get; set; }


        public ImageID(string title, string location)
        {
            this.Title = title;
            this.Location = location;
        }

        public override string ToString()
        {
            string output = $@"{this.Title}::{this.Location}";
            return output;
        }

        public bool Equals(ImageID other)
        {
            bool output =
                this.Location == other.Location &&
                this.Title == other.Title;
            return output;
        }

        public override bool Equals(object obj)
        {
            // Note that because structs are implicitly sealed (no descendent classes) instead of using 'as' we can use 'is'.
            bool output = false;
            if (obj is ImageID)
            {
                output = this.Equals((ImageID)obj);
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.Location, this.Title);
        }
    }
}
