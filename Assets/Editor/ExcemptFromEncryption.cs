#if UNITY_IOS
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using System.IO;
using UnityEditor.iOS.Xcode;


public class ExcemptFromEncryption : IPostprocessBuildWithReport // Will execute after XCode project is built
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.iOS) // Check if the build is for iOS 
        {
            string plistPath = report.summary.outputPath + "/Info.plist";

            PlistDocument plist = new PlistDocument(); // Read Info.plist file into memory
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            File.WriteAllText(plistPath, plist.WriteToString()); // Override Info.plist

            DisableBitcode(report.summary.outputPath); // Disable Bitcode
        }
    }

    void DisableBitcode(string basePath)
    {
        string projPath = basePath + "/Unity-iPhone.xcodeproj/project.pbxproj";

        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        string target = proj.GetUnityMainTargetGuid();
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        string target2 = proj.GetUnityFrameworkTargetGuid();
        proj.SetBuildProperty(target2, "ENABLE_BITCODE", "NO");
        File.WriteAllText(projPath, proj.WriteToString());
    }
}
#endif