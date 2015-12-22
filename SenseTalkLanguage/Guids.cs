// Guids.cs
// MUST match guids.h
using System;

namespace TestPlant.SenseTalkLanguage
{
    static class GuidList
    {
        public const string guidSenseTalkLanguagePkgString = "debf7ccd-88b5-4a54-b371-dce3bf4fc0e7";
        public const string guidSenseTalkLanguageCmdSetString = "7e2c3d1f-c08c-4960-86eb-950859e786f8";

        public static readonly Guid guidSenseTalkLanguageCmdSet = new Guid(guidSenseTalkLanguageCmdSetString);
    };
}