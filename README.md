# Rig Control API

Rig Control API is a robust and extensible backend service designed to monitor and report hardware resource utilization such as CPU, GPU, and Network activities in real-time. It employs a modular architecture, making it straightforward to add support for monitoring additional hardware resources.

## Features

- Real-time monitoring of hardware resources including CPU, GPU, and Network.
- SignalR integration for real-time updates to connected clients.
- A well-structured and extensible design allowing for easy addition of new hardware monitors.
- Dependency Injection for better testability and loose coupling.
- Timer-based polling of hardware resources.
- Error handling to ensure robustness.

## Getting Started

### Prerequisites

- .NET Core 3.1 or later

### Installation

1. Clone the repository
```bash
git clone https://github.com/your-username/RigControlApi.git
```
2. Navigate to the project directory
```bash
cd RigControlApi
```
3. Restore the .NET packages
```bash
dotnet restore
```
4. Build and run the project
```bash
dotnet build
dotnet run
```

## Usage

Once the Rig Control API is running, clients can connect to the SignalR hub to receive real-time updates on hardware resource utilization. The API provides separate monitors for CPU, GPU, and Network resources, each reporting relevant metrics such as load, temperature, throughput, etc.

## Extending

To add a new hardware monitor:

1. Create a new class in the `RigControlApi.Utilities.Monitors` namespace.
2. Inherit from the `HardwareMonitor` base class.
3. Provide a concrete implementation of the `UpdateHub` method to send the hardware usage data to the SignalR clients in the appropriate manner for the new type of hardware.

## Contributing

Feel free to fork the project, create a feature branch, and send us a pull request!

## License

This project is licensed under the MIT License.
