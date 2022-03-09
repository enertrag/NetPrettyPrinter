// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
namespace Targets

module Pack =
    open Fake.Core
    open Fake.DotNet
    open CommandLineParser
    open Version

    let target (projects: seq<string>) (version: Version) (targetParameter: TargetParameter) =
        let parsedArgs = getParsedArgsFromTargetParameter "Pack" targetParameter
        let configuration = getConfiguration parsedArgs
        let packageVersion = version.ToString()

        let msbuildParams =
            { MSBuild.CliArguments.Create() with Properties = [ ("PackageVersion", packageVersion) ] }

        projects
        |> Seq.iter (
            DotNet.pack (fun packOptions ->
                { packOptions with
                    Configuration = configuration
                    MSBuildParams = msbuildParams
                    IncludeSymbols = true
                    OutputPath = Some("nupkgs")
                    NoRestore = true
                    NoBuild = true })
        )
