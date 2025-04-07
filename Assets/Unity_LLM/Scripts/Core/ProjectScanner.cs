// ProjectScanner.cs
// 프로젝트 디렉토리 구조를 계층적으로 스캔하는 유틸리티

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProjectScanner
{
    public class FileEntry
    {
        public string name;
        public string path;
    }

    public class DirectoryEntry
    {
        public string name;
        public string path;
        public List<FileEntry> files = new();
        public List<DirectoryEntry> subdirectories = new();
    }

    public static DirectoryEntry Scan(string rootPath)
    {
        var root = new DirectoryEntry
        {
            name = Path.GetFileName(rootPath),
            path = rootPath
        };

        foreach (var file in Directory.GetFiles(rootPath))
        {
            root.files.Add(new FileEntry
            {
                name = Path.GetFileName(file),
                path = file
            });
        }

        foreach (var dir in Directory.GetDirectories(rootPath))
        {
            root.subdirectories.Add(Scan(dir));
        }

        return root;
    }

    public static DirectoryEntry ScanAssetsDirectory()
    {
        return Scan("Assets");
    }
}