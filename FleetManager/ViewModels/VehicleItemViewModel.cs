// (ViewModel dla UserControl)

using System;
using System.Reactive;
using System.Threading.Tasks;
using FleetManager.Models;
using ReactiveUI;

namespace FleetManager.ViewModels;

public class VehicleItemViewModel : ViewModelBase
{
    private readonly Vehicle _vehicle;
    public Vehicle Vehicle => _vehicle;
    private readonly Func<Task> _save;

    public string Name => _vehicle.Name;
    public int Fuel => _vehicle.Fuel;
    public VehicleStatus Status => _vehicle.Status;

    public ReactiveCommand<Unit, Unit> SendCommand { get; }
    public ReactiveCommand<Unit, Unit> RefuelCommand { get; }

    public VehicleItemViewModel(Vehicle vehicle, Func<Task> save)
    {
        _vehicle = vehicle;
        _save = save;

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
        _ = _save();
    }

    private void Refuel()
    {
        if (_vehicle.Status == VehicleStatus.InRoute)
            return;

        _vehicle.Fuel = 100;
        _ = _save();
    }
}