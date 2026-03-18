// (ViewModel dla UserControl)

using System.Reactive;
using FleetManager.Models;
using ReactiveUI;

namespace FleetManager.ViewModels;

public class VehicleItemViewModel : ViewModelBase
{
    public readonly Vehicle _vehicle;

    public string Name => _vehicle.Name;
    public int Fuel => _vehicle.Fuel;
    public VehicleStatus Status => _vehicle.Status;

    public ReactiveCommand<Unit, Unit> SendCommand { get; }
    public ReactiveCommand<Unit, Unit> RefuelCommand { get; }

    public VehicleItemViewModel(Vehicle vehicle)
    {
        _vehicle = vehicle;

        // 🔥 TU dajesz WhenAnyValue
        var canSend = this.WhenAnyValue(
            _ => _vehicle.Fuel,
            _ => _vehicle.Status,
            (fuel, status) =>
                fuel >= 15 && status != VehicleStatus.Service
        );

        SendCommand = ReactiveCommand.Create(
            SendToRoute,
            canSend
        );

        RefuelCommand = ReactiveCommand.Create(Refuel);
    }

    private void SendToRoute()
    {
        _vehicle.Status = VehicleStatus.InRoute;
    }

    private void Refuel()
    {
        if (_vehicle.Status == VehicleStatus.InRoute)
            return;

        _vehicle.Fuel = 100;
    }
}