using System;
using System.Collections.Generic;

namespace ApkParser
{
    public class ApkInfo
    {
        public string PackageName { get; set; }
        public string Label { get; set; }
        public string VersionName { get; set; }
        public long VersionCode { get; set; }
        public int MinSdkVersion { get; set; }
        public int TargetSdkVersion { get; set; }
        public ScreenSupport ScreenSupport { get; set; }
        public IList<string> Permissions { get; set; }
        public IList<Uri> Icons { get; set; }

        public ApkInfo()
        {
            Permissions = new List<string>();
            Icons = new List<Uri>();
        }

        protected bool Equals(ApkInfo other)
        {
            return string.Equals(PackageName, other.PackageName) && string.Equals(Label, other.Label) &&
                   string.Equals(VersionName, other.VersionName) && VersionCode == other.VersionCode &&
                   MinSdkVersion == other.MinSdkVersion && TargetSdkVersion == other.TargetSdkVersion &&
                   ScreenSupport.Equals(other.ScreenSupport);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((ApkInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PackageName?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Label?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (VersionName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ VersionCode.GetHashCode();
                hashCode = (hashCode * 397) ^ MinSdkVersion;
                hashCode = (hashCode * 397) ^ TargetSdkVersion;
                hashCode = (hashCode * 397) ^ ScreenSupport.GetHashCode();
                return hashCode;
            }
        }
    }
}