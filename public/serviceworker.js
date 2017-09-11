// our caches
const mainCache = 'js-playground-cache-8';
const runtimeCache = 'runtime';
const currentCaches = [mainCache, runtimeCache];

// resources/routes we always want to cache
const precachedResources = [
    "index.html",
    "css/font-awesome.min.css",
    "css/reset.css",
    "scripts/ajax.js",
    "scripts/main.js",
    "scripts/sse-ie.js",
    "scripts/sse.js"
];

// resources/routes we never want to cache
const ignoredResources = []

const eventStreams = [
    "stream"
]

self.addEventListener('install', event => {
    // the browser is installing our serviceWorker
    event.waitUntil(
        // when done, cache all our initial resources
        caches.open(mainCache)
        .then(cache => cache.addAll(precachedResources))
        .then(self.skipWaiting())
    );
});

self.addEventListener('activate', event => {
    // new cache has been activated, so we can remove the old ones
    event.waitUntil(
        caches.keys().then(cacheNames => {
            // get all caches that are not currently in use
            return cacheNames.filter(cacheName => !currentCaches.includes(cacheName));
        }).then(cachesToDelete => {
            // remove the unused caches
            return Promise.all(cachesToDelete.map(cacheToDelete => {
                return caches.delete(cacheToDelete);
            }));
        }).then(() => self.clients.claim())
    );
});

function clearCache() {
    caches.keys().then(cacheNames => {
        return caches.delete(cacheNames);
    })
}

self.addEventListener('fetch', event => {
    // immediatly return a call to an event stream
    if (eventStreams.reduce((prev, curr) => prev || event.request.url.endsWith(curr), false)) {
        return fetch(event.request);
    }

    if (event.request.url.startsWith(self.location.origin) &&
        !event.request.url.startsWith("http://localhost")) {
        // if a fetch took place for our site
        event.respondWith(
            // check for a cached response and send it if it exists
            caches.match(event.request).then(cachedResponse => {
                if (cachedResponse) {
                    return cachedResponse;
                }

                // otherwise make the call and cache it
                return caches.open(runtimeCache).then(cache => {
                    return fetch(event.request).then(response => {
                        // ignore posts and other routes that don't always return the same result
                        const shouldBeIgnored = event.request.method === 'POST' ||
                            ignoredResources.reduce((prev, curr) => prev || event.request.url.endsWith(curr), false);
                        if (shouldBeIgnored) {
                            // return the response without caching
                            return response;
                        } else {
                            try {
                                // cache first, then return the response
                                return cache.put(event.request, response.clone()).then(() => {
                                    return response;
                                });
                            } catch (err) {
                                // if we can't cache it we return it as is
                                return response;
                            }
                        }
                    });
                });
            })
        );
    }
});