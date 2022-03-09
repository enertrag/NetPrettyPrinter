// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
namespace Targets

module CleanAll =
    open Fake.Core
    open Fake.IO

    let target (glob: IGlobbingPattern) (_: TargetParameter) = glob |> Seq.iter Shell.cleanDir
