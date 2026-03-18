using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.Models;

public class Vehicle
{
    public string Name { get; set; } = "";
    public int Fuel { get; set; }
    public VehicleStatus Status { get; set; }
}