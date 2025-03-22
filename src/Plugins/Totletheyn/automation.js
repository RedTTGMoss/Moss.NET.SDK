every "minute" do {
        log("hello from custom syntax");
} as log;

every "minute" do {
        var url = `https://en.wikipedia.org/api/rest_v1/page/random/summary`;

        var request = new HttpRequest(url);
        request.method = "GET";

        var result = request.send();
        var output = new Base64();
        var writer = new EpubWriter();

        writer.AddChapter("Chapter 1", render(result.json, "some '{{hello}}' text"));

        writer.Write(output);

        var notebook = newEpub(result.json.title, output);
        notebook.MoveTo(Config.inbox);
        notebook.Upload();
} as wiki;