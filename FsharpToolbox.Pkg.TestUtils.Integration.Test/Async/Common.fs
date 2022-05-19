module FsharpToolbox.Pkg.TestUtils.Integration.Test.Async.Common

open System
open System.Threading
open System.Threading.Tasks
open Polly.Utilities

let disablePollySleep () =
  // https://github.com/App-vNext/Polly/issues/556#issuecomment-451768251
  SystemClock.Sleep      <- Action<TimeSpan,CancellationToken>(fun _ __ -> ())
  SystemClock.SleepAsync <- Func<TimeSpan,CancellationToken,Task>(fun _ __ -> Task.CompletedTask)

let resetPollySleepBehavior () =
  // https://github.com/App-vNext/Polly/issues/556#issuecomment-451768251
  SystemClock.Reset()
