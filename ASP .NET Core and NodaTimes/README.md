# ASP .NET Core and NodaTimes

Show how to make NodaTimes work with ASP .NET Core. 

Remember to add NodaTime serializer options in `Startup.cs`
```
services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Bcl));
```