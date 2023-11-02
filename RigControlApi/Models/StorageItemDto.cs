namespace RigControlApi.Models;

public class StorageItemDto
{
    public StorageItemDto(string name, StorageItemType type, long size, string path, DateTime lastModified, DateTime created, DateTime lastAccessed)
    {
        Name = name;
        Type = type;
        Size = size;
        Path = path;
        LastModified = lastModified;
        Created = created;
        LastAccessed = lastAccessed;
    }

    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public StorageItemType Type { get; set; }
    
    public long Size { get; set; }
    
    public DateTime LastModified { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime LastAccessed { get; set; }
}

public enum StorageItemType
{
    File,
    Directory
}