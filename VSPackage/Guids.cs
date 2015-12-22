// Guids.cs
// MUST match guids.h

using System;

namespace TestPlant.EggPlantVSPackage
{
    internal static class GuidList
    {
        public const string guidEggPlantVSPackagePkgString = "268fd30e-e4ba-44ac-849f-777fce3f9877";
        public const string guidEggPlantVSPackageCmdSetString = "af93a7a7-00e4-4e2f-aee5-3d411c11378b";
        public const string guidToolWindowPersistanceString = "d146ce8e-5a3f-40dd-acad-3abba1bb6639";

        public static readonly Guid guidEggPlantVSPackageCmdSet = new Guid(guidEggPlantVSPackageCmdSetString);
    };
}