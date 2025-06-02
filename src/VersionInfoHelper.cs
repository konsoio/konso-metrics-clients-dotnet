using System;
using System.Reflection;

namespace Konso.Clients.Metrics
{
    public static class VersionInfoHelper
    {
        public static string AppVersion()
        {
            Assembly? entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
            {
                return "0.0.0";
            }

            string location = entryAssembly.Location;

            // AssemblyVersion
            Version? assemblyVersion = entryAssembly.GetName().Version;

            if(assemblyVersion != null)
                return assemblyVersion.ToString();


            return "0.0.0";
        }
    }
}
