every("minute")
    .name("wiki")
    .do(function() {
        var url = `https://en.wikipedia.org/api/rest_v1/page/random/summary`;

        log("Fetching article of the day from " + url);
        var request = new HttpRequest(url);
        request.method = "GET";

        var result = request.send();
        var output = new Base64();
        var writer = new EpubWriter();

        writer.AddChapter("Chapter 1", render({hello: "world"}, "some '{{hello}}' text"));

        writer.Write(output);

        var notebook = newEpub(result.json.title, output);
        notebook.MoveTo(Config.inbox);
        notebook.Upload();
    });