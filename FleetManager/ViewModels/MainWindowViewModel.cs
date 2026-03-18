using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using FleetManager.Models;
using FleetManager.Services;
using ReactiveUI;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    private readonly IVehicleService _vehicleService;
    
    public ReactiveCommand<Vehicle, Unit> RefuelCommand { get; }
    public ReactiveCommand<Vehicle, Unit> SendToRouteCommand { get; }
    public ReactiveCommand<Vehicle, Unit> SetServiceCommand { get; }

    public MainWindowViewModel(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;

        Task.Run(async () => await LoadAsync());

        RefuelCommand = ReactiveCommand.Create<Vehicle>(Refuel);
        SendToRouteCommand = ReactiveCommand.Create<Vehicle>(SendToRoute);
        SetServiceCommand = ReactiveCommand.Create<Vehicle>(SetService);
    }


    private async Task LoadAsync()
    {
        var list = await _vehicleService.LoadAsync();
        
        Vehicles.Clear();
        foreach (var v in list)
        {
            Vehicles.Add(v);
        }
    }

    private async Task SaveAsync()
    {
        await _vehicleService.SaveAsync(Vehicles);
    }

    private void Refuel(Vehicle v)
    {
        if (v.Status == VehicleStatus.InRoute)
        {
            Console.WriteLine("Nie można zatankować w trasie");
            return;
        }
            
        v.Fuel = 100;
        _ = SaveAsync();
    }

    private void SendToRoute(Vehicle v)
    {
        if (v.Fuel < 15 || v.Status == VehicleStatus.Service)
        {
            Console.WriteLine("Nie można wysłać w trasę");
            return;
        } if (v.Status == VehicleStatus.Service)
        {
            Console.WriteLine("Nie można wysłać w trasę");
            return;
        }
            
        
        v.Status = VehicleStatus.InRoute;
        
        _ = SaveAsync();
    }

    private void SetService(Vehicle v)
    {
        v.Status = VehicleStatus.Service;
        _ = SaveAsync();
    }
}