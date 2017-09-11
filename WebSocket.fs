namespace FSharpPlayground

open System.Text
open System.Threading
open Suave
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket

type SocketMessageEvent () =

    let socketMessageEvent = new Event<string>()

    [<CLIEvent>]
    member this.MessageEvent = socketMessageEvent.Publish

    member this.SendMessage(arg) =
        socketMessageEvent.Trigger (arg)

type Action = 
        | ADD of WebSocket
        | REMOVE of WebSocket
        | SEND of string

module WebSocket =
    let socketMessageEvent = new SocketMessageEvent ()
    let response (message : string) = 
        message
        |> Encoding.ASCII.GetBytes
        |> ByteSegment

    let websocket (webSocket : WebSocket) (httpContext : HttpContext) =
        let cts = new CancellationTokenSource ()
        let socket = socket {
            let rec socketLoop (socket : WebSocket) (context : HttpContext) = async {
                let! message = socket.read ()
                match message with
                | _ -> 
                    let send = async {
                        let! result = socket.send Text (response "{\"action\": \"keep-alive\"}") true
                        match result with
                        | Choice1Of2 () -> ()
                        | Choice2Of2 _ -> ()
                    }
                    do! send
                    return! socketLoop socket context
            }

            socketMessageEvent.MessageEvent.Subscribe (fun message -> webSocket.send Text (response message) true
                                                                                        |> Async.RunSynchronously
                                                                                        |> ignore) 
            |> ignore
            
            let! result = socketLoop webSocket httpContext
            ()
        }
        socket