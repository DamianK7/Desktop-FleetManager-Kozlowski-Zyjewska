using Avalonia.Controls;
using FleetManager.Models;
using FleetManager.ViewModels;
using FleetManager.Services;
namespace FleetManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainWindowViewModel(new JsonVehicleService());
    }
}