using System.Net.NetworkInformation;
using Hardware.Info;
using RigControlApi.Models;

namespace RigControlApi.Utilities;

public class HardwareUtility
{
    private readonly IHardwareInfo _hardwareInfo;
    private readonly HardwareDto _hardwareDto = new();

    public HardwareUtility()
    {
        _hardwareInfo = new HardwareInfo();
    }

    public OS GetOperatingSystem(bool refresh = false)
    {
        if (!refresh && _hardwareDto.OperatingSystem != null) return _hardwareDto.OperatingSystem;
        _hardwareInfo.RefreshOperatingSystem();
        _hardwareDto.OperatingSystem = _hardwareInfo.OperatingSystem;
        return _hardwareInfo.OperatingSystem;
    }

    public List<Drive> GetDriveList(bool refresh = false)
    {
        if (!refresh && _hardwareDto.DriveList != null) return _hardwareDto.DriveList;
        _hardwareInfo.RefreshDriveList();
        _hardwareDto.DriveList = _hardwareInfo.DriveList;
        //Make sure that each volume only appears once (by volume letter)
        var driveList = _hardwareInfo.DriveList;
        driveList.ForEach(d =>
        {
            d.PartitionList.ForEach(p =>
            {
                p.VolumeList = p.VolumeList.GroupBy(v => v.VolumeName).Select(drive => drive.First()).ToList();
            });
        });
        return driveList;
    }

    public List<Memory> GetMemoryList(bool refresh = false)
    {
        if (!refresh && _hardwareDto.MemoryList != null) return _hardwareDto.MemoryList;
        _hardwareInfo.RefreshMemoryList();
        _hardwareDto.MemoryList = _hardwareInfo.MemoryList;
        return _hardwareInfo.MemoryList;
    }

    public List<BIOS> GetBiosList(bool refresh = false)
    {
        if (!refresh && _hardwareDto.BiosList != null) return _hardwareDto.BiosList;
        _hardwareInfo.RefreshBIOSList();
        _hardwareDto.BiosList = _hardwareInfo.BiosList;
        return _hardwareInfo.BiosList;
    }

    public List<VideoController> GetVideoControllerList(bool refresh = false)
    {
        if (!refresh && _hardwareDto.VideoControllerList != null) return _hardwareDto.VideoControllerList;
        _hardwareInfo.RefreshVideoControllerList();
        _hardwareDto.VideoControllerList = _hardwareInfo.VideoControllerList;
        return _hardwareInfo.VideoControllerList;
    }

    public IEnumerable<NetworkDto> GetNetworkAdapterList(bool refresh = false)
    {
        if (!refresh && _hardwareDto.NetworkAdapterList != null) return _hardwareDto.NetworkAdapterList;
        _hardwareInfo.RefreshNetworkAdapterList();
        var result = _hardwareInfo.NetworkAdapterList.Select(adapter => new NetworkDto
        {
            Type = adapter.AdapterType,
            Manufacturer = adapter.Manufacturer,
            Speed = adapter.Speed,
            Caption = adapter.Caption,
            NetworkSpeed = new NetworkSpeed
            {
                ConnectionId = adapter.NetConnectionID,
                BytesReceivedPerSec = adapter.BytesReceivedPersec,
                BytesSentPerSec = adapter.BytesSentPersec
            },
            Name = adapter.Name,
            Description = adapter.Description,
            MacAddress = adapter.MACAddress,
            IpAddressList = adapter.IPAddressList.Select(ip => ip.ToString()),
            DnsServerList = adapter.DNSServerSearchOrderList.Select(ip => ip.ToString()),
            GatewayList = adapter.DefaultIPGatewayList.Select(ip => ip.ToString()),
            Dhcp = adapter.DHCPServer.ToString()
        });
        _hardwareDto.NetworkAdapterList = result.ToList();
        return _hardwareDto.NetworkAdapterList;
    }

    public List<CPU> GetCpuList(bool refresh = false)
    {
        if (!refresh && _hardwareDto.CpuList != null) return _hardwareDto.CpuList;
        _hardwareInfo.RefreshCPUList();
        _hardwareDto.CpuList = _hardwareInfo.CpuList;
        return _hardwareInfo.CpuList;
    }
}