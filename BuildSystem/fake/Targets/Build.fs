// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
namespace Targets

module Build =
    open Fake.Core
    open Fake.DotNet
    open CommandLineParser

    let target (solution: string) (targetParameter: TargetParameter) =
        DotNet.build
            (fun buildOptions ->
                let parsedArgs = getParsedArgsFromTargetParameter "Build" targetParameter
                let configuration = getConfiguration parsedArgs
                let msbuildParams = { MSBuild.CliArguments.Create() with NodeReuse = true }

                { buildOptions with
                    Configuration = configuration
                    NoRestore = true
                    MSBuildParams = msbuildParams })
            solution
