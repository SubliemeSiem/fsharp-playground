# js-playground

A project to play around with *JavaScript* and *F#* using *Suave*. Will feature various common design patterns and libraries, and shows how you can host js, css and html files using Suave.

## Contents:
- [x] Suave
  - [x] [set up](Program.fs)
  - [x] [routers](Routing.fs)
- [ ] websockets
  - [x] set it up
    - [x] [client](scripts/webSocket.js)
    - [x] [server](WebSocket.fs)
    - [x] [routing](Routing.fs)
  - [x] [send from the server] (WebSocket.fs)
  - [ ] [read from the client] (scripts/webSocket.js)
- [ ] using a "let's encrypt" sll certificate and automatically updating it
- [ ] js DOM selectors (no jQuery)
  - [ ] by id
  - [ ] by tag
  - [ ] by class
  - [ ] first in a specified query
  - [ ] all in a specified query
- [x] AJAX calls without jQuery
  - [x] [set up script](scripts/ajax.js)
  - [x] [use](scripts/main.js)
- [x] [using Suave to host client resource files](Routing.fs)
- [ ] dynamically create a script block and serve it through a route as a file
- [ ] combining AJAX calls with Suave routing
  - [ ] get
    - [ ] client-side call
    - [ ] server-side route
  - [ ] post
    - [ ] client-side call
    - [ ] server-side route
- [ ] responsive css
  - [x] [stylesheet](StaticAssets.fs)
  - [x] [HTML views using responsive CSS](StaticAssets.fs)
- [ ] JSON models
- [ ] mongodb
- [ ] Promises
  - [x] [create](scripts/ajax.js)
  - [ ] use
- [x] [F# fold](PageFactory.fs)
- [ ] js map
- [ ] js reduce
- [ ] error handling
- [ ] logging
- [ ] minify and concatenate client scripts and style sheets
- [x] [add web app manifest](public/manifest.json)
- [x] add a favicon.ico
- [x] manipulate browser history
  - [x] [create history entries](scripts/main.js)
  - [x] [recover history entries](scripts/main.js)
- [x] add a service worker (for offline caching)
  - [x] [worker script](public/serviceworker.js)
  - [x] [installation](scripts/main.js)

## Running the code

You'll need to install dotnet Core 2.0 first.
Afterwards you can run the following commands in your shell to get and run the code:
```bash
$ git clone https://github.com/SubliemeSiem/fsharp-playground.git
$ cd fsharp-playground
$ dotnet restore
$ dotnet run
```
To visit the page, browse to http://localhost:4000. 
To see the mobile or tablet version, browse to http://[ip]:4000,
where [ip] is the ip address of the system running Node.js, using a phone or tablet that is connected to the same network.
To get ssl to work using let's encrypt, you'll need some sort of static address. This may be an address provided by a DDNS service, or a static ip address. Details about the configuration for let's encrypt can be found in (**not implemented yet**).