using System;
using System.Text;


/// <summary>
/// There should be only one Extensions namespace for each combination of repository, domain, and solution.
/// 
/// The reason to put extensions into their own namespace in the first place is to avoid overwhelming users of your library with bucketfuls of extensions in their autocomplete dropdown. This is colloquially known as "crapping-up the autocomplete".
/// But, how granular should the extensions namespace be? For example, should string extensions be in their own Public.Common.Lib.Extensions.System.String namespace?
/// No. If extension namespaces are too granular, a user of your library has to spend all their time importing extension namespaces by typing out multitudes of using declarations to get all the extensions they want.
/// Note that for a type to be used, it's namespace has to be imported. Thus extensions on a type will not appear unless that type has appeared.
/// This means that while a vast number of extension methods can be included in a namespace, they will only apply to types that have appeared.
/// 
/// The exception here is the System namespace. Generally, fundamental classes will have more extensions because they are used more often. Thus if extensions for both System.String and System.DateTime are in the same Common.Extensions namespace, even if you want to use only the String extensions, DateTime will also be swamped with the DateTime extensions.
/// The saving grace here is the extension methods are defined in their own assemblies and namespaces. For example, both Common.Lib and Excel.Lib might define extensions on String. Thus only when both are referenced and imported will a String's autocomplete be chock-full of extensions (which is likely the desired behavior).
/// 
/// In conclusion, keep extensions located in the same assemblies and namespaces as the functionality they support, and keep the number of extensions in the Common.Lib to a minimum.
/// </summary>
namespace Public.Examples.Code.Extensions
{
    public static class StringExtensions
    {
        public static string[] TokenizeLine(this string line, char separator)
        {
            string[] tokens = line.Split(separator); // StringSplitOptions.None is used by default.
            return tokens;
        }
    }
}