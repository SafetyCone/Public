using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Contains all information and methods needed to work with a generic color value type.
    /// </summary>
    /// <remarks>
    /// No restrictions on the generic type since they are not needed for the interface.
    /// </remarks>
    public interface IColorValueTypeInfo<T>
    {
        /// <summary>
        /// The maximum value for the color value type.
        /// </summary>
        T Max { get; }
        /// <summary>
        /// The minimum value for the color value type.
        /// </summary>
        T Min { get; }
        /// <summary>
        /// The range of the color value type (max minus min).
        /// </summary>
        T Range { get; }


        /// <summary>
        /// Subtraction operation for type T.
        /// </summary>
        T Subtract(T a, T b);

        /// <summary>
        /// Converts to a double, used for casting.
        /// </summary>
        double ToDouble(T value);

        /// <summary>
        /// Converts from a double, used for casting.
        /// </summary>
        T FromDouble(double value);
    }
}
