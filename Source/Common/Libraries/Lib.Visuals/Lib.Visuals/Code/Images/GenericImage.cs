//using System;


//namespace Public.Common.Lib.Visuals
//{
//    [Serializable]
//    public class GenericImage<T>
//    {
//        public int Rows
//        {
//            get
//            {
//                int output = this.zPixels.GetLength(0);
//                return output;
//            }
//        }
//        public int Columns
//        {
//            get
//            {
//                int output = this.zPixels.GetLength(1);
//                return output;
//            }
//        }
//        protected T[,] zPixels;
//        public T this[int row, int column]
//        {
//            get
//            {
//                T output = this.zPixels[row, column];
//                return output;
//            }
//            set
//            {
//                this.zPixels[row, column] = value;
//            }
//        }


//        public GenericImage() { }

//        public GenericImage(int rows, int columns)
//        {
//            this.SetImageSize(rows, columns);
//        }

//        /// <remarks>
//        /// Setting the image size of an image will clear all image data.
//        /// </remarks>
//        public void SetImageSize(int rows, int columns)
//        {
//            this.zPixels = new T[rows, columns];
//        }
//    }
//}
