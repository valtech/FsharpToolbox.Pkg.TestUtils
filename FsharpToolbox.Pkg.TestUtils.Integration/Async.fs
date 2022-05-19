namespace FsharpToolbox.Pkg.TestUtils

open System
open NUnit.Framework
open Polly

module Async =
  let private sleepProvider retryCount =
    retryCount |> float |> TimeSpan.FromSeconds

  let waitFor (checkIfComplete: unit -> bool) =
    let retryCount = 5
    let isComplete =
      Policy
        .HandleResult<bool>(fun isComplete -> not isComplete)
        .WaitAndRetry(
          retryCount = retryCount,
          sleepDurationProvider = sleepProvider)
        .Execute(fun () -> checkIfComplete ())

    if not isComplete
    then Assert.Fail($"Async.waitFor: The condition is still false after %d{retryCount} retries")

  let waitForAsync (checkIfComplete: unit -> Async<bool>) =
    async {
      let retryCount = 5
      let! isComplete =
        Policy
          .HandleResult<bool>(fun isComplete -> not isComplete)
          .WaitAndRetryAsync(
            retryCount = retryCount,
            sleepDurationProvider = Func<int,TimeSpan>(sleepProvider))
          .ExecuteAsync(fun () -> checkIfComplete () |> Async.StartAsTask)
        |> Async.AwaitTask

      if not isComplete
      then Assert.Fail($"Async.waitForAsync: The condition is still false after %d{retryCount} retries")
    }
