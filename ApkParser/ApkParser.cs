using System;
using System.Collections.Generic;
using System.IO;

namespace ApkParser
{
    /// <summary>
    /// ApkParser, provides the required method to parse an apk file, and extract major information
    /// </summary>
    public class ApkParser
    {
        private const string UserPermission = "uses-permission:";
        private const string AppLabel = "application-label:";
        private const string AppIcon = "application-icon-";
        private const string SupportsScreens = "supports-screens: ";
        private static readonly string TempPath = Path.GetTempPath() + @"ApkParser\{0}\v.{1}\";

        /// <summary>
        /// Parses an apk file and returns its major information
        /// </summary>
        /// <param name="apkFilePath">path to the apk file</param>
        /// <returns><see cref="ApkInfo"/> major info of the apk</returns>
        public static ApkInfo Parse(string apkFilePath)
        {
            if (!File.Exists(apkFilePath))
                throw new FileNotFoundException(apkFilePath + " file not found!");
            string output = ConsoleExecutor.Execute(@"Extensions\aapt.exe", "dump",
                "badging", "\"" + apkFilePath + "\"");
            string[] apkOut = output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            ApkInfo apkInfo = new ApkInfo();
            SetPackageData(apkInfo, apkOut[0]);
            SetSdkData(apkInfo, apkOut[1], apkOut[2]);
            SetPermissionsAndLabel(apkInfo, apkOut);
            SetIcons(apkInfo, apkOut, apkFilePath);
            return apkInfo;
        }

        private static void SetPackageData(ApkInfo apkInfo, string packageData)
        {
            string[] parts = packageData.Split();
            if (parts.Length > 1)
            {
                string name = parts[1];
                apkInfo.PackageName = name.Split('\'')[1];
            }
            if (parts.Length > 2)
            {
                string versionCode = parts[2];
                apkInfo.VersionCode = Convert.ToInt64(versionCode.Split('\'')[1]);
            }
            if (parts.Length > 3)
            {
                string versionName = parts[3];
                apkInfo.VersionName = versionName.Split('\'')[1];
            }
        }

        private static void SetSdkData(ApkInfo apkInfo, string sdkVersion, string targetSdkVersion)
        {
            apkInfo.MinSdkVersion = Convert.ToInt32(sdkVersion.Split('\'')[1]);
            apkInfo.TargetSdkVersion = Convert.ToInt32(targetSdkVersion.Split('\'')[1]);
        }

        private static void SetPermissionsAndLabel(ApkInfo apkInfo, string[] apkOut)
        {
            for (var i = 3; i < apkOut.Length; i++)
            {
                var splited = apkOut[i].Split('\'');
                if (splited[0] == UserPermission)
                {
                    apkInfo.Permissions.Add(splited[1]);
                }
                if (splited[0] == AppLabel)
                {
                    apkInfo.Label = splited[1];
                }
                if (splited[0] == SupportsScreens)
                {
                    SetScreenSupport(apkInfo, splited);
                }
            }
        }

        private static void SetScreenSupport(ApkInfo apkInfo, string[] supportsScreens)
        {
            var screenSupport = new ScreenSupport();
            for (var i = 1; i < supportsScreens.Length; i += 2)
            {
                if (i == 1 && supportsScreens[i] == "small")
                    screenSupport.Small = true;
                if (i == 3 && supportsScreens[i] == "normal")
                    screenSupport.Normal = true;
                if (i == 5 && supportsScreens[i] == "large")
                    screenSupport.Large = true;
                if (i == 7 && supportsScreens[i] == "xlarge")
                    screenSupport.XLarge = true;
            }
            apkInfo.ScreenSupport = screenSupport;
        }

        private static void SetIcons(ApkInfo apkInfo, string[] apkOut, string apkFilePath)
        {
            var icons = new HashSet<string>();
            for (var i = 3; i < apkOut.Length; i++)
            {
                if (apkOut[i].StartsWith(AppIcon))
                {
                    var splited = apkOut[i].Split('\'');
                    icons.Add(splited[1]);
                }
            }
            var rootPath = ApkTempFilePath(apkInfo.PackageName, apkInfo.VersionName);
            foreach (var icon in icons)
            {
                var dir = Path.GetDirectoryName(Path.Combine(rootPath, icon));
                if (dir == null)
                    continue;
                Directory.CreateDirectory(dir);
                ConsoleExecutor.Execute(@"Extensions\unzip.exe", "-j", "-o", "\"" + apkFilePath + "\"",
                    icon, "-d", "\"" + dir + "\"");
                apkInfo.Icons.Add(new Uri(Path.Combine(rootPath, icon)));
            }
        }

        private static string ApkTempFilePath(string packageName, string version)
        {
            return string.Format(TempPath, packageName, version);
        }
    }
}