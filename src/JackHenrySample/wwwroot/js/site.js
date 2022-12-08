(function () {
    let connection = new signalR.HubConnectionBuilder()
        .withUrl('/hub/tweets')
        .withAutomaticReconnect()
        .build();
    let list = document.getElementById('tweets');
    let hashtags = document.getElementById('hashtags');
    let emoji = document.getElementById('emoji');
    let startstop = document.getElementById('startstop');
    let running = true;

    startstop.onclick = function () {
        running = !running;
        startstop.innerText = running ? '⏸' : '▶';
        if (running)
            start();
        else
            stop();
    }

    function span(text, className) {
        let e = document.createElement('span');
        e.className = className;
        e.innerText = text;
        return e;
    }

    function badge(s, className) {
        let p = span('');
        p.classList.add('badge', className, 'rounded-pill', 'me-2');
        p.appendChild(span(`#${s.key}`))
        p.appendChild(span(`: ${s.value}`, 'float-end'));
        return p;
    }

    connection.on('tweet', function (tweet) {
        let li = document.createElement('li');
        list.appendChild(li);
        li.textContent = `${tweet.id}: ${tweet.text}`;
        if (list.childElementCount > 1000) {
            list.removeChild(list.firstChild);
        }
    });
    connection.on('stats', function (stats) {
        hashtags.innerHTML = '';
        stats.hashtags.forEach(s => {
            hashtags.appendChild(badge(s, 'bg-primary'));
        });

        emoji.innerHTML = '';
        stats.emojis.forEach(s => {
            emoji.appendChild(badge(s, 'bg-secondary'));
        });
    });
    function start() {
        connection.start().then(function () {
            console.log('Connected');
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }
    function stop() {
        connection.stop();
    }
    start();
})();