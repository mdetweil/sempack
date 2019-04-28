namespace sempack.Tests.CSProjModifierTests
{
    public class CsProjModifierTest
    {
        public bool DeleteVersion { get; set; }
        public bool DeleteVersionPrefix { get; set; }
        public string PresetPrefixVersion { get; set; } = "";
        public bool IncrementMajor { get; set; }
        public bool IncrementMinor { get; set; }
        public bool IncrementBuild { get; set; }
        public bool IncrementRevision { get; set; }
        public bool Result { get; set; }
        public string ExpectedMajorVersion { get; set; }
        public string ExpectedMinorVersion { get; set; }
    }
}