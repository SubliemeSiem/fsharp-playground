(function() {
    const protocol = window.location.protocol;
    const currentUrl = window.location.href;
    const currentRoute = window.location.pathname;
    websocket = new WebSocket("ws://localhost:4000/stream");


    websocket.onopen = function(e) {
        setInterval(function() {
            websocket.send("keep-alive");
        }, 30000)
    };

    websocket.onclose = function(e) {

    };

    websocket.onmessage = function(e) {
        message = JSON.parse(e.data);
        if (message.action === "new post") {
            document.getElementById('main-content').innerHTML += `<div>${message.data}</div>`;
        }
    };

    websocket.onerror = function(e) {

    };

})();