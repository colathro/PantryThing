﻿using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pantry.ServiceGateways.Location;
using Pantry.WPF.Shared;
using Serilog.Core;
using Stylet;

namespace Pantry.WPF.Location
{
    public class LocationViewModel : Screen
    {
        private readonly LocationServiceGateway _locationServiceGateway;
        private readonly Logger _logger;
        public BindableCollection<Core.Models.Location> Locations { get; set; } = new();
        public Core.Models.Location SelectedLocation { get; set; }
        public string NewLocationName { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand AddLocationCommand { get; set; }
        public LocationViewModel(LocationServiceGateway locationServiceGateway, Logger logger)
        {
            _locationServiceGateway = locationServiceGateway;
            _logger = logger;
            DeleteCommand = new(DeleteSelectedLocation);
            AddLocationCommand = new(AddNewLocation);
            LoadData();
        }

        public void LoadData()
        {
            var locations = _locationServiceGateway.GetLocations();
            Locations.Clear();
            Locations.AddRange(locations);
        }

        public async Task AddNewLocation()
        {
            if (!string.IsNullOrWhiteSpace(NewLocationName))
            {
                var insertCount = await _locationServiceGateway.AddNewLocation(NewLocationName);
                if (insertCount > 0)
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Maybe a location with this name already exists.");
                }
            }
            else
            {
                MessageBox.Show("New Location Name must not be null or empty.");
            }
        }

        public async Task DeleteSelectedLocation()
        {
            if (SelectedLocation is null) return;
            var deleteCount = await _locationServiceGateway.DeleteLocation(SelectedLocation.LocationId);
            if (deleteCount.HasValue)
            {
                MessageBox.Show($"{deleteCount.Value} LocationFoods changed.");
                LoadData();
            }
            else
            {
                MessageBox.Show("Cannot delete the default location.");
            }
        }

    }
}
