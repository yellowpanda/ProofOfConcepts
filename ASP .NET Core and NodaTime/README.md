# ASP .NET Core and NodaTime

Show how to make NodaTime work with ASP .NET Core. 

Remember to add NodaTime serializer options in `Startup.cs`
```
services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Bcl));
```