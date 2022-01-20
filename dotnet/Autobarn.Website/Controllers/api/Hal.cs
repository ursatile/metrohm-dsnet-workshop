using System.Collections.Generic;
using System.Dynamic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    public static class Hal {
        public static dynamic PaginateAsDynamic(string baseUrl, int index, int count, int total) {
            dynamic links = new ExpandoObject();
            links.self = new { href = "/api/vehicles" };
            if (index < total) {
                links.next = new { href = $"/api/vehicles?index={index + count}" };
                links.final = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
            }
            if (index > 0) {
                links.prev = new { href = $"/api/vehicles?index={index - count}" };
                links.first = new { href = $"/api/vehicles?index=0" };
            }
            return links;
        }

        public static Dictionary<string, object> PaginateAsDictionary(string baseUrl, int index, int count, int total) {
            var links = new Dictionary<string, object>();
            links.Add("self", new { href = "/api/vehicles" });
            if (index < total) {
                links["next"] = new { href = $"/api/vehicles?index={index + count}" };
                links["final"] = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
            }
            if (index > 0) {
                links["prev"] = new { href = $"/api/vehicles?index={index - count}" };
                links["first"] = new { href = $"/api/vehicles?index=0" };
            }
            return links;
        }
    }
}
