﻿using System.Collections.Generic;
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

            return _cases.Select(x => new object[] { x.Key }).ToList();
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
            _cases.Add("No Version or Version Prefix Should Succeed", BuildNoVersionTestCase());
            _cases.Add("With Version and No Version Prefix Should Succeed", BuildVersionTestCase());
            _cases.Add("Version Prefix not populated Should Succeed", BuildVersionPrefixTestCase());
            _cases.Add("Preset Version Prefix should increment Major Version", BuildIncrementMajorVersionPrefixTestCase());
            _cases.Add("Preset Version Prefix should increment Minor Version", BuildIncrementMinorVersionPrefixTestCase());
            _cases.Add("Preset Version Prefix should increment Build Version", BuildIncrementBuildVersionPrefixTestCase());
            _cases.Add("Preset Version Prefix should increment Revision Version", BuildIncrementRevisionVersionPrefixTestCase());
            _cases.Add("Preset Version Prefix should Increment All", BuildIncrementAllVersionPrefixTestCase());
            _cases.Add("Version should override Version Prefix", BuildValidateVersionOverrideVersionPrefixTestCase());
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

        private static CsProjModifierTest BuildVersionTestCase()
        {
            return new CsProjModifierTest()
            {
                DeleteVersionPrefix = true,
                ExpectedMajorVersion = "1",
                ExpectedMinorVersion = "0",
                Result = true
            };
        }

        private static CsProjModifierTest BuildVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                ExpectedMajorVersion = "1",
                ExpectedMinorVersion = "0",
                Result = true
            };
        }

        private static CsProjModifierTest BuildIncrementMajorVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                PresetPrefixVersion = "1.0.0.0",
                IncrementMajor = true,
                ExpectedMajorVersion = "2",
                ExpectedMinorVersion = "0",
                Result = true
            };
        }

        private static CsProjModifierTest BuildIncrementMinorVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                PresetPrefixVersion = "1.1.0.0",
                IncrementMinor = true,
                ExpectedMajorVersion = "1",
                ExpectedMinorVersion = "2",
                Result = true
            };
        }

        private static CsProjModifierTest BuildIncrementBuildVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                PresetPrefixVersion = "1.1.1.1",
                IncrementBuild = true,
                ExpectedMajorVersion = "1",
                ExpectedMinorVersion = "1",
                Result = true
            };
        }

        private static CsProjModifierTest BuildIncrementRevisionVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                PresetPrefixVersion = "1.1.1.1",
                IncrementRevision = true,
                ExpectedMajorVersion = "1",
                ExpectedMinorVersion = "1",
                Result = true
            };
        }

        private static CsProjModifierTest BuildIncrementAllVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                PresetPrefixVersion = "1.1.1.1",
                IncrementMajor = true,
                IncrementMinor = true,
                IncrementBuild = true,
                IncrementRevision = true,
                ExpectedMajorVersion = "2",
                ExpectedMinorVersion = "2",
                Result = true
            };
        }

        private static CsProjModifierTest BuildValidateVersionOverrideVersionPrefixTestCase()
        {
            return new CsProjModifierTest()
            {
                PresetPrefixVersion = "1.1.1.1",
                PresetVersion = "2.2.2.2",
                ExpectedMajorVersion = "2",
                ExpectedMinorVersion = "2",
                Result = true
            };
        }
    }
}