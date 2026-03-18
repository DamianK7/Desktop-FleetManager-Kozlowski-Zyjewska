using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FleetManager.Models;
using FleetManager.Services;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new();

    private readonly IVehicleService _vehicleService;

    public MainWindowViewModel(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;

        Task.Run(async () => await LoadAsync());
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
}