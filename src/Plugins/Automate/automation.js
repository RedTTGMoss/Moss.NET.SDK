let counter = 0;
on("import")
    .do(md => move(md, Config.inbox));

every("second")
    .where(_ => counter % 2 == 0)
    .do(() => log(counter++));