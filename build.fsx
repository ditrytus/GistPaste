#r @"packages/FAKE.4.39.0/tools/FakeLib.dll"

open Fake

let buildDir = "./build/"

//Target "Clean" (fun _ ->
//    CleanDir buildDir
//)

Target "BuildApp" (fun _ ->
    !! "*GistPaste.Desktop/*.csproj"
        |> MSBuildRelease buildDir "Build"
        |> ignore
)

//"Clean"
//    ==> "BuildApp"

RunTargetOrDefault "BuildApp"