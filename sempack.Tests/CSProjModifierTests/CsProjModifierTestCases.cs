using System.Collections.Generic;
using System.Linq;

namespace sempack.Tests.CSProjModifierTests
{
    public class CsProjModifierTestCases
    {
        private static Dictionary<string, CsProjModifierTest> _cases;

        public static List<object[]> GetModifyProjectFileTestCaseTitles()
        {
            if (_cases is null)
            {
                BuildTestCaseDictionary();
            }

            return _cases.Select(x => new object[] {x.Key}).ToList();
        }

        public static CsProjModifierTest GetModifyProjectFilesTestCase(string key)
        {
            if (_cases is null)
            {
                BuildTestCaseDictionary();
            }

            return _cases[key];
        }

        private static void BuildTestCaseDictionary()
        {
            _cases = new Dictionary<string, CsProjModifierTest>();
            _cases.Add("No Version or Version Prefix Successful Result", BuildNoVersionTestCase());
        }

        private static CsProjModifierTest BuildNoVersionTestCase()
        {
            return new CsProjModifierTest()
            {
                DeleteVersion = true,
                DeleteVersionPrefix = true,
                ExpectedMajorVersion = "1",
                ExpectedMinorVersion = "0",
                Result = true
            };
        }
    }
}