module FsharpToolbox.Pkg.TestUtils.Integration.Test.Async.WaitForAsyncTest

open System.Threading.Tasks
open NUnit.Framework
open FsharpToolbox.Pkg.TestUtils
open FsharpToolbox.Pkg.TestUtils.Integration.Test.Async.Common

[<SetUp>]
let setUp () =
  disablePollySleep ()

[<TearDown>]
let tearDown () =
  resetPollySleepBehavior ()

[<Test>]
let ``When waitForAsync check returns true, returns directly without error`` () =
  Async.waitForAsync (fun () -> true |> async.Return )

[<Test>]
let ``When waitForAsync check first returns false then true, returns without error`` () =
  let isComplete =
    let mutable index = 0
    fun () ->
      index <- index + 1
      index <> 1

  Async.waitForAsync (fun () -> isComplete () |> async.Return)

[<Test>]
let ``When waitForAsync check returns false, it raises an AssertionException after the retries`` () =
  let failingCheck = fun () -> false |> async.Return
  let ex = Assert.Throws<AssertionException>(fun () -> Async.waitForAsync failingCheck |> Async.RunSynchronously)
  Assert.That(ex.Message, Does.Contain("Async.waitForAsync: The condition is still false"))
