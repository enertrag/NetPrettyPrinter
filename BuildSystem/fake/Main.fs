// Copyright(c) 2022, ENERTRAG SE
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1.Redistributions of source code must retain the above copyright notice, this
// list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimer in the documentation
// and/or other materials provided with the distribution.
//
// 3. Neither the name of the copyright holder nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES, LOSS OF USE, DATA, OR PROFITS, OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

module Main

open Fake.Core
open Fake.Core.TargetOperators
open Definitions
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
    Target.create "GenerateDocs" GenerateDocs.target

let private setupTargetDependendies () =
    "Clean" ==> "Rebuild" |> ignore
    "Build" ==> "Rebuild" |> ignore
    "Clean" ?=> "Build" |> ignore
    "Restore" ==> "Pack" |> ignore
    "Build" ==> "GenerateDocs" |> ignore

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
