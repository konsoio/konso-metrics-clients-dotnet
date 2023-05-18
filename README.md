![Nuget](https://img.shields.io/nuget/v/Konso.Clients.Metrics) 
![example workflow](https://github.com/konsoio/konso-metrics-clients-dotnet/actions/workflows/dotnet.yml/badge.svg)

# Konso Metrics .Net Client

The Konso Metrics .NET client library is a powerful tool designed to capture web application metrics. It provides a simple and efficient way to monitor and analyze various aspects of your web application's performance, allowing you to gain valuable insights and make data-driven decisions.

## Features

**Metric Collection**: Konso Metrics enables you to capture a wide range of metrics related to your web application, including request/response times, error rates, throughput, latency, and many others. It offers a comprehensive set of predefined metrics as well as the flexibility to define custom metrics tailored to your specific needs.

**Real-time Monitoring**: With Konso Metrics, you can monitor the performance of your web application in real-time. It provides live updates and visualizations of metrics, allowing you to quickly identify bottlenecks, track trends, and detect anomalies.

**Integration with Existing Systems**: The library seamlessly integrates with popular .NET frameworks and libraries, making it easy to instrument your web application without significant code changes. It supports integration with ASP.NET, ASP.NET Core, and other commonly used web frameworks.

**Easy to Use**: Konso Metrics is designed to be easy to use and developer-friendly. It offers a straightforward API and clear documentation, making it simple to integrate into your existing projects. Additionally, it provides robust error handling and diagnostics capabilities, ensuring a smooth experience during implementation and troubleshooting.

## Getting Started

To start using the Konso Metrics .NET Client Library, simply follow the steps below:

⚠️ Prerequisites: *Konso account and created bucket*

### Install the library via NuGet or by manually referencing the assembly in your project

```

NuGet\Install-Package Konso.Clients.Metrics

```

### Initialize the library with your API credentials and configuration settings

Add config to `appsettings.json`:

```json
"Konso": {
    "Metrics": {
        "Endpoint": "https://apis.konso.io",
        "BucketId": "<your bucket id>",
        "ApiKey": "<bucket's access key>"
    }
}
```

in `startup.cs`:

```csharp

builder.Logging.ConfigureKonsoMetrics(options =>
{
    options.Endpoint = builder.Configuration["Konso:Metrics:Endpoint"];
    options.BucketId = builder.Configuration["Konso:Metrics:BucketId"];
    options.AppName = builder.Configuration["Konso:Metrics:App"];
});

```

Use to register a middleware:

```csharp

    builder.UseKonsoMetrics();

```

Metrics will be captured every time there is request to the application

## Requirements

- .NET 5 or higher
- Konso BucketId and API key

## Support and Feedback

If you encounter any issues or have any questions or feedback, please reach out to our support team at <support at konso.io>. We are here to assist you and continually improve the Konso Value Tracking .NET Client Library to meet your business needs.

✅ Developed / Developing by [InDevLabs](https://indevlabs.de)
