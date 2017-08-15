using System;
using System.Collections;
using System.Collections.Generic;


namespace Minex.Common.Lib.Visuals
{
    [Serializable]
    public class GenericCoordinatedImage<T> : GenericImage<T>, ICoordinatedImage<T>
        where T : ICoordinated
    {
        #region ICoordinatedImage<T> Members

        public T this[Coordinate coordinate]
        {
            get
            {
                T output = this.zPixels[coordinate.Row, coordinate.Column];
                return output;
            }
            set
            {
                this.zPixels[coordinate.Row, coordinate.Column] = value;
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                for (int iCol = 0; iCol < this.Columns; iCol++)
                {
                    yield return this[iRow, iCol];
                }
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion


        public GenericCoordinatedImage() : base() { }

        public GenericCoordinatedImage(int rows, int columns) : base(rows, columns) { }
    }
}
