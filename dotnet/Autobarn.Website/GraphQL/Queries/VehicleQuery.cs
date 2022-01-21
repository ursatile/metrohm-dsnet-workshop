using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;
        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;

            Field<ListGraphType<VehicleGraphType>>("Vehicles",
                "Query to return all vehicles in the system",
                new QueryArguments(
                    new QueryArgument<IntGraphType> {
                        Name = "index",
                        Description = "the first vehicle in the collection"
                    },
                    new QueryArgument<IntGraphType> {
                        Name = "count",
                        Description = "the number of vehicles to return"
                    }
                ),
                resolve: GetAllVehicles);

            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor",
            "Query to return all vehicles of a particular color",
            new QueryArguments(MakeNonNullStringArgument("color", "What color cars do you want?")),
            resolve: GetVehiclesByColor);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext context) {
            var index = context.GetArgument<int>("index");
            var count = context.GetArgument<int>("count");
            return db.ListVehicles().Skip(index).Take(count);
        }

        private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles()
                .Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}