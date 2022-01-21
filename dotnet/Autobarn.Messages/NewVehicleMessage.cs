using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public DateTimeOffset ListedAt { get; set; }

        public NewVehiclePriceMessage ToNewVehiclePriceMessage(int price, string currencyCode) {
            var result = new NewVehiclePriceMessage {
                Price = price,
                CurrencyCode = currencyCode,
                Registration = this.Registration,
                Color = this.Color,
                ManufacturerName = this.ManufacturerName,
                ModelName = this.ModelName,
                Year = this.Year,
                ListedAt = this.ListedAt
            };
            return result;
        }

    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }

    }
}
