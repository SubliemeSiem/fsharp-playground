const ajax = (function() {
    "use strict";
    return {
        get: function(url, data) {
            // we return a promise for nice callback handling
            return new Promise(function(resolve, reject) {
                const xhr = new XMLHttpRequest();
                const query = [];
                for (const key in data) {
                    query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
                }
                const request = url + (query.length ? '?' + query : '');
                xhr.open('GET', request, true);
                xhr.onload = function() {
                    if (xhr.status === 200) {
                        resolve(xhr);
                    } else {
                        reject(xhr);
                    }
                };
                xhr.send();
            });
        },
        post: function(url, data, contentType) {
            // we return a promise for nice callback handling
            return new Promise(function(resolve, reject) {
                // if no contentType is set we use json
                contentType = contentType || 'application/json';

                const xhr = new XMLHttpRequest();
                xhr.open('POST', url, true);
                xhr.setRequestHeader('Content-Type', contentType);

                // when we get a response we handle the response
                xhr.onload = function() {
                    if (xhr.status === 200) {
                        resolve(xhr);
                    } else {
                        reject(xhr.response);
                    }
                };

                if (contentType === 'application/json') {
                    // encode data as json
                    xhr.send(JSON.stringify(data));
                } else if (contentType === 'application/x-www-form-urlendcoded') {
                    // encode data as URI query
                    const query = [];

                    for (const key in data) {
                        query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
                    }

                    xhr.send(encodeURI(query.join('&')));
                } else {
                    // send data 'as is'
                    xhr.send(data);
                }
            });
        }
    }
})();