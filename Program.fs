namespace FSharpPlayground

open System
open System.Threading
open Suave

open Routing

module Program =
    
    [<EntryPoint>]
    let main argv =
        let cts = new CancellationTokenSource()
        let conf = { defaultConfig with 
                         bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" 4000 ]; 
                         listenTimeout = TimeSpan.FromMilliseconds 3000.;
                         cancellationToken = cts.Token }
        let listening, server = startWebServerAsync conf app
        try
            Async.Start(server, cts.Token)
            printfn "Listening on port 4000"
            printfn "Press any key to quit"
            Console.ReadKey true |> ignore
        finally
            cts.Cancel() |> ignore
        0