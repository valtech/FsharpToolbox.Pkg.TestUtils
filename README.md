
# FsharpToolbox.Pkg.TestUtils

This repo contains reusable pieces for writing tests.

## How to use in unit tests

Add a package reference to `FsharpToolbox.Pkg.TestUtils` in your unit test project.

## How to use for writing integration tests

### EventListener.fs
Example setup:
```
  let mutable eventsSent = 0

  let topicReceiverSettings =
    TopicReceiverSettings(
      ConnectionString = "...",
      TopicName = "...",
      SubscriptionName = "..."
    )

  let handleOrderPaid (_order : OrderEvent) =
    eventsSent <- eventsSent + 1
    () |> Ok

  let callbacks = TopicListener.createCallback<OrderEvent> "OrderPaid" "1" handleOrderPaid

  [<TearDown>]
  member _.cleanupEachTest () =
    eventsSent <- 0

  [<OneTimeSetUp>]
  member _.setup () =
    topicListener <- TopicListener.startEventListening topicReceiverSettings callbacks |> Some
```
Example usage in test:
```
    // Do some workflow in test that sends an event
    TopicListener.waitForCondition (fun () -> eventsSent > 0)
    Assert.AreEqual(1, eventsSent)
```
### TestHost.fs

The TestHost helpers default to using the `IntegrationTest` environment. To configure integration
test specific application settings, you need to create an `appsettings.IntegrationTest.json` file
in your application's main project.

The default environment can be overridden by setting the ASPNETCORE_ENVIRONMENT environment variable
when running the tests.

```
  let mutable host : IHost = null
  let mutable client = null
  
  [<SetUp>]
  member _.setup () =
    host <- TestHost.createHost(App.configureServices, App.configureApp).Start()
    client <- TestHost.createClient(host)
      
  [<Test>]
  member _.``Test exampl``() =
    let response = client.GetAsync $"/path?arg=%s{value}"
```