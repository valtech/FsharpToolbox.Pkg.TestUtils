module FsharpToolbox.Pkg.TestUtils.TestHost

open System
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open NUnit.Framework

open FsharpToolbox.Pkg.AspNetCore.Setup

let private setupTestServer (hostBuilder:IWebHostBuilder) = hostBuilder.UseTestServer()

let setIntegrationTestEnvironment () =
  let aspnetcoreEnvironment = "ASPNETCORE_ENVIRONMENT"
  let currentEnv = Environment.GetEnvironmentVariable(aspnetcoreEnvironment)

  if String.IsNullOrEmpty(currentEnv) then
    let integrationTestEnv = "IntegrationTest"
    TestContext.Progress.WriteLine($"[TestUtil]: Setting {aspnetcoreEnvironment} to integration test default: \"{integrationTestEnv}\"")
    Environment.SetEnvironmentVariable(aspnetcoreEnvironment, integrationTestEnv)
  else
    TestContext.Progress.WriteLine($"[TestUtil]: Using existing {aspnetcoreEnvironment} value: \"{currentEnv}\"")

let createHost(configureServices, configureApp) =
  setIntegrationTestEnvironment()
  WebHostHelpers
    .CreateWebHostBuilder(
      Array.Empty<string>(),
      Action<WebHostBuilderContext, IServiceCollection, IConfiguration> configureServices,
      Action<IApplicationBuilder, IConfiguration> configureApp,
      Func<IWebHostBuilder, IWebHostBuilder> setupTestServer)

let createClient (host : IHost) =
  let client = host.GetTestServer().CreateClient()
  client.DefaultRequestHeaders.Add("ApiKey", "AEX123")
  client
