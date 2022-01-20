using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public ModelsController(IAutobarnDatabase db, IBus bus) {
            this.db = db;
            this.bus = bus;
        }

        [HttpGet]
        public IEnumerable<Model> Get() {
            return db.ListModels();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var resource = vehicleModel.ToDynamic();
            resource._actions = new {
                create = new {
                    href = $"/api/models/{id}",
                    type = "application/json",
                    method = "POST",
                    name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
                }
            };
            return Ok(resource);
        }

        // POST api/vehicles
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody] VehicleDto dto) {
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default) return Conflict($"Sorry, there is already a vehicle with registration {dto.Registration} in the database.");
            var vehicleModel = db.FindModel(id);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            // db.CreateVehicle(vehicle);
            await PublishNewVehicleMessage(vehicle);
            return Accepted($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
        }

        private async Task PublishNewVehicleMessage(Vehicle vehicle) {
            var message = new NewVehicleMessage {
                Registration = vehicle.Registration,
                ModelName = vehicle.VehicleModel?.Name ?? "(model missing)",
                ManufacturerName = vehicle.VehicleModel?.Manufacturer?.Name ?? "(manufacturer missing)",
                Color = vehicle.Color,
                Year = vehicle.Year,
                ListedAt = DateTimeOffset.UtcNow
            };
            await bus.PubSub.PublishAsync(message);
        }
    }
}