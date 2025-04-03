using dispatch_order_api.middlewares;
using Orders.Application.Handlers;
using Orders.Core.Repositories;
using Orders.Infrastructure;
using RabbitMQ.Client;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(OrdersCommandHandler).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(FailedDispatchHandler).GetTypeInfo().Assembly));


builder.Services.AddScoped<ISftpRepository, SftpRepository>();

Uri _uri = new Uri("amqp://guest:guest@rabbitmq:5672/#/");
builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory
{
    Uri = _uri,
    RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
    RequestedHeartbeat = TimeSpan.FromSeconds(30)
});

builder.Services.AddSingleton<Task<IConnection>>(async sp =>
{
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return await factory.CreateConnectionAsync();
});

// Register IConnection as a singleton
builder.Services.AddSingleton<IConnection>(sp =>
{
    var connection = sp.GetRequiredService<Task<IConnection>>();
    return connection.Result;  // Wait for async task to complete
});

builder.Services.AddScoped<IFailedDispatchStoreRespository, MQOrderRespository>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<MQOrderRespository>>();
    var connectionTask = sp.GetRequiredService<Task<IConnection>>();
    var connection = connectionTask.GetAwaiter().GetResult();

    var mq = new MQOrderRespository(logger, connection);

    return mq;
});

builder.Services.AddScoped<IFailedDispatchProcessRepository, FailedDispatchProcessRepository>(x =>
{
    var logger = x.GetRequiredService<ILogger<FailedDispatchProcessRepository>>();
    var connectionTask = x.GetRequiredService<Task<IConnection>>();
    var connection = connectionTask.GetAwaiter().GetResult();

    var mq = new FailedDispatchProcessRepository(logger, connection);

    return mq;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var queue = scope.ServiceProvider.GetRequiredService<IFailedDispatchStoreRespository>();
    await queue.QueueInit();
}

using (var scope = app.Services.CreateScope())
{
    var queue = scope.ServiceProvider.GetRequiredService<IFailedDispatchProcessRepository>();
    var result = queue.Redispatch().Result;
}

app.UseMiddleware<CustomAuthenticationMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
