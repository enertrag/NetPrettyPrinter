// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
module Main

open Fake.Core
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Definitions
open Version
open Utils
open Targets

let private setupTargets () =
    Target.initEnvironment ()
    Target.create "CleanAll" (CleanAll.target buildOutputGlob)
    Target.create "Clean" (Clean.target Solution)
    Target.create "Build" (Build.target Solution)
    Target.create "Rebuild" ignore
    Target.create "Restore" (Restore.target Solution)
    Target.create "Pack" (Pack.target nugetProjects version)
    Target.create "Noop" ignore

let private setupTargetDependendies () =
    "Clean" ==> "Rebuild" |> ignore
    "Build" ==> "Rebuild" |> ignore
    "Clean" ?=> "Build" |> ignore
    "Build" ==> "Pack" |> ignore

let private setupFakeContext (args: string list) =
    let execContext = Context.FakeExecutionContext.Create false "FakeBuild.exe" args

    Context.setExecutionContext (Context.RuntimeContext.Fake execContext)

let private runTarget (args: string list) =
    if args.Length = 0 then
        Target.runOrDefaultWithArguments "Noop"
    else
        Target.runOrDefaultWithArguments (List.head args)

[<EntryPoint>]
let private main (args: string []) =
    let fakeContextArgs =
        if args.Length = 0 then
            []
        else
            args |> Array.skip 1 |> Array.toList

    setupFakeContext fakeContextArgs
    setupTargets ()
    setupTargetDependendies ()

    try
        args |> Array.toList |> runTarget
        0
    with
    | _ -> 1
