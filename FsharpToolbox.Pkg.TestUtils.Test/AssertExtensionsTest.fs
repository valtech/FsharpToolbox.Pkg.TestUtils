module FsharpToolbox.Pkg.TestUtils.AssertExtensionsTest

open NUnit.Framework
open FsharpToolbox.Pkg.TestUtils

[<SetUp>]
let Setup () =
  ()

[<Test>]
let ``assumeOk returns the value of an Ok`` () =
  let value = Ok "foo" |> Assert.assumeOk
  Assert.That(value, Is.EqualTo("foo"))

[<Test>]
let ``assumeOk throws when given an Error`` () =
  Assert.Throws<System.Exception>(fun () -> Error "foo" |> Assert.assumeOk) |> ignore

[<Test>]
let ``assumeError returns the value of an Error`` () =
  let value = Error "foo" |> Assert.assumeError
  Assert.That(value, Is.EqualTo("foo"))

[<Test>]
let ``assumeError throws when given an Ok`` () =
  Assert.Throws<System.Exception>(fun () -> Ok "foo" |> Assert.assumeError) |> ignore

[<Test>]
let ``isOk does not throw when given an Ok`` () =
  Ok "foo" |> Assert.isOk

[<Test>]
let ``isOk throws when given an Error`` () =
  Assert.Throws<System.Exception>(fun () -> Error "foo" |> Assert.isOk) |> ignore

[<Test>]
let ``isError does not throw when given an Error`` () =
  Error "foo" |> Assert.isError

[<Test>]
let ``isError throws when given an Ok`` () =
  Assert.Throws<System.Exception>(fun () -> Ok "foo" |> Assert.isError) |> ignore

[<Test>]
let ``assumeSome returns the value of a Some`` () =
  let value = Some "foo" |> Assert.assumeSome
  Assert.That(value, Is.EqualTo("foo"))

[<Test>]
let ``assumeSome throws when given a None`` () =
  Assert.Throws<System.Exception>(fun () -> None |> Assert.assumeSome) |> ignore

[<Test>]
let ``isSome does not throw when given a Some`` () =
  Some "foo" |> Assert.isSome

[<Test>]
let ``isSome throws when given a None`` () =
  Assert.Throws<System.Exception>(fun () -> None |> Assert.isSome) |> ignore

[<Test>]
let ``isNone does not throw when given a None`` () =
  None |> Assert.isNone

[<Test>]
let ``isNone throws when given a Some`` () =
  Assert.Throws<System.Exception>(fun () -> Some "foo" |> Assert.isNone) |> ignore
