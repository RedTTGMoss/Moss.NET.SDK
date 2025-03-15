let counter = 0;
on("import")
    .do(md => move(md, Config.inbox));

every("second")
    .where(_ => counter % 2 == 0)
    .do(() => log(counter++));

every("day")
    .do(function() {
            let formattedDate = new Date().toISOString().split('T')[0].replace(/-/g, "/");
            let url = `https://api.wikimedia.org/feed/v1/wikipedia/en/featured/${formattedDate}`;

            log("Fetching article of the day from " + url);
            let request = new HttpRequest(url);
            request.method = "GET";

            let result = request.send();

            log(result.json().tfa.title);
    });