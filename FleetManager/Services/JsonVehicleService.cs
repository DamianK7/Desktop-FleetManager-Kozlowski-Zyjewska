using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FleetManager.Models;

namespace FleetManager.Services;

public class JsonVehicleService : IVehicleService
{
    private const string FilePath = "Data/vehicles.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public async Task<List<Vehicle>> LoadAsync()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new List<Vehicle>();

            var json = await File.ReadAllTextAsync(FilePath);
            var data = JsonSerializer.Deserialize<List<Vehicle>>(json, JsonOptions);
            return data ?? new List<Vehicle>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BŁĄD: {ex.Message}");
            return new List<Vehicle>();
        }
    }

    public async Task SaveAsync(IEnumerable<Vehicle> vehicles)
    {
        try
        {
            var json = JsonSerializer.Serialize(vehicles, JsonOptions);
            await File.WriteAllTextAsync(FilePath, json);
        }
        catch
        {
            Console.WriteLine("Błąd zapisu danych");
        }
    }
}