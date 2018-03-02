using System;
using System.Collections.Generic;

using Public.Common.Lib;


namespace Eshunna.Lib.PLY
{
    // http://paulbourke.net/dataformats/ply/
    public class PlyFile
    {
        #region Constants

        public const string charTypeName = @"char"; // 1 byte.
        public const string ucharTypeName = @"uchar"; // 1 byte.
        public const string shortTypeName = @"short"; // 2 bytes.
        public const string uShortTypeName = @"ushort"; // 2 bytes.
        public const string intTypeName = @"int"; // 4 bytes.
        public const string uintTypeName = @"uint"; // 4 bytes.
        public const string floatTypeName = @"float"; // 4 bytes.
        public const string doubleTypeName = @"double"; // 8 bytes.

        public const string PlyFileMarker = @"ply";
        public const string PlyFileHeaderBegin = PlyFile.PlyFileMarker;
        public const string PlyFileHeaderEnd = @"end_header";

        public const string asciiFormatMarker = @"ascii";
        public const string littleEndianFormatMarker = @"binary_little_endian";
        public const string bigEndianFormatMarker = @"binary_big_endian";

        public const string formatKeyword = @"format";
        public const string commentKeyword = @"comment";
        public const string elementKeyword = @"element";
        public const string propertyKeyword = @"property";
        public const string listKeyword = @"list";

        public const string vertexElementName = @"vertex";
        public const string faceElementName = @"face";

        #endregion

        #region Static

        public static Version GetDefaultVersion()
        {
            var version = new Version(1, 0);
            return version;
        }

        /// <summary>
        /// Allocates, but does not fill, space for PLY file data.
        /// </summary>
        public static PlyFile Build(PlyFileHeader header)
        {
            PlyFile output = new PlyFile(header);

            foreach (var elementDescriptor in header.Elements)
            {
                var element = new Dictionary<string, object>();
                output.Values.Add(elementDescriptor.Name, element);

                foreach (var propertyDescriptor in elementDescriptor.PropertyDescriptors)
                {
                    object valuesArray = PlyFile.GetValuesArray(propertyDescriptor, elementDescriptor.Count);
                    element.Add(propertyDescriptor.Name, valuesArray);
                }
            }

            return output;
        }

        private static object GetValuesArray(PlyPropertyDescriptor propertyDescriptor, int elementCount)
        {
            object output;
            if (propertyDescriptor.IsList)
            {
                output = PlyFile.GetListValuesArray(propertyDescriptor.DataType, elementCount);
            }
            else
            {
                output = PlyFile.GetValuesArray(propertyDescriptor.DataType, elementCount);
            }

            return output;
        }

        private static object GetListValuesArray(PlyDataType plyDataType, int elementCount)
        {
            object output;
            switch (plyDataType)
            {
                case PlyDataType.Character:
                    output = new sbyte[elementCount][];
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = new byte[elementCount][];
                    break;

                case PlyDataType.Double:
                    output = new double[elementCount][];
                    break;

                case PlyDataType.Float:
                    output = new float[elementCount][];
                    break;

                case PlyDataType.Integer:
                    output = new int[elementCount][];
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = new uint[elementCount][];
                    break;

                case PlyDataType.Short:
                    output = new short[elementCount][];
                    break;

                case PlyDataType.ShortUnsigned:
                    output = new ushort[elementCount][];
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(plyDataType);
            }
            return output;
        }

        private static object GetValuesArray(PlyDataType plyDataType, int count)
        {
            object output;
            switch (plyDataType)
            {
                case PlyDataType.Character:
                    output = new sbyte[count];
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = new byte[count];
                    break;

                case PlyDataType.Double:
                    output = new double[count];
                    break;

                case PlyDataType.Float:
                    output = new float[count];
                    break;

                case PlyDataType.Integer:
                    output = new int[count];
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = new uint[count];
                    break;

                case PlyDataType.Short:
                    output = new short[count];
                    break;

                case PlyDataType.ShortUnsigned:
                    output = new ushort[count];
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(plyDataType);
            }
            return output;
        }

        public static ValueConverter[][] BuildValueConverters(PlyFile plyFile)
        {
            PlyFileHeader header = plyFile.Header;

            int nElements = header.Elements.Count;
            ValueConverter[][] output = new ValueConverter[nElements][];
            for (int iElement = 0; iElement < nElements; iElement++)
            {
                var elementDescriptor = header.Elements[iElement];
                var element = plyFile.Values[elementDescriptor.Name];

                int nProperties = elementDescriptor.PropertyDescriptors.Count;
                ValueConverter[] valueConverters = new ValueConverter[nProperties];
                output[iElement] = valueConverters;
                for (int iProperty = 0; iProperty < nProperties; iProperty++)
                {
                    var propertyDescriptor = elementDescriptor.PropertyDescriptors[iProperty];
                    var valuesArray = element[propertyDescriptor.Name];

                    ValueConverter valueConverter = ValueConverter.GetValueConverter(propertyDescriptor, valuesArray);
                    valueConverters[iProperty] = valueConverter;
                }
            }

            return output;
        }

        #endregion


        public PlyFileHeader Header { get; }
        public Dictionary<string, Dictionary<string, object>> Values { get; }


        public PlyFile(PlyFileHeader header)
        {
            this.Values = new Dictionary<string, Dictionary<string, object>>();

            this.Header = header;
        }
    }
}
