using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors((option) =>
{
    option.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();



app.MapGet("/", (HttpContext context) =>
{
     var addresses = context.RequestServices
        .GetService<IServer>()?
        .Features.Get<IServerAddressesFeature>()?
        .Addresses.ToList();


    var data = new Dictionary<string, string> {
        { "Hello World", "Server Connected" },
        {"Dev Swagger", addresses != null ? addresses[0] + "/swagger": "Unknown"},
        // {"Prod Swagger", addresses != null ? addresses[1] + "/swagger": "Unknown"},

    };
    Console.WriteLine(app.Urls);

    return Results.Json(data);
});

app.Run();

