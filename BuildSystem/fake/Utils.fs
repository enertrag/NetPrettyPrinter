// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
module Utils

open System.IO
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing
open Fake.IO.Globbing.Operators
open Fake.Core
open Fake.DotNet
open NuGet.Configuration

let projectDirFromPartialName name =
    sprintf "PowerSystem.DatenStromDrehkreuz.%s" name

let projectFileFromPartialName name =
    sprintf "%s/PowerSystem.DatenStromDrehkreuz.%s.csproj" (projectDirFromPartialName name) name

let getNuGetGlobalPackageDir () =
    let settings = Settings.LoadDefaultSettings(null)
    SettingsUtility.GetGlobalPackagesFolder(settings)

let isFileNewer (fromFile: string) (toFile: string) =
    if not (Shell.testFile toFile) then
        true
    else
        let fromFileInfo = FileInfo.ofPath fromFile
        let toFileInfo = FileInfo.ofPath toFile
        fromFileInfo.LastWriteTimeUtc > toFileInfo.LastWriteTimeUtc

let copyFileIfNewer (fromFile: string) (toDir: string) =
    let fileName = System.IO.Path.GetFileName(fromFile)
    let toFile = Path.combine toDir fileName

    if isFileNewer fromFile toFile then
        Shell.cp fromFile toDir

let addGlobbingPatternFromSeq (patterns: seq<string>) =
    if Seq.length patterns = 0 then
        { BaseDirectory = Path.GetFullPath "."
          Includes = []
          Excludes = [] }
        :> IGlobbingPattern
    else
        let head = Seq.head patterns
        let tail = Seq.tail patterns

        tail
        |> Seq.fold (fun glob globPattern -> glob ++ globPattern) (!!head)
