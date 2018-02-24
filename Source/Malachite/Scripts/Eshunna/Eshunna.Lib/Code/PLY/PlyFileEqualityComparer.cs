using System;
using System.Collections.Generic;
using System.Linq;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public class PlyFileEqualityComparer : IEqualityComparer<PlyFile>
    {
        #region Static

        private static bool Equals(PlyFileHeader header, Dictionary<string, Dictionary<string, object>> xElements, Dictionary<string, Dictionary<string, object>> yElements, ILog log)
        {
            bool output = true;

            var elementNamesX = new List<string>(xElements.Keys);
            var elementNamesY = new List<string>(yElements.Keys);

            int nElementsX = elementNamesX.Count;
            int nElementsY = elementNamesY.Count;
            bool elementCountEquals = nElementsX == nElementsY;
            if (!elementCountEquals)
            {
                output = false;

                string message = $@"Element count mismatch: x: {nElementsX.ToString()}, y: {nElementsY.ToString()}";
                log.WriteLine(message);
            }
            else
            {
                // The header equality test already tested the order of the elements, here we just test that we do in fact have all values.
                elementNamesX.Sort();
                elementNamesY.Sort();

                bool elementsEqual = elementNamesX.SequenceEqual(elementNamesY);
                if (!elementsEqual)
                {
                    output = false;

                    string message = @"Element sequence mismatch.";
                    log.WriteLine(message);
                }
                else
                {
                    foreach (var elementName in elementNamesX)
                    {
                        var propertyValuesX = xElements[elementName];
                        var propertyValuesY = yElements[elementName];

                        PlyElementDescriptor elementDescriptor = header.Elements.Where((x) => x.Name == elementName).FirstOrDefault();
                        bool propertiesEqual = PlyFileEqualityComparer.Equals(elementDescriptor, propertyValuesX, propertyValuesY, log);
                        if (!propertiesEqual)
                        {
                            output = false;

                            string message = $@"Property values mismatch: element: {elementName}";
                            log.WriteLine(message);
                        }
                    }
                }
            }

            return output;
        }

        private static bool Equals(PlyElementDescriptor elementDescriptor, Dictionary<string, object> xProperties, Dictionary<string, object> yProperties, ILog log)
        {
            bool output = true;

            var propertyNamesX = new List<string>(xProperties.Keys);
            var propertyNamesY = new List<string>(yProperties.Keys);

            int nPropertiesX = propertyNamesX.Count;
            int nPropertiesY = propertyNamesY.Count;
            bool propertyCountEquals = nPropertiesX == nPropertiesY;
            if (!propertyCountEquals)
            {
                output = false;

                string message = $@"Property count mismatch: x: {nPropertiesX.ToString()}, y: {nPropertiesY.ToString()}";
                log.WriteLine(message);
            }
            else
            {
                // The header equality comparison has already taken care of the order of properties, here we just test the values.
                propertyNamesX.Sort();
                propertyNamesY.Sort();

                bool propertyNamesEqual = propertyNamesX.SequenceEqual(propertyNamesY);
                if (!propertyNamesEqual)
                {
                    output = false;

                    string message = @"Property names mismatch.";
                    log.WriteLine(message);
                }
                else
                {
                    foreach (var propertyName in propertyNamesX)
                    {
                        object valuesArrayX = xProperties[propertyName];
                        object valuesArrayY = yProperties[propertyName];

                        PlyPropertyDescriptor propertyDescriptor = elementDescriptor.PropertyDescriptors.Where((x) => x.Name == propertyName).FirstOrDefault();

                        bool valuesEqual = PlyFileEqualityComparer.Equals(propertyDescriptor, valuesArrayX, valuesArrayY, log);
                        if (!valuesEqual)
                        {
                            output = false;

                            string message = $@"Values not equal: property: {propertyName}";
                            log.WriteLine(message);
                        }
                    }
                }
            }

            return output;
        }

        private static bool Equals(PlyPropertyDescriptor propertyDescriptor, object valuesArrayX, object valuesArrayY, ILog log)
        {
            bool output = true;

            ValueComparer comparer = ValueComparer.GetValueComparer(propertyDescriptor, log);
            bool valuesEqual = comparer.ValuesEqual(valuesArrayX, valuesArrayY);
            if (!valuesEqual)
            {
                output = false;

                string message = $@"Values not equal: property: {propertyDescriptor.Name}";
                log.WriteLine(message);
            }

            return output;
        }

        #endregion

        public PlyFileHeaderEqualityComparer HeaderComparer { get; }
        public ILog Log { get; }


        public PlyFileEqualityComparer(PlyFileHeaderEqualityComparer headerComparer, ILog log)
        {
            this.HeaderComparer = headerComparer;
            this.Log = log;
        }

        public PlyFileEqualityComparer(ILog log)
            : this(new PlyFileHeaderEqualityComparer(log), log)
        {
        }

        public bool Equals(PlyFile x, PlyFile y)
        {
            bool output = true;

            bool headersEqual = this.HeaderComparer.Equals(x.Header, y.Header);
            if(!headersEqual)
            {
                output = false;

                string message = @"Headers not equal.";
                this.Log.WriteLine(message);
            }

            bool valuesEqual = PlyFileEqualityComparer.Equals(x.Header, x.Values, y.Values, this.Log);
            if(!valuesEqual)
            {
                output = false;

                string message = @"Values not equal.";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(PlyFile obj)
        {
            throw new NotImplementedException();
        }
    }
}
