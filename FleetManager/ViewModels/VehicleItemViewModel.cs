using System;
using System.Reactive;
using System.Threading.Tasks;
using FleetManager.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.ViewModels;

public class VehicleItemViewModel : ViewModelBase
{
    private readonly Vehicle _vehicle;
    public Vehicle Vehicle => _vehicle;
    private readonly Func<Task> _save;

    public string Name => _vehicle.Name;
    public string RegistrationNumber => _vehicle.RegistrationNumber;

    [Reactive] public int Fuel { get; set; }
    [Reactive] public VehicleStatus Status { get; set; }

    public ReactiveCommand<Unit, Unit> SendCommand { get; }
    public ReactiveCommand<Unit, Unit> RefuelCommand { get; }
    public ReactiveCommand<Unit, Unit> SetServiceCommand { get; }
    public ReactiveCommand<Unit, Unit> SetAvailableCommand { get; }

    public VehicleItemViewModel(Vehicle vehicle, Func<Task> save)
    {
        _vehicle = vehicle;
        _save = save;

        Fuel = vehicle.Fuel;
        Status = vehicle.Status;

        var canSend = this.WhenAnyValue(
            x => x.Fuel,
            x => x.Status,
            (fuel, status) => fuel >= 15 
                               && status != VehicleStatus.Service 
                               && status != VehicleStatus.InRoute
        );

        var canRefuel = this.WhenAnyValue(
            x => x.Status,
            status => status != VehicleStatus.InRoute
        );

        var canSetService = this.WhenAnyValue(
            x => x.Status,
            status => status != VehicleStatus.Service
        );

        var canSetAvailable = this.WhenAnyValue(
            x => x.Status,
            status => status != VehicleStatus.Available
        );

        SendCommand       = ReactiveCommand.Create(SendToRoute,   canSend);
        RefuelCommand     = ReactiveCommand.Create(Refuel,        canRefuel);
        SetServiceCommand = ReactiveCommand.Create(SetService,    canSetService);
        SetAvailableCommand = ReactiveCommand.Create(SetAvailable, canSetAvailable);
    }

    private void SendToRoute()
    {
        Status = VehicleStatus.InRoute;
        _vehicle.Status = Status;   
        _ = _save();
    }

    private void Refuel()
    {
        Fuel = 100;
        _vehicle.Fuel = Fuel;       
        _ = _save();
    }

    private void SetService()
    {
        Status = VehicleStatus.Service;
        _vehicle.Status = Status;
        _ = _save();
    }

    private void SetAvailable()
    {
        Status = VehicleStatus.Available;
        _vehicle.Status = Status;
        _ = _save();
    }
}