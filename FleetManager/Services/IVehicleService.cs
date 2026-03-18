using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManager.Models;

namespace FleetManager.Services;

public interface IVehicleService
{
    Task<List<Vehicle>> LoadAsync();
    Task SaveAsync(IEnumerable<Vehicle> vehicles);
}