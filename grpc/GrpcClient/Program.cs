using Grpc.Net.Client;
using GrpcExample;

var random = new Random();
var languages = new[] { "en", "fr", "de-CH", "sk" };
var names = new[] { "Alice", "Bob", "Diana", "Carlos", "Eve" };

using var channel = GrpcChannel.ForAddress("https://localhost:7152");
var client = new Greeter.GreeterClient(channel);
Console.WriteLine("gRPC ready! Press any key to send a request...");
while (true) {
    Console.ReadKey();
    var firstName = names[random.Next(names.Length)];
    var lastName = names[random.Next(names.Length)];
    var language = languages[random.Next(languages.Length)];
    var request = new HelloRequest {
        FirstName = firstName,
        LastName = lastName,
        Language = language
    };
    var reply = client.SayHello(request);
    Console.WriteLine(reply.Message);
}