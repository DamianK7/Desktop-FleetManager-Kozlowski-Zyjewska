using System.Collections.ObjectModel;
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
    }
}