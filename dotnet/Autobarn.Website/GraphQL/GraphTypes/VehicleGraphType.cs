using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(v => v.Registration).Description("The registration plate of this vehicle");
            Field(v => v.Color).Description("What color is this vehicle?");
            Field(v => v.Year).Description("The year this vehicle was first registered");
            Field(v => v.VehicleModel, nullable: false,
            typeof(VehicleModelGraphType)).Description("Which model of vehicle is this?");
        }
    }
}