using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleModelGraphType : ObjectGraphType<Model> {
        public VehicleModelGraphType() {
            Name = "vehicleModel";
            Field(m => m.Name).Description("The name of this model");
            Field(m => m.Manufacturer, nullable: false,
            typeof(ManufacturerGraphType)).Description("The manufacturer who makes this model");
        }
    }
}