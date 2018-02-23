using System;


namespace Eshunna.Lib.Verification
{
    public interface IFileComparer
    {
        bool Equals(string filePath1, string filePath2);
    }
}
