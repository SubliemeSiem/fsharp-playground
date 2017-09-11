namespace FSharpPlayground

open Suave
open Suave.Files
open Suave.Filters
open Suave.Operators
open Suave.Successful

open PageFactory
open StaticAssets
open TempAssets
open WebSocket

module Routing =
    let pageOrContent (link : string) =
        request (fun req -> match req.queryParam "onlyContent" with
                            | Choice1Of2 onlyContent -> OK (contentResponse (title link) link (content link))
                            | _ -> OK (page link "/" scripts styleSheets inlineStyle links (content link) messages))

    // handshake opslaan
    // index teruggeven met response?
    // meegeven met requests
    let app : WebPart =
        choose 
            [ path "/stream" >=> handShake websocket
              GET >=> choose
                [ path "/" >=> pageOrContent "Index"
                  path "/index" >=> pageOrContent "Index"
                  path "/test" >=> pageOrContent "Test"
                  pathScan "/scripts/%s" (sprintf "./scripts/%s" >> file)
                  pathScan "/public/%s" (sprintf "./public/%s" >> file) 
                  pathScan "/css/%s" (sprintf "./css/%s" >> file)
                  pathScan "/fonts/%s" (sprintf "./fonts/%s" >> file)
                  path "/serviceworker.js" >=> file "./public/serviceworker.js" ]
              POST >=> choose
                [ path "/test" >=> request (fun req -> socketMessageEvent.SendMessage "{ \"action\": \"new post\", \"data\": \"Test passed!\"  }"
                                                       //socketMailBox.Post (AddToQueue (Choice1Of2 "{ \"action\": \"new post\", \"data\": \"Test passed!\"  }"))
                                                       OK "") ]
            ]