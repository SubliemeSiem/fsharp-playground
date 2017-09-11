    function openPage(page, setHistory) {
        // get the response of the route using AJAX
        ajax.get(page, { onlyContent: true }).then(function(succes) {
                // parse the JSON string to an object
                const response = JSON.parse(succes.response);

                // use the parsed response to fill the div with the content
                document.title = response.title;
                document.getElementById('page-title').innerText = response.title;
                document.getElementById('main-content').innerHTML = response.html;
                if (setHistory) {
                    history.pushState({ page: response.page }, response.title, `/${response.page}`);
                }
                toggleLinks(page);
            },
            function(failure) {
                alert(failure);
            }).catch(function(error) {
            alert(error);
        })
    }

    function toggleLinks(page) {
        page = page || "Index";
        document.querySelectorAll(`.pagelink`).forEach(function(x) {
            if (x.id.toLowerCase() === page.toLowerCase()) {
                x.classList.toggle("active", true);
            } else {
                x.classList.toggle("active", false);
            }
        });
    }

    function onPageLoad(newFunction) {
        if (window.attachEvent) {
            window.attachEvent('onload', newFunction);
        } else {
            if (window.onload) {
                var curronload = window.onload;
                var newonload = function(evt) {
                    curronload(evt);
                    newFunction(evt);
                };
                window.onload = newonload;
            } else {
                window.onload = newFunction;
            }
        }
    }

    window.addEventListener('popstate', function(e) {
        if (e.state) {
            console.log(e.state);
            openPage(e.state.page);
        }
    });

    onPageLoad(function() {
        const page = window.location.pathname.slice(1);
        console.log(page);
        history.replaceState({ page: page }, window.title);
        toggleLinks(page);
    });

    (function() {
        function waitUntilInstalled(registration) {
            return new Promise(function(resolve, reject) {
                if (registration.installing) {
                    // If the current registration represents the "installing" service worker, then wait
                    // until the installation step (during which the resources are pre-fetched) completes
                    // to display the file list.
                    registration.installing.addEventListener('statechange', function(e) {
                        if (e.target.state == 'installed') {
                            resolve();
                        } else if (e.target.state == 'redundant') {
                            reject();
                        }
                    });
                } else {
                    // Otherwise, if this isn't the "installing" service worker, then installation must have been
                    // completed during a previous visit to this page, and the resources are already pre-fetched.
                    // So we can show the list of files right away.
                    resolve();
                }
            });
        }

        if ('serviceWorker' in navigator) {
            // navigator.serviceWorker.getRegistrations().then(function(registrations) {
            //     for (let registration of registrations) {
            //         registration.unregister()
            //     }
            // });
            navigator.serviceWorker.register('serviceworker.js', { scope: './' })
                .then(waitUntilInstalled)
                .catch(function(error) {});
        }
    })();