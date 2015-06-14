// Guids.cs
// MUST match guids.h
using System;

namespace Company.VSPackage1
{
    static class GuidList
    {
        public const string guidVSPackage1PkgString = "bf3f36df-921c-4f43-9446-eaf315c44e3a";
        public const string guidVSPackage1CmdSetString = "100144f6-96d6-4ec8-af12-d75fc9921d33";
        public const string guidToolWindowPersistanceString = "f3b9ca5f-9524-4170-a850-dda0158580ce";

        public static readonly Guid guidVSPackage1CmdSet = new Guid(guidVSPackage1CmdSetString);
    };
}