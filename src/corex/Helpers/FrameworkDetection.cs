using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Corex.Helpers
{

    public enum FrameworkVariant
    {
        Default,
        Client,
        Full
    }

    public class FrameworkVersion : IComparable<FrameworkVersion>
    {

        public Version Version { get; set; }
        public int ServicePack { get; set; }
        public FrameworkVariant Variant { get; set; }

        private static List<FrameworkVersion> _installedVersions;
        public static List<FrameworkVersion> InstalledVersions
        {
            get
            {
                if (_installedVersions == null) _installedVersions = GetInstalled();
                return _installedVersions;
            }
        }

        private static FrameworkVersion _Current;
        public static FrameworkVersion Current
        {
            get
            {
                if (_Current == null && InstalledVersions.Count != 0) _Current = InstalledVersions[InstalledVersions.Count - 1];
                return _Current;
            }
        }

        // http://msdn.microsoft.com/en-us/kb/kbarticle.aspx?id=318785
        private static string registryBasePath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\";

        private static List<FrameworkVersion> GetInstalled()
        {
            var list = new List<FrameworkVersion>();
            list.Add(GetVersion("1.1.4322"));
            list.Add(GetVersion("2.0.50727"));
            list.Add(GetVersion("3.0"));
            list.Add(GetVersion("3.5"));
            list.Add(GetVersion("4", "4.0", FrameworkVariant.Client));
            list.Add(GetVersion("4", "4.0", FrameworkVariant.Full));
            list.Add(GetVersion("4", "4.5", FrameworkVariant.Client));
            list.Add(GetVersion("4", "4.5", FrameworkVariant.Full));
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null) list.RemoveAt(i);
            }
            list.Sort();
            return list;
        }

        private static bool GetInstalled(string path)
        {
            var value = Registry.GetValue(registryBasePath + path, "Install", 0);
            return value == null ? false : ((int)value == 0 ? false : true);
        }

        private static int GetSP(string path)
        {
            var value = Registry.GetValue(registryBasePath + path, "SP", 0);
            return value == null ? 0 : (int)value;
        }

        private static string GetExactVersion(string path)
        {
            var value = Registry.GetValue(registryBasePath + path, "Version", "");
            return value == null ? "" : (string)value;
        }

        public static FrameworkVersion GetVersion(string version, string exactVersion = "", FrameworkVariant variant = FrameworkVariant.Default)
        {
            var path = "v" + version;
            if (variant != FrameworkVariant.Default) path += "\\" + variant.ToString();
            if (!GetInstalled(path)) return null;

            if (exactVersion != "")
            {
                var exactVer = new Version(exactVersion);
                var regVer = new Version(GetExactVersion(path));
                if (exactVer.Major == regVer.Major && exactVer.Minor == regVer.Minor)
                    version = new Version(exactVersion).ToString(2);
                else
                    return null;
            }

            var sp = GetSP(path);
            var ver = new Version(version);
            ver = new Version(ver.ToString(2));
            return new FrameworkVersion(ver, variant) { ServicePack = sp };
        }

        public FrameworkVersion(Version version, FrameworkVariant variant = FrameworkVariant.Default)
        {
            this.Version = version;
            this.Variant = variant;
        }


        public int CompareTo(FrameworkVersion other)
        {
            if (Version != other.Version) return Version.CompareTo(other.Version);
            if (ServicePack != other.ServicePack) return ServicePack.CompareTo(other.ServicePack);
            if (Variant != other.Variant) return Variant.CompareTo(other.Variant);
            return 0;
        }

        public override string ToString()
        {
            var s = "v" + Version.ToString(2);
            if (ServicePack != 0) s += " Service Pack " + ServicePack.ToString();
            if (Variant != FrameworkVariant.Default) s += " " + Variant.ToString();
            return s;
        }

        public static bool HasVersionkOrBetter(FrameworkVersion other)
        {
            if (Current == null) return false;
            return Current.CompareTo(other) >= 0;
        }

    }

}
