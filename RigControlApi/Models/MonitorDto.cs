using LibreHardwareMonitor.Hardware;

namespace RigControlApi.Models;

public class MonitorDto
{
    public MonitorDto(string id)
    {
        Id = id;
    }
    public List<LabeledData> Data { get; set; } = new();
    public string Id { get; set; }
    public int MaxValue { get; set; } = 100;
    public int MinValue { get; set; } = 0;
}

public class LabeledData
{
    public LabeledData(string label, double data)
    {
        Label = label;
        Data = data;
    }

    public string Label { get; set; }
    public double Data { get; set; }
}

public class MonitorSensor
{
    public MonitorSensor(SensorType sensorType, string name, string label)
    {
        Name = name;
        Label = label;
        SensorType = sensorType;
    }

    public SensorType SensorType { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
}