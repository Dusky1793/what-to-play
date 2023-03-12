using WhatToPlay.API.Interfaces;
using WhatToPlay.API.Services;

var builder = WebApplication.CreateBuilder(args);

const string CORS_POLICY_allowedSpecificOrigins = "_allowedSpecificOrigins";

// Add services to the container.
builder.Services.AddTransient<IHttpClientService, SteamHttpClientService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_POLICY_allowedSpecificOrigins,
        policy =>
        {
            // TODO: properly configure origin urls pre go-live
            policy.WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CORS_POLICY_allowedSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
