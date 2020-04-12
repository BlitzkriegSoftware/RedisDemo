namespace Blitz.Redis.Demo.Models
{
    /// <summary>
    /// Custom Metadata For All Blitzkireg Software Micro-Service
    /// </summary>
    public class BlitzAssemblyVersionMetadata
    {
        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright { get; set; } = string.Empty;

        /// <summary>
        /// Company
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Semantic Version <para>See: semver.org</para>
        /// </summary>
        public string SemanticVersion { get; set; } = string.Empty;

        /// <summary>
        /// File Version
        /// </summary>
        public string FileVersion { get; set; } = string.Empty;

        /// <summary>
        /// Product
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Property Set
        /// </summary>
        /// <param name="name">(sic)</param>
        /// <param name="value">(sic)</param>
        public void PropertySet(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            if (string.IsNullOrWhiteSpace(value)) return;

            switch (name.ToLowerInvariant().Trim())
            {
                case "assemblycopyrightattribute": this.Copyright = value; break;
                case "assemblycompanyattribute": this.Company = value; break;
                case "assemblydescriptionattribute": this.Description = value; break;
                case "assemblyinformationalversionattribute": this.SemanticVersion = value; break;
                case "assemblyproductattribute": this.Product = value; break;
                case "assemblyfileversionattribute": this.FileVersion = value; break;
            }
        }

        /// <summary>
        /// Version String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Product} {this.Copyright} {this.SemanticVersion}\n{this.Description}\n";
        }

    }
}
