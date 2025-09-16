using FinanceTracker.Api;
using FinanceTracker.Api.Extensions;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Api.Services.Providers;
using FinanceTracker.AwsS3.Sdk.Configs;
using FinanceTracker.AwsS3.Sdk.Services.Interfaces;
using FinanceTracker.AwsS3.Sdk.Services.Providers;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Pg.Sdk;
using FinanceTracker.Pg.Sdk.Repositories;
using FinanceTracker.Pg.Sdk.Repositories.Interfaces;
using FinanceTracker.Pg.Sdk.Repositories.Providers;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddActorSystem();
services.AddControllers();

services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

services.AddRouting(x => x.LowercaseUrls = true);

services.AddDataLayer(builder.Configuration, "FinanceTrackerDb");

services.AddScoped<IAmazonBucketService, AmazonBucketService>();
services.AddScoped<ITransactionService, TransactionService>();
services.Configure<AwsS3Config>(builder.Configuration.GetSection(nameof(AwsS3Config)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.ApplyPendingMigrations();

app.UseRouting();
app.UseCors();
app.MapControllers();



await app.RunAsync();
