namespace FSharpPlayground

open Suave
open Suave.Files
open Suave.Filters
open Suave.Operators
open Suave.Successful

open Page
open StaticAssets
open TempAssets
open WebSocket

module Routing =
    let (|OnlyContent|FullPage|) (req : Choice<string, string>) =
        match req with
        | Choice1Of2 x -> OnlyContent
        | Choice2Of2 x -> FullPage

    let pageOrContent (link : string) (path : string) =
        request (fun req -> match req.queryParam "onlyContent" with
                            | OnlyContent -> OK (Page.ContentResponse (Assets.Title link) link (content link))
                            | FullPage -> OK (Page.Html link path Assets.Scripts Assets.StyleSheets Assets.InlineStyle Assets.Links (content link) messages))

    let app : WebPart =
        choose 
            [ path "/stream" >=> handShake websocket
              GET >=> choose
                [ path "/" >=> pageOrContent "Index" "/"
                  path "/index" >=> pageOrContent "Index" "/"
                  path "/test" >=> pageOrContent "Test" "/"
                  pathScan "/scripts/%s" (sprintf "./scripts/%s" >> file)
                  pathScan "/public/%s" (sprintf "./public/%s" >> file) 
                  pathScan "/css/%s" (sprintf "./css/%s" >> file)
                  pathScan "/fonts/%s" (sprintf "./fonts/%s" >> file)
                  path "/serviceworker.js" >=> file "./public/serviceworker.js" ]
              POST >=> choose
                [ path "/test" >=> request (fun req -> socketMessage.Send "{ \"action\": \"new post\", \"data\": \"Test passed!\"  }"
                                                       OK "") ]
            ]