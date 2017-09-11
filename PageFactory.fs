namespace FSharpPlayground

module PageFactory =
    let scriptsBlock (scripts : string list) (path : string) = 
        List.fold (fun prev curr -> prev + "    <script src=\"" + path + curr + "\" async></script>\r\n") "" scripts

    let styleBlock (styles : string list) (path : string) = 
        List.fold (fun prev curr -> prev + "    <link rel=\"stylesheet\" type=\"text/css\" href=\"" + path + curr + "\" media=\"none\" onload=\"if(media!=='all')media='all'\" ></link>\r\n") "" styles

    let pageLinks (links : string list) (path : string) = 
        List.fold (fun prev curr -> prev + "            <div class=\"pagelink\" onclick=\"openPage('" + path + curr + "'.toLowerCase(), '" + curr + "', true);\" id=\"" + curr + "\">" + curr + "</div>\r\n") "" links

    let messageBlock (messages : string list) =
        List.fold (fun prev curr -> prev + "            <div class=\"message\">\r\n                " + curr + "\r\n            </div>\r\n") "" messages

    let contentResponse (title : string) (page : string) (html : string) =
        "{\"title\": \"" + title + "\", \"page\": \"" + page + "\", \"html\": \"" + html + "\"}"

    let page (title : string) (path : string) (scripts : string list) (styles : string list) (inlineStyle : string) (links : string list) (content : string) (messages : string list) = 
        String.concat "" ["""<html lang="en">
<head>
    <link rel="shortcut icon" type="image/x-icon" href="public/favicon.ico">
    <title>"""; title; """</title>
    <link rel="manifest" href="public/manifest.json">
"""; styleBlock styles path; """
    <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
    <meta name="theme-color" content="#5d1d1d"></meta>
"""; scriptsBlock scripts path; """
    <style>
"""; inlineStyle; """
    </style>
</head>

<body>
    <div id="script-block"></div>
    <div class="row">
        <div class="col-12 col-m-12 header">
            <span id="page-title">
                js & F# playground
            </span>
        </div>
    </div>
    <div class="row main-body">
        <div class="col-2 col-m-2 left sidebar">
"""; pageLinks links path; """
        </div>
        <div class="col-7 col-m-7 content" id="main-content">
"""; content; """
        </div>
        <div class="col-3 col-m-3 right sidebar">
"""; messageBlock messages; """
        </div>
    </div>
</body>

</html>
""" ]