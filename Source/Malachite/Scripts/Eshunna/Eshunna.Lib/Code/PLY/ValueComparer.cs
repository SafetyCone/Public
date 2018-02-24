using System;
using System.Linq;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public abstract class ValueComparer
    {
        #region Static

        public static ValueComparer GetValueComparer(PlyPropertyDescriptor propertyDescriptor, ILog log)
        {
            var output = ValueComparer.GetValueComparer(propertyDescriptor.DataType, propertyDescriptor.IsList, log);
            return output;
        }

        public static ValueComparer GetValueComparer(PlyDataType plyDataType, bool isList, ILog log)
        {
            ValueComparer output;
            switch (plyDataType)
            {
                case PlyDataType.Character:
                    output = new ValueComparer<sbyte>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = new ValueComparer<byte>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.Double:
                    output = new ValueComparer<double>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.Float:
                    output = new ValueComparer<float>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.Integer:
                    output = new ValueComparer<int>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = new ValueComparer<uint>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.Short:
                    output = new ValueComparer<short>((x, y) => x == y, isList, log);
                    break;

                case PlyDataType.ShortUnsigned:
                    output = new ValueComparer<ushort>((x, y) => x == y, isList, log);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(plyDataType);
            }
            return output;
        }

        #endregion


        public abstract bool ValuesEqual(object xValues, object yValues);
    }


    public class ValueComparer<T> : ValueComparer
    {
        private Func<T, T, bool> Comparer;
        private bool IsList;
        private ILog Log;


        public ValueComparer(Func<T, T, bool> comparer, bool isList, ILog log)
        {
            this.Comparer = comparer;
            this.IsList = isList;
            this.Log = log;
        }

        public override bool ValuesEqual(object xValues, object yValues)
        {
            bool output = true;

            if (this.IsList)
            {
                if (!(xValues is T[][] xValuesArrayAsT))
                {
                    throw new ArgumentException($@"Invalid type. Expected: {typeof(T[][]).FullName}, found: {xValues.GetType().FullName}", nameof(xValues));
                }

                if (!(yValues is T[][] yValuesArrayAsT))
                {
                    throw new ArgumentException($@"Invalid type. Expected: {typeof(T[][]).FullName}, found: {yValues.GetType().FullName}", nameof(yValues));
                }

                bool valuesEqual = this.Equals(xValuesArrayAsT, yValuesArrayAsT);
                if(!valuesEqual)
                {
                    output = false;

                    string message = $@"Values array not equal. Type: {typeof(T[][]).FullName}";
                    this.Log.WriteLine(message);
                }
            }
            else
            {
                if (!(xValues is T[] xValuesArrayAsT))
                {
                    throw new ArgumentException($@"Invalid type. Expected: {typeof(T[]).FullName}, found: {xValues.GetType().FullName}", nameof(xValues));
                }

                if (!(yValues is T[] yValuesArrayAsT))
                {
                    throw new ArgumentException($@"Invalid type. Expected: {typeof(T[]).FullName}, found: {yValues.GetType().FullName}", nameof(yValues));
                }

                bool valuesEqual = this.Equals(xValuesArrayAsT, yValuesArrayAsT);
                if (!valuesEqual)
                {
                    output = false;

                    string message = $@"Values array not equal. Type: {typeof(T[]).FullName}";
                    this.Log.WriteLine(message);
                }
            }

            return output;
        }

        private bool Equals(T[][] x, T[][] y)
        {
            bool output = true;

            int xCount = x.Length;
            int yCount = y.Length;
            bool countEquals = xCount == yCount;
            if (!countEquals)
            {
                output = false;

                string message = $@"Value count mismatch: x: {xCount.ToString()}, y: {yCount.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iValue = 0; iValue < xCount; iValue++)
                {
                    T[] valueX = x[iValue];
                    T[] valueY = y[iValue];

                    bool valuesEqual = this.Equals(valueX, valueY);
                    if (!valuesEqual)
                    {
                        output = false;

                        string message = $@"Values not equal: index: {iValue.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        private bool Equals(T[] x, T[] y)
        {
            bool output = true;

            int xCount = x.Length;
            int yCount = y.Length;
            bool countEquals = xCount == yCount;
            if(!countEquals)
            {
                output = false;

                string message = $@"Value count mismatch: x: {xCount.ToString()}, y: {yCount.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iValue = 0; iValue < xCount; iValue++)
                {
                    T valueX = x[iValue];
                    T valueY = y[iValue];

                    bool valuesEqual = this.Comparer(valueX, valueY);
                    if(!valuesEqual)
                    {
                        output = false;

                        string message = $@"Values not equal: index: {iValue.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }
    }
}
