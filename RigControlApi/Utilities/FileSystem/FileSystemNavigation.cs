using RigControlApi.Models;

namespace RigControlApi.Utilities.FileSystem;

public abstract class FileSystemNavigation
{
    public static IEnumerable<StorageItemDto> GetBaseItems()
    {
        var drives = DriveInfo.GetDrives();
        return drives.Select(drive => new StorageItemDto(name: drive.Name, type: StorageItemType.Directory,
                size: drive.TotalSize, path: drive.Name, lastModified: drive.RootDirectory.LastWriteTime,
                created: drive.RootDirectory.CreationTime, lastAccessed: drive.RootDirectory.LastAccessTime))
            .ToList();
    }

    public static IEnumerable<StorageItemDto> GetFolders(string path)
    {
        var directories = Directory.GetDirectories(path);
        var items = directories.Select(directory => new DirectoryInfo(directory)).Select(directoryInfo =>
            new StorageItemDto(name: directoryInfo.Name, type: StorageItemType.Directory,
                size: CalculateTotalFolderSize(directoryInfo.FullName),
                path: directoryInfo.FullName, lastModified: directoryInfo.LastWriteTime,
                created: directoryInfo.CreationTime, lastAccessed: directoryInfo.LastAccessTime));
        return items;
    }

    private static long CalculateTotalFolderSize(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        var totalSize = 0L;
        try
        {
            totalSize = directoryInfo.EnumerateFiles().Sum(file => file.Length);
        }
        catch (UnauthorizedAccessException)
        {
            return totalSize;
        }

        var subFolders = directoryInfo.EnumerateDirectories();
        totalSize += subFolders.Sum(subFolder => CalculateTotalFolderSize(subFolder.FullName));
        return totalSize;
    }

    public static IEnumerable<StorageItemDto> GetFiles(string path, string searchPattern)
    {
        var files = Directory.GetFiles(path, searchPattern);
        var items = files.Select(file => new FileInfo(file)).Select(fileInfo =>
            new StorageItemDto(name: fileInfo.Name, type: StorageItemType.File, size: fileInfo.Length,
                path: fileInfo.FullName, lastModified: fileInfo.LastWriteTime,
                created: fileInfo.CreationTime, lastAccessed: fileInfo.LastAccessTime));
        return items;
    }

    public static IEnumerable<StorageItemDto> GetItems(string path, string searchPattern)
    {
        if (!Directory.Exists(path))
        {
            return new List<StorageItemDto>();
        }

        var folders = GetFolders(path);
        var files = GetFiles(path, searchPattern);
        var items = folders.Concat(files);
        return items;
    }
}