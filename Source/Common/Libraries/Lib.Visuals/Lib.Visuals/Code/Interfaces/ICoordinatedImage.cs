using System.Collections.Generic;


namespace Minex.Common.Lib.Visuals
{
    public interface ICoordinatedImage<T> : IEnumerable<T>
    {
        T this[Coordinate coordinate] { get; set; }


        void SetImageSize(int rows, int columns);
    }
}
