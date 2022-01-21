using Grpc.Core;
using GrpcExample;
using System;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase {
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger) {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        string name;
        name = $"{request.FirstName} {request.LastName}";
        var message = request.Language switch {
            "en" => $"Hello, {name}",
            "fr" => $"Bonjour, {name}",
            "de-CH" => $"Grüetzi, {name}",
            _ => $"Sorry, {name} - I don't know how to greet people in {request.Language}"
        };
        return Task.FromResult(new HelloReply {
            Message = message
        });
    }
}
