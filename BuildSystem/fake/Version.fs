// Copyright (c) 2022 ENERTRAG SE. All rights reserved.
module Version

open FSharp.Data
open Fake.IO

type Version =
    { Major: int32
      Minor: int32
      Build: int32
      Revision: int32 }
    override this.ToString() =
        $"{this.Major}.{this.Minor}.{this.Build}.{this.Revision}"

[<Literal>]
let private VersionPropsPath =
    __SOURCE_DIRECTORY__ + "/../msbuild/Version.props"

type private VersionProps = XmlProvider<VersionPropsPath>

let readVersionFromVersionProps () =
    let versionProps = VersionProps.Load(VersionPropsPath)
    let propertyGroup = versionProps.PropertyGroups.[0]

    { Major = propertyGroup.Major.Value
      Minor = propertyGroup.Minor.Value
      Build = propertyGroup.Build.Value
      Revision = propertyGroup.Revision.Value }

let readTimeStampFromVersionProps () =
    (FileInfo.ofPath VersionPropsPath)
        .LastWriteTimeUtc
