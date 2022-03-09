$oldwd = $PWD
Set-Location $PSScriptRoot

try {
    & dotnet tool restore
    & dotnet run --project BuildSystem/fake $args
}
finally {
    Set-Location $oldwd
}
