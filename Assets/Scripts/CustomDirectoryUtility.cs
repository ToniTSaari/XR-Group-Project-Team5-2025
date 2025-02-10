using UnityEngine;

public static class CustomDirectoryUtility
{
    public static string GetRelativeProjectPath(string fullPath)
    {
        string projectPath = Application.dataPath;  // Get the absolute path of the "Assets" folder
        return fullPath.Replace(projectPath, "").Replace("\\", "/");  // Return the relative path
    }
}
