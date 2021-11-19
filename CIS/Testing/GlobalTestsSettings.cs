using System;

namespace CIS.Testing
{
    public static class GlobalTestsSettings
    {
        // nazev vychoziho souboru pro seed databaze
        internal static string DatabaseSeedScriptName = "DatabaseSeed.sql";

        internal static string BaseNamespace = "";

        internal static string SolutionRelativeContentRoot = "";

        internal static string TestsFolderName = "Tests";

        internal static string TestsClassNameSuffix = "Tests";

        internal static bool IsInitialized = false;
        
        public static void Init(string solutionRelativeContentRoot, Action<GlobalTestsSettingsOptions> options)
        {
            SolutionRelativeContentRoot = solutionRelativeContentRoot;

            var opt = new GlobalTestsSettingsOptions();
            options.Invoke(opt);
            
            IsInitialized = true;
        }

        public sealed class GlobalTestsSettingsOptions
        {
            public void SetBaseNamespace(string baseNamespace)
            {
                BaseNamespace = baseNamespace;
            }

            public void SetTestsFolderName(string folderName)
            {
                TestsFolderName = folderName;
            }

            public void SetTestsClassNameSuffix(string suffix)
            {
                TestsClassNameSuffix = suffix;
            }
        }
    }
}
