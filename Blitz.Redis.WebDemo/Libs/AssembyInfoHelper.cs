namespace Blitz.Redis.WebDemo.Libs
{
    /// <summary>
    /// Helper: Assembly Info
    /// <para>Translates the <c>CustomAttributeData</c> to a usable string value</para>
    /// </summary>
    public static class AssembyInfoHelper
    {
        /// <summary>
        /// Try Parse a <c>System.Reflection.CustomAttributeData</c> into a string
        /// </summary>
        /// <param name="attribute">(this)</param>
        /// <param name="s">Strng to parse into</param>
        /// <returns>True if success</returns>
        public static bool TryParse(this System.Reflection.CustomAttributeData attribute, out string s)
        {
            var flag = false;
            s = string.Empty;
            if (attribute == null) return flag;
            s = attribute.ToString();
            var i = s.IndexOf('"');
            if (i >= 0) { s = s.Substring(i + 1); flag = true; }
            i = s.IndexOf('"');
            if (i >= 0) { s = s.Substring(0, i); flag = true; }
            return flag;
        }
    }
}
