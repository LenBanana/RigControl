using RigControlApi.Models;

namespace RigControlApi.Hubs.Interfaces;

public interface INetworkDataHub
{
    public Task SendNetworkSpeed(NetworkSpeed networkSpeed);
}