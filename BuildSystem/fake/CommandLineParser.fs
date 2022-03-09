// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
module CommandLineParser

open Fake.DotNet
open Fake.Core

let private commandLineOptions target =
    $"""
usage: {target} [options]

options:
-c <cfg>              Configuration (Debug, Release)
"""

let private argsParser (target: string) = Docopt(commandLineOptions target)

let getConfiguration (parsedArgs: Map<string, DocoptResult>) =
    match DocoptResult.tryGetArgument "-c" parsedArgs with
    | Some ("Release") -> DotNet.Release
    | _ -> DotNet.Debug

let getParsedArgsFromTargetParameter (target: string) (targetParameter: TargetParameter) =
    (commandLineOptions target |> argsParser)
        .Parse(targetParameter.Context.Arguments)
