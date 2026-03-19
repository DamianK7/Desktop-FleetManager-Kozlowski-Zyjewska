using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.Models;

public class Vehicle : ReactiveObject
{
    [Reactive] public string Name { get; set; } = "";
    [Reactive] public string RegistrationNumber { get; set; } = "";
    [Reactive] public int Fuel { get; set; }
    [Reactive] public VehicleStatus Status { get; set; }
}