namespace FsharpToolbox.Pkg.TestUtils

open NUnit.Framework

[<AutoOpen>]
module Extensions =
  type Assert
  with
    static member assumeOk result =
      match result with
      | Ok value -> value
      | Error e -> failwith $"Assumed Ok, but was Error. ErrorValue: %A{e}"

    static member assumeError result =
      match result with
      | Ok value -> failwith $"Assumed Error, but was Ok. ResultValue: %A{value}"
      | Error e -> e

    static member isOk result =
      match result with
      | Ok _ -> ()
      | _ -> failwith "Expected Ok"

    static member isError result =
      match result with
      | Error _ -> ()
      | _ -> failwith "Expected Error"

    static member assumeSome option =
      match option with
      | Some value -> value
      | None -> failwith "Assumed Some, but was None."

    static member isSome option =
      match option with
      | Some _value -> ()
      | None -> failwith "Assumed Some, but was None."

    static member isNone option =
      match option with
      | Some value -> failwith $"Assumed None, but was Some. Value: %A{value}"
      | None -> ()
