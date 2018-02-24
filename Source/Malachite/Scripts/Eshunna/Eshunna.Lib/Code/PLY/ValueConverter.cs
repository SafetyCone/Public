using System;
using System.IO;
using System.Text;

using Public.Common.Lib;
using Public.Common.Lib.Extensions;


namespace Eshunna.Lib.PLY
{
    public abstract class ValueConverter
    {
        #region Static

        public static Func<byte[], int> GetListSizeConversionFunction(PlyDataType listSizePlyDataType)
        {
            Func<byte[], int> output;
            switch (listSizePlyDataType)
            {
                case PlyDataType.None:
                    output = (x) => -1; // Should never be called.
                    break;

                case PlyDataType.Character:
                    output = (x) => Convert.ToInt32(Convert.ToSByte(x[0]));
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = (x) => Convert.ToInt32(x[0]);
                    break;

                case PlyDataType.Double:
                    output = (x) => Convert.ToInt32(BitConverter.ToDouble(x, 0));
                    break;

                case PlyDataType.Float:
                    output = (x) => Convert.ToInt32(BitConverter.ToSingle(x, 0));
                    break;

                case PlyDataType.Integer:
                    output = (x) => Convert.ToInt32(BitConverter.ToInt32(x, 0));
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = (x) => Convert.ToInt32(BitConverter.ToUInt32(x, 0));
                    break;

                case PlyDataType.Short:
                    output = (x) => Convert.ToInt32(BitConverter.ToInt16(x, 0));
                    break;

                case PlyDataType.ShortUnsigned:
                    output = (x) => Convert.ToInt32(BitConverter.ToUInt16(x, 0));
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(listSizePlyDataType);
            }
            return output;
        }

        public static Func<int, byte[]> GetListSizeToBytesConversionFunction(PlyDataType listSizePlyDataType)
        {
            Func<int, byte[]> output;
            switch (listSizePlyDataType)
            {
                case PlyDataType.None:
                    output = (x) => new byte[0]; // Should never be called.
                    break;

                case PlyDataType.Character:
                    output = (x) => new byte[] { Convert.ToByte(Convert.ToSByte(x)) };
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = (x) => new byte[] { Convert.ToByte(x) };
                    break;

                case PlyDataType.Double:
                    output = (x) => BitConverter.GetBytes(Convert.ToDouble(x));
                    break;

                case PlyDataType.Float:
                    output = (x) => BitConverter.GetBytes(Convert.ToSingle(x));
                    break;

                case PlyDataType.Integer:
                    output = (x) => BitConverter.GetBytes(Convert.ToInt32(x));
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = (x) => BitConverter.GetBytes(Convert.ToUInt32(x));
                    break;

                case PlyDataType.Short:
                    output = (x) => BitConverter.GetBytes(Convert.ToInt16(x));
                    break;

                case PlyDataType.ShortUnsigned:
                    output = (x) => BitConverter.GetBytes(Convert.ToUInt16(x));
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(listSizePlyDataType);
            }
            return output;
        }

        public static int SizeInBytes(PlyDataType plyDataType)
        {
            int output;
            switch (plyDataType)
            {
                case PlyDataType.None:
                    output = 0;
                    break;

                case PlyDataType.Character:
                    output = 1;
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = 1;
                    break;

                case PlyDataType.Double:
                    output = 8;
                    break;

                case PlyDataType.Float:
                    output = 4;
                    break;

                case PlyDataType.Integer:
                    output = 4;
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = 4;
                    break;

                case PlyDataType.Short:
                    output = 2;
                    break;

                case PlyDataType.ShortUnsigned:
                    output = 2;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(plyDataType);
            }
            return output;
        }

        public static ValueConverter GetValueConverter(PlyDataType plyDataType, object valuesArray, bool isList, PlyDataType listLengthDataType = PlyDataType.None)
        {
            ValueConverter output;
            int sizeInBytes = ValueConverter.SizeInBytes(plyDataType);
            switch (plyDataType)
            {
                case PlyDataType.Character:
                    output = new ValueConverter<sbyte>(valuesArray, isList, listLengthDataType, Convert.ToSByte, Convert.ToString, sizeInBytes, (x) => Convert.ToSByte(x[0]), (x) => new byte[] { Convert.ToByte(x) });
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = new ValueConverter<byte>(valuesArray, isList, listLengthDataType, Convert.ToByte, Convert.ToString, sizeInBytes, (x) => x[0], (x) => new byte[] { x });
                    break;

                case PlyDataType.Double:
                    output = new ValueConverter<double>(valuesArray, isList, listLengthDataType, Convert.ToDouble, Convert.ToString, sizeInBytes, (x) => BitConverter.ToDouble(x, 0), (x) => BitConverter.GetBytes(x));
                    break;

                case PlyDataType.Float:
                    output = new ValueConverter<float>(valuesArray, isList, listLengthDataType, Convert.ToSingle, Convert.ToString, sizeInBytes, (x) => BitConverter.ToSingle(x, 0), (x) => BitConverter.GetBytes(x));
                    break;

                case PlyDataType.Integer:
                    output = new ValueConverter<int>(valuesArray, isList, listLengthDataType, Convert.ToInt32, Convert.ToString, sizeInBytes, (x) => BitConverter.ToInt32(x, 0), (x) => BitConverter.GetBytes(x));
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = new ValueConverter<uint>(valuesArray, isList, listLengthDataType, Convert.ToUInt32, Convert.ToString, sizeInBytes, (x) => BitConverter.ToUInt32(x, 0), (x) => BitConverter.GetBytes(x));
                    break;

                case PlyDataType.Short:
                    output = new ValueConverter<short>(valuesArray, isList, listLengthDataType, Convert.ToInt16, Convert.ToString, sizeInBytes, (x) => BitConverter.ToInt16(x, 0), (x) => BitConverter.GetBytes(x));
                    break;

                case PlyDataType.ShortUnsigned:
                    output = new ValueConverter<ushort>(valuesArray, isList, listLengthDataType, Convert.ToUInt16, Convert.ToString, sizeInBytes, (x) => BitConverter.ToUInt16(x, 0), (x) => BitConverter.GetBytes(x));
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(plyDataType);
            }
            return output;
        }

        public static ValueConverter GetValueConverter(PlyPropertyDescriptor plyPropertyDescriptor, object valuesArray)
        {
            var output = ValueConverter.GetValueConverter(plyPropertyDescriptor.DataType, valuesArray, plyPropertyDescriptor.IsList, plyPropertyDescriptor.ListLengthValueDataType);
            return output;
        }

        #endregion


        public abstract void ConvertAndAdd(FileStream fileStream, int index);
        public abstract void ConvertAndAdd(string token, int index);
        public abstract void ConvertAndAdd(string[] tokens, int index);
        public abstract string ToString(int index);
        public abstract byte[] ToBytes(int index);
        public abstract int GetListLength(int index);
    }


    public class ValueConverter<T> : ValueConverter
    {
        private bool IsList;
        private PlyDataType ListDataType;
        int ListSizeByteArrayLength;
        byte[] ListSizeBytes;
        Func<byte[], int> ListSizeConverter;
        private Func<int, byte[]> ListSizeToBytesConverter;
        private T[] ValuesArray;
        private T[][] ValuesArrayList;
        private Func<string, T> FromStringConverter;
        private Func<T, string> ToStringConverter;
        private int ByteArrayLength;
        private byte[] Bytes;
        private Func<byte[], T> FromBytesConverter;
        private Func<T, byte[]> ToBytesConverter;


        public ValueConverter(object valuesArray, bool isList, PlyDataType listSizeDataType, Func<string, T> fromStringConverter, Func<T, string> toStringConverter, int byteArrayLength, Func<byte[], T> fromBytesConverter, Func<T, byte[]> toBytesConverter)
        {
            this.IsList = isList;
            this.ListDataType = listSizeDataType;
            this.ListSizeByteArrayLength = ValueConverter.SizeInBytes(listSizeDataType);
            this.ListSizeBytes = new byte[this.ListSizeByteArrayLength];
            this.ListSizeConverter = ValueConverter.GetListSizeConversionFunction(listSizeDataType);
            this.ListSizeToBytesConverter = ValueConverter.GetListSizeToBytesConversionFunction(listSizeDataType);
            this.FromStringConverter = fromStringConverter;
            this.ToStringConverter = toStringConverter;
            this.ByteArrayLength = byteArrayLength;
            this.Bytes = new byte[this.ByteArrayLength];
            this.FromBytesConverter = fromBytesConverter;
            this.ToBytesConverter = toBytesConverter;

            if (this.IsList)
            {
                if (valuesArray is T[][] valuesArrayAsT)
                {
                    this.ValuesArrayList = valuesArrayAsT;
                }
                else
                {
                    throw new ArgumentException($@"Invalid type. Expected: {typeof(T[][]).FullName}, found: {valuesArray.GetType().FullName}", nameof(valuesArray));
                }
            }
            else
            {
                if (valuesArray is T[] valuesArrayAsT)
                {
                    this.ValuesArray = valuesArrayAsT;
                }
                else
                {
                    throw new ArgumentException($@"Invalid type. Expected: {typeof(T[]).FullName}, found: {valuesArray.GetType().FullName}", nameof(valuesArray));
                }
            }
        }

        public override void ConvertAndAdd(FileStream fileStream, int index)
        {
            if(this.IsList)
            {
                fileStream.Read(this.ListSizeBytes, 0, this.ListSizeByteArrayLength);
                int listSize = this.ListSizeConverter(this.ListSizeBytes);

                T[] values = new T[listSize];
                for (int iValue = 0; iValue < listSize; iValue++)
                {
                    fileStream.Read(this.Bytes, 0, this.ByteArrayLength);

                    T value = this.FromBytesConverter(this.Bytes);
                    values[iValue] = value;
                }

                this.ValuesArrayList[index] = values;
            }
            else
            {
                fileStream.Read(this.Bytes, 0, this.ByteArrayLength);

                T value = this.FromBytesConverter(this.Bytes);
                this.ValuesArray[index] = value;
            }
        }

        public override void ConvertAndAdd(string token, int index)
        {
            if(this.IsList)
            {
                throw new InvalidOperationException(@"Cannot add a single values to a list.");
            }

            T value = this.FromStringConverter(token);
            this.ValuesArray[index] = value;
        }

        public override void ConvertAndAdd(string[] tokens, int index)
        {
            if (!this.IsList)
            {
                throw new InvalidOperationException(@"Cannot add a list of values to scalar.");
            }

            int nTokens = tokens.Length;
            T[] values = new T[nTokens];
            for (int iToken = 0; iToken < nTokens; iToken++)
            {
                string token = tokens[iToken];
                T value = this.FromStringConverter(token);
                values[iToken] = value;
            }

            this.ValuesArrayList[index] = values;
        }

        /// <remarks>
        /// Using System.Convert.ToString(), not any fancy formatting.
        /// Ignoring cases:
        /// * 4.68083e-006 (expected), 4.68083E-06 (actual).
        /// </remarks>
        public override string ToString(int index)
        {
            string output;
            if(this.IsList)
            {
                T[] values = this.ValuesArrayList[index];
                StringBuilder builder = new StringBuilder();
                foreach(T value in values)
                {
                    string appendix = $@"{this.ToStringConverter(value)} ";
                    builder.Append(appendix);
                }
                builder.RemoveLast();

                output = builder.ToString();
            }
            else
            {
                T value = this.ValuesArray[index];
                output = this.ToStringConverter(value);
            }
            return output;
        }

        public override byte[] ToBytes(int index)
        {
            byte[] output;
            if(this.IsList)
            {
                T[] values = this.ValuesArrayList[index];
                int listSize = values.Length;
                byte[][] bytes = new byte[listSize + 1][];

                byte[] listSizeBytes = this.ListSizeToBytesConverter(listSize);
                bytes[0] = listSizeBytes;
                for (int iValue = 0; iValue < listSize; iValue++)
                {
                    T value = values[iValue];
                    byte[] valueBytes = this.ToBytesConverter(value);
                    bytes[iValue + 1] = valueBytes;
                }

                output = bytes.Flatten();
            }
            else
            {
                T value = this.ValuesArray[index];
                output = this.ToBytesConverter(value);
            }

            return output;
        }

        public override int GetListLength(int index)
        {
            if(!this.IsList)
            {
                throw new InvalidOperationException(@"Cannot get list length from a non-list property.");
            }

            int output = this.ValuesArrayList[index].Length;
            return output;
        }
    }
}
