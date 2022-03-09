#!/usr/bin/env bash

oldwd="$PWD"

trap "cd \"$oldwd\"" EXIT

cd `dirname "$0"`

set -eu
set -o pipefail

dotnet tool restore
dotnet run --project BuildSystem/fake "$@"
