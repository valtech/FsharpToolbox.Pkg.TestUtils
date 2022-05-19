module FsharpToolbox.Pkg.TestUtils.Integration.Test.Async.WaitForTest

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
let ``When waitFor check returns true, returns directly without error`` () =
  Async.waitFor (fun () -> true)

[<Test>]
let ``When waitFor check first returns false then true, returns without error`` () =
  let isComplete =
    let mutable index = 0
    fun () ->
      index <- index + 1
      index <> 1

  Async.waitFor (fun () -> isComplete ())


[<Test>]
let ``When waitFor check returns false, raises an error after the retries`` () =
  let ex = Assert.Throws<AssertionException>(fun () -> Async.waitFor (fun () -> false))
  Assert.That(ex.Message, Does.Contain("Async.waitFor: The condition is still false"))

