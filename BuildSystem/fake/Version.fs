// Copyright(c) 2020, ENERTRAG SE
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
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
