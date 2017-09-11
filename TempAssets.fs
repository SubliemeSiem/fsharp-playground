namespace FSharpPlayground

module TempAssets =
    let content page : string =
        match page with
        | "Index" -> "            <div><h1>Content test</h1>This content will eventually come out of a database.</div><div><h1>Code block test</h1><div class=\"code-block\">F#<br/> |> I <i class=\"fa fa-heart\"></i></div></div>"
        | "Test" -> "            <div onclick=\\\"ajax.post('/test');\\\">Succes!</div>"
        | _ -> "The page you're looking for does not exist."

    let messages : string list =
        [ "<h1>Test</h1>First test using F#"
          "<h1>Test</h1>Also a test"]