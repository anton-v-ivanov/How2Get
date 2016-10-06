/**
 * JAL - Just Another Loader
 * Conditional resource loader for modern web browser.
 *
 * Licensed under the Apache V2 license.
 *
 * Copyright 2012, Tail-F Systems AB.
 */

(function(ctx, undefined) {
    var queue,              // Resource queue
        pollerId,           // Queue poller id
        queuePos,           // Points to current pos in queue
        readyListeners,     // Array of "ready" listeners
        holdLoad,           // Whether to postpone "ready" event
        db,                 // Debugger function
        wait,               // Stops loading until current resource group is loaded
        head,               // <HEAD> tag
        css,                // querystring param
        js,                 // querystring param
        qs,                 // querystring (in script source)
        loaderscript,       // optional async loader script
        d = document;

    queuePos = -1;
    holdLoad = false;
    readyListeners = [];
    readyFired = false;
    qs = d.getElementById('loader-script').src.replace(/^[^\?]+\??/,'').split(',')
    for (var i = 0, len = qs.length; i < len; i++) {
        if (qs[i] === 'debug') ctx.___DEBUG___ = true;
        else if (qs[i] === 'all') {
            css = true;
            js = true;
        } else if (qs[i] == 'css') {
            css = true;
        } else if (qs[i] == 'js') {
            js = true;
        } else if (qs[i].indexOf('load=') > -1) {
            loaderscript = qs[i].substring(5)
        }
    }
    queue = [];
    head = d.getElementsByTagName('head')[0];

    // If debug is enabled, write progress to console.
    if (ctx.___DEBUG___ && window.console) {
        db = function() {
            window.console.log(arguments)
        }
    } else {
        db = function() {};
    }

    // Load next resource on the queue
    function nextResource() {
        var res
        if (queue.length === 0) {
            fireReady();
        } else {
            res = queue.shift()
            db('#nextResource - loading next resource', res);
            loadResources(res);
        }
    }

    // Load a resource group.
    function loadResources(res) {
        var monitor,
            tag;
        monitor = createMonitor(res);
        for (var i = 0, i_len = res.res.length; i < i_len; i++) {
            tag = createTag(res.res[i], monitor);
            if (tag) head.appendChild(tag);
        }
    }

    // The monitor monitors the loading of the resource
    function createMonitor(res) {
        db('#createMonitor')
        var copy = res && res.res ? [].concat(res.res) : [];
        return function(resource, failed) {
            if (failed) {
                loaded(res, fail, resource);
                return;
            }
            for (var i = 0, i_len = copy.length; i < i_len; i++) {
                if (resource == copy[i]) {
                    copy = copy.slice(0, i).concat(copy.slice(i+1));
                    break;
                }
            }
            if (copy.length == 0) loaded(res);
        }
    }

    // Inject an html tag (script/link) into the document head so the
    // browser loads it.
    function createTag(resource, monitor) {
        var len,
            tag,
            state;
        // Javascript/coffescript
        if ((/.js$|.js\?|.coffee$|.coffee\?/i).test(resource)) {
            tag = d.createElement('script');
            tag.setAttribute('type', (/.js$|.js\?/i).test(resource) ? 'text/javascript' : 'text/coffeescript');
            tag.setAttribute('src', resource);
            tag.setAttribute('async', 'true');
            // The load handler triggers when the resource has been loaded.
            var _onload = function() {
                if (tag.readyState && tag.readyState != 'complete' && tag.readyState != 'loaded') return;
                monitor(resource);
                tag.onload = tag.onreadystatechange = null;
                if (tag.addEventListener) {
                    tag.removeEventListener('load', _onload)
                }
            }
            if (tag.addEventListener) {
                tag.addEventListener('load', _onload)
            } else {
                tag.onload = tag.onreadystatechange = _onload
            }
        } // CSS or LESS
        else if ((/.css$|.css\?|.less$|.less\?/i).test(resource)) {
            tag = d.createElement('link');
            tag.media = 'screen';
            tag.rel = (/.css$|.css\?/i).test(resource) ? 'stylesheet' : 'stylesheet/less';
            tag.type = 'text/css';
            tag.href = resource;
            monitor(resource);
        }
        return tag;
    }

    // Resource group loaded. Call listeners.
    function loaded(res, fail, failedResource) {
        var old = queuePos;
        if (res.done) {
            queuePos = 0;
            db('#loaded - calling done on: ', res)
            res.done(ctx.$loader);
        }
        // Restore queue position in case a resource was injected.
        queuePos = old;
        db('#loaded - wait = false')
        nextResource();
    }

    // Register a resource group to be loaded in parallel.
    function load(res, failCondition) {
        db('loader#load', res)
        if (res && res.length == 0) return ctx.$loader;
        if (typeof res == 'string') {
            res = [res];
        }
        res = {
            res: res,
            done: null,
            fail: null,
            failCondition: failCondition
        };
        if (queuePos == -1) queue.push(res);
        else {
            queue.splice(queuePos, 0, res);
            queuePos++;
        }

        if (queue.length === 1) {
            setTimeout(nextResource, 0);
        }

        return ctx.$loader;
    }

    // Triggers when the resource group has been loaded.
    function done(callback) {
        db('loader#done - queue len: ' + queue.length)
        queue[queue.length - 1].done = callback;
        return ctx.$loader;
    }

    // Triggers if the resource group, or a resource in the
    // resource group, failed to load.
    // *This event is not yet supported.*
    function fail(callback) {
        queue[queue.length - 1].fail = callback;
        return ctx.$loader;
    }

    // Call the 'callback' function if 'condition'
    // evaluates to true.
    function when(condition, callback) {
        if (condition) callback(ctx.$loader);
        return ctx.$loader;
    }

    // Set whether to delay triggering the "ready"
    // event or not.
    function holdReady(hold) {
        holdLoad = hold;
        return ctx.$loader;
    }

    // Trigger the "ready" event.
    function fireReady() {
        db("In #fireReady")
        if (holdLoad) setTimeout(fireReady, 10);
        else {
            db("#fireReady - notifying listeners")
            readyFired = true;
            for (var i = 0, len = readyListeners.length; i < len; i++) {
                readyListeners[i]();
            }
            readyListeners = [];
        }
    }

    // Register a listener to the "ready" event. If "ready" has been
    // fired, triggers immediately.
    function ready(listener) {
        if (readyFired) listener();
        else readyListeners.push(listener);
        return ctx.$loader;
    }

    // Export the loader interface.
    ctx.$loader = {
        load: load,
        done: done,
        fail: fail,
        when: when,
        css: css,
        js: js,
        holdReady: holdReady,
        ready: ready
    };
    // Load the loader script asynchronously
    if (loaderscript) {
        var tag = d.createElement('script');
        tag.setAttribute('type', 'text/javascript');
        tag.setAttribute('src', loaderscript);
        tag.setAttribute('async', 'true');
        head.appendChild(tag);
    }
})(this);

