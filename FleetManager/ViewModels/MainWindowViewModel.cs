using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using FleetManager.Services;
using ReactiveUI;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<VehicleItemViewModel> Vehicles { get; } = new();

    private readonly IVehicleService _vehicleService;

    public MainWindowViewModel(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
        Task.Run(async () => await LoadAsync());
    }

    private async Task LoadAsync()
    {
        try
        {
            var list = await _vehicleService.LoadAsync();
            Vehicles.Clear();
            foreach (var v in list)
                Vehicles.Add(new VehicleItemViewModel(v, SaveAsync));
        }
        catch
        {
            Vehicles.Clear();
            Console.WriteLine("Brak danych - tryb awaryjny");
        }
    }

    private async Task SaveAsync()
    {
        var list = Vehicles.Select(vm => vm.Vehicle).ToList();
        await _vehicleService.SaveAsync(list);
    }
}