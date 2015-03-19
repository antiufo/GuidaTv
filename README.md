# Guida TV [abbandonato]

Guida TV è un'applicazione per Windows che consente di consultare la lista dei programmi televisivi direttamente dal proprio desktop. I programmi sono disposti in ordine temporale, ed è possibile aggiungere regole personalizzate per evidenziare in automatico o nascondere determinati programmi, serie TV o generi di film.

Per molti film è inoltre disponibile la trama ed il trailer, oltre ovviamente a informazioni come genere, anno e valutazione.

Questo codice non è più mantenuto. In particolare, il connettore per MyMovies.it non è più funzionante (a maggio 2020).

## Contribuire al codice
Se qualcuno fosse interessato a rimettere l'applicazione in sesto, alcune note:
* Il connettore per MyMovies va aggiustato per funzionare sull'attuale layout del sito.
* Versioni aggiornate delle librerie in `References/` si trovano su NuGet o nei vari repository `Shaman.*` sul [mio profilo GitHub](https://github.com/antiufo). Per le altre, vedere [IlSpy](https://github.com/icsharpcode/ILSpy/) o simili.
* `BackgroundOperationEx` andrebbe sostituito con `async`/`await`.
* `Movies.dat` contiene il genere dei film presenti su MyMovies (generato con `MyMoviesDatabaseDump`). Non è stato aggiornato dal 2012.
