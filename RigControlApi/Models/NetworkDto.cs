namespace RigControlApi.Models;

public class NetworkDto
{
    public string? Type { get; set; }
    public string? Manufacturer { get; set; }
    public ulong Speed { get; set; }
    public string? Caption { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? MacAddress { get; set; }
    public string? ConnectionId => NetworkSpeed?.ConnectionId;
    public NetworkSpeed NetworkSpeed { get; set; }
    public IEnumerable<string> IpAddressList { get; set; }
    public IEnumerable<string> DnsServerList { get; set; }
    public IEnumerable<string> GatewayList { get; set; }
    public string? Dhcp { get; set; }

    public NetworkDto()
    {
        IpAddressList = new List<string>();
        DnsServerList = new List<string>();
        GatewayList = new List<string>();
    }
}

public class NetworkSpeed
{
    public string? ConnectionId { get; set; }
    public ulong BytesReceivedPerSec { get; set; }
    public ulong BytesSentPerSec { get; set; }
}