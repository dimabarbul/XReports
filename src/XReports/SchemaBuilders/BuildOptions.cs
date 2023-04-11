namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Build options that can be used by post-builders to change report schema.
    /// </summary>
    public class BuildOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether report schema should be vertical or horizontal.
        /// </summary>
        public bool IsVertical { get; set; }

        /// <summary>
        /// Gets or sets count of header rows. Is used only for horizontal report schema, for vertical it is ignored.
        /// </summary>
        public int HeaderRowsCount { get; set; }
    }
}
