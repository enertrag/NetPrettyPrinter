// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
module Definitions

open Fake.IO.Globbing.Operators
open Version

[<Literal>]
let TargetFrameworkMoniker = "net6.0"

[<Literal>]
let Solution = "NetPrettyPrinter.sln"

let buildOutputGlob =
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ "tmp"
    ++ "output"

let projectGlob =
    !! "src/**/*.csproj"
    ++ "src/**/*.fsproj"

let nugetProjects = projectGlob

let version = readVersionFromVersionProps ()
