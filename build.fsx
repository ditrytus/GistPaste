#r @"packages/FAKE.4.39.0/tools/FakeLib.dll"

open Fake
open Fake.Testing

let buildDir = "./build/"
let testDir = "./test/"

Target "BuildApp" (fun _ ->
    !! "*GistPaste.Desktop/*.csproj"
        |> MSBuildRelease buildDir "Build"
        |> ignore
)

Target "BuildTest" (fun _ ->
    !! "*GistPaste.Desktop.UnitTests/*.csproj"
        |> MSBuildRelease testDir "Build"
        |> ignore
)

Target "Test" (fun _ ->
    !! (testDir @@ "GistPaste.Desktop.UnitTests.dll")
        |> xUnit2 (fun p -> p)
)

"BuildApp"
    ==> "BuildTest"
    ==> "Test"

RunTargetOrDefault "Test"