using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using FleetManager.Models;
using FleetManager.Services;
using ReactiveUI;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<VehicleItemViewModel> Vehicles { get; } = new();

    private readonly IVehicleService _vehicleService;
    
    public ReactiveCommand<Vehicle, Unit> RefuelCommand { get; }
    public ReactiveCommand<Vehicle, Unit> SetServiceCommand { get; }
    public ReactiveCommand<Vehicle, Unit> SetAvailableCommand { get; }

    public MainWindowViewModel(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;

        Task.Run(async () => await LoadAsync());

        RefuelCommand = ReactiveCommand.Create<Vehicle>(Refuel);
        SetServiceCommand = ReactiveCommand.Create<Vehicle>(SetService);
        SetAvailableCommand = ReactiveCommand.Create<Vehicle>(SetAvailable);
    }


    private async Task LoadAsync()
    {
        try
        {
            var list = await _vehicleService.LoadAsync();

            Vehicles.Clear();
            foreach (var v in list)
            {
                Vehicles.Add(new VehicleItemViewModel(v, SaveAsync));
            }
        }
        catch
        {
            Vehicles.Clear();
            Console.WriteLine("Brak danych - tryb awaryjny");
        }
        
    }

    private async Task SaveAsync()
    {
        var list = Vehicles
            .Select(vm => vm.Vehicle)
            .ToList();
        await _vehicleService.SaveAsync(list);
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
    

    private void SetService(Vehicle v)
    {
        v.Status = VehicleStatus.Service;
        _ = SaveAsync();
    }
    
    private void SetAvailable(Vehicle v)
    {
        v.Status = VehicleStatus.Available;
        _ = SaveAsync();
    }
}