module FsharpToolbox.Pkg.TestUtils.EventListener

open System
open System.Threading
open FsharpToolbox.Pkg.Communication.ServiceBus
open FsharpToolbox.Pkg.EventListener
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions
open FsharpToolbox.Pkg.TestUtils
open FsharpToolbox.Pkg.FpUtils

type TopicListener(hostedService: IHostedService) =

  static member createCallback<'T> eventName eventVersion handleEvent: EventListenerCallbacks = {
    onEvent = [
      {name = eventName; version = eventVersion},
        EventListener.createEventProcessor<'T> (fun (_scope: IServiceProvider) -> handleEvent)
    ] |> Map.ofList
  }

  static member startEventListening (topicReceiverSettings : TopicReceiverSettings) (callbacks : EventListenerCallbacks): TopicListener =
    let services: IServiceCollection = ServiceCollection() :> IServiceCollection

    let eventReceiver = EventReceiver(TopicReceiverClient.CreateTopicReceiverClient(topicReceiverSettings))

    let serviceProvider =
      services
        .AddSingleton<ILoggerFactory, NullLoggerFactory>()
        .AddHostedService<EventListener>(fun services ->
          let scope : IServiceScopeFactory = services.GetRequiredService<IServiceScopeFactory>()
          EventListener(scope, eventReceiver, callbacks))
        .BuildServiceProvider()

    let service: EventListener = serviceProvider.GetService<IHostedService>() :?> EventListener
    let svc: IHostedService = service :> IHostedService

    svc.StartAsync(CancellationToken.None)
    |> Async.AwaitTask
    |> Async.RunSynchronously

    TopicListener(svc)

  member this.stopEventListening() =
    hostedService.StopAsync(CancellationToken.None)
    |> Async.AwaitTask
    |> Async.RunSynchronously

  static member waitForCondition (f: Unit -> bool) =
    Async.waitFor (fun () -> f())

