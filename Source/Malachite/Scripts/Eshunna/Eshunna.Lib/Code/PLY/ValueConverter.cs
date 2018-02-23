using System;
using System.Text;

using Public.Common.Lib.Extensions;


namespace Eshunna.Lib.PLY
{
    public abstract class ValueConverter
    {
        public abstract void ConvertAndAdd(string token, int index);
        public abstract void ConvertAndAdd(string[] tokens, int index);
        public abstract string ToString(int index);
        public abstract int GetListLength(int index);
    }


    public class ValueConverter<T> : ValueConverter
    {
        private bool IsList;
        private T[] ValuesArray;
        private T[][] ValuesArrayList;
        private Func<string, T> FromStringConverter;
        private Func<T, string> ToStringConverter;


        public ValueConverter(object valuesArray, bool isList, Func<string, T> fromStringConverter, Func<T, string> toStringConverter)
        {
            this.IsList = isList;
            this.FromStringConverter = fromStringConverter;
            this.ToStringConverter = toStringConverter;

            if(this.IsList)
            {
                if (valuesArray is T[][] valuesArrayAsT)
                {
                    this.ValuesArrayList = valuesArrayAsT;
                }
                else
                {
                    throw new ArgumentException(@"Invalid type.", nameof(valuesArray));
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
                    throw new ArgumentException(@"Invalid type.", nameof(valuesArray));
                }
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
