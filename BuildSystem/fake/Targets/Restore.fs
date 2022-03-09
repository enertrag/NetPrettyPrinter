// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
namespace Targets

module Restore =
    open Fake.Core
    open Fake.DotNet

    let target (solution: string) (_: TargetParameter) = DotNet.restore id solution
