

# Konso Metrics .Net Client

A .NET 5 Open Source Client Library for [KonsoApp](https://app.konso.io)

✅ Developed / Developing by [InDevLabs](https://indevlabs.de)


### Installation

⚠️ Prerequisites: *Konso account and created bucket* 

In order to use this library, you need reference it in your project.

Add config to `appsettings.json`:
```
"Konso": {
    "Metrics": {
        "Endpoint": "https://apis.konso.io",
        "BucketId": "<your bucket id>",
        "ApiKey": "<bucket's access key>"
    }
}
```


It uses `HttpClientFactory`:

```
public void ConfigureServices(IServiceCollection services)
{
    // use httpclient factory
    services.AddHttpClient();
}
```

Add the following to `Startup.cs`:
```
public void ConfigureServices(IServiceCollection services)
{
    // configure metrics middleware
    services.ConfigureKonsoMetricsMiddleware();
}
```

```
public void Configure(IApplicationBuilder app)
{
    // use metrics
    app.UseRequestPerformanceMiddleware();
}
```

Browse your application metrics at [https://app.konso.io](https://app.konso.io) 
