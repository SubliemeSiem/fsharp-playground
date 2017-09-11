namespace FSharpPlayground

open System.Text
open System.Threading
open Suave
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket

type SocketMessageEvent () =
    let socketMessage = new Event<string>()

    [<CLIEvent>]
    member this.Event = socketMessage.Publish

    member this.Send(message) =
        socketMessage.Trigger (message)

module WebSocket =
    let socketMessage = new SocketMessageEvent ()

    let (|Success|Failure|) result =
        match result with
        | Choice1Of2 a -> Success a
        | Choice2Of2 b -> Failure b
    
    // These choices don't contain any message yet
    let (|KeepAlive|Invalid|) msg =
        let message = Encoding.UTF8.GetString msg
        if (message = "keep-alive") then 
            KeepAlive 
        else 
            Invalid

    let response (message : string) = 
        message
        |> Encoding.ASCII.GetBytes
        |> ByteSegment

    let handleSocketError (error : Choice<'a, Error>) =
        match error with // TODO: handle these errors
        | Success a -> printfn "%O" a // Loop stopped early, but no warning. 
                                      // Should never happen. print result for debugging
        | Failure error ->
            match error with
            | SocketError systemSocketError -> ()
            | InputDataError (code, message) -> ()
            | ConnectionError message -> ()

    let websocket (webSocket : WebSocket) (httpContext : HttpContext) =
        socket {
            let sendKeepAlive () =
                webSocket.send Text (response "{\"action\": \"keep-alive\"}") true

            let rec listen (socket : WebSocket) = async {
                let! message = socket.read ()
                match message with
                | Success (_, KeepAlive, _) -> 
                    let result = Async.RunSynchronously (sendKeepAlive ())
                    match result with
                    | Success () -> return! listen socket
                    | Failure error -> return Choice2Of2 error

                | Success (_, Invalid, _) ->  // TODO: handle succesful read but invalid message
                    return! listen socket

                | Failure error -> return Choice2Of2 error
            }

            socketMessage.Event.Subscribe (fun message -> webSocket.send Text (response message) true
                                                          |> Async.RunSynchronously
                                                          |> handleSocketError) 
            |> ignore

            Async.RunSynchronously (listen webSocket)
            |> handleSocketError
        }