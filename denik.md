## 25.8.2024

### DONE:
- Dokoukal jsem intro to Front-end a Back-end development in .NET 
- Premyslim, zda aplikaci vest zpusobem Model-View-Controller, kde bych mel Blazor frontend a .NET Core backend, ktere by spolu komunikovali pomoci API
- Problem: template na takovouto aplikaci existuje jen pro .NET 7
- Nainstaloval jsem si dotnet pres brew: brew install --cask dotnet-sdk
    - ta instalace se mi propojila s mym dotnet commandem v terminalu

## 26.8.2024

### TODO:
- Zjistit, jak spojit Blazor frontend a .NET Core backend pro .NET 8
- Zprovoznit jednoduche vykreslovani z me databaze na MySQL

### DONE:
- Zakladam jednu solution a dva separatni projekty, ktere pak spojim do te jedne solution
  - Spojeni do jedne solution se dela commandem `dotnet sln add Server/Server.csproj` a `dotnet sln add Client/Client.csproj`
- Vyrobim si shared class library abych mohl sdilet kod svych modelu (napriklad Participant)
  - Pridam reference na tuto shared library
- Vyrobit model Participant v Shared folderu
- Vyrobit controller Participants, ktery zatim vraci random participanty podle meho modelu Participant
- V serverovem Program.cs odstanit kod kteremu nerozumim a pridat kod, ktery namapuje vsechny controllery

### Problem1:
Z nejakeho duvodu nefungovalo spojeni Clientskych razor pages a Serveroveho spousteni. 

Nutne commandy:
1. `app.UseBlazorFrameworkFiles();` - jinak se vubec nenacte blazorove okno
2. `app.MapFallbackToFile("index.html");` - jinak nefunguje refresh stranky
3. `app.UseStaticFiles();` - jinak se nacte stranka, ale nemuze pouzivat .css styly ani nema pristup k index.html

Je nutne pridat do Server projektu referenci na ten Client.csproj, aby mohl server najit ty Blazerove stranky.

! Je rozdil mezi Blazor page a Razor page! Takze neni nutne vubec pouzivat Razor stranky a setupovat pro ne service a routes

### Problem2:
Nefungoval dotnet watch (connection to browser is taking too long)

Reseni bylo v serverovem `launchSettings.json` upravit polozku `"applicationUrl"` na jiny port na localhostu v profilu http. Nevim, proc tomu tak je.

## 27.8.2024

### TODO: 
- Naplanovat konkretni dny a co udelat (treba do Google kalendare)
- Zprovoznit vykreslovani z databaze, kterou mam na Admineru
- Zprovoznit ukladani do databaze, kterou mam na Admineru

- Komunikace s controllerem in memory - post, get 

### DONE:
- Do Google Cal jsem si zapsal kratky plan
- Testuji post metodu na controlleru - zjistil jsem, ze controller pri kazdem POST requestu vytvari novou instanci
- Komunikace klienta s backendem pomoci metody post (vytvareni ucastniku)
  - Pouzit dependency injection abych mohl pouzivat Http class k sendovani requestu
  - Pouzit funkci `PostAsJsonAsync`
- Vypisovani tabulky ucastniku v klientovi
  - Dependency injection
  - `GetFromJsonAsync`

### PROBLEM1:
- Nefunguje spojeni s MySQL databazi
- Duvod je nejspis to, ze je potreba se pripojovat z rotundy, coz ale delat nechci

### Azure database creation
- Vyuzil jsem studentsky plan, a vyrobil jsem si databazi + server
- Development workload environment
- Zpusob autentikace jsem dal jednoduse SQL Authentication
- locally redundant storage
- Bylo potreba povolit svoji IP na koleji

### NEW TODO:
- naucit se pridavat do Azure databaze interaktivne veci pres vs code nebo v browseru
- propojit server s moji Azure databazi
- okomentovat svuj kod
- jak zmenim tabulku v databazi? staci upravit model a zavolat dotnet ef udpate?

### NEW DONE:
- Komunikovat interaktivne skrz SQL dotazy lze v te sekci databaze v polozce `Query editor`
- Vyzkousel jsem komunikovat s databazi pomoci `SqlConnection` tridy z `System.Data.SqlClient` 
- Po spusteni dotnet ef database update se mi automaticky vyrobila database dle meho `DbContext.cs` a moji entity `Participant.cs`


### Entity
- K pripojeni k databazi pouzivam Entity Framework, ktery umoznuje, ze muj Controller dostane v konstruktoru DbContext pomoci dependency injection a muze ho potom pouzivat
- Entity samotna je chytra, jelikoz se dokaze spojit s mou databazi a vytvorit pozadovane dabulky dle mych modelu
- `dotnet ef migrations add InitialCreate` a pote `dotnet ef database update`

### EVENING TODO:
- Pridat dalsi atributy do tabulky
- Umoznit upravovat udaje ucastniku skrze dalsi Blazor stranku

### EVENING DONE:
- Upravil jsem model Participanta - atributy nejsou required v instancich, ale v databazi ano, stringy jsou nullable
- Smazal jsem stare migrations a vytvoril nove
- Pouzil jsem reflection, aby byl muj kod nezavisly na jmenech atributu
  - To ma mozne nevyhody:
    - reflection je pomala a prasacka
    - stejne musim pro jednotlive parametry mit custom constraints, ktere jen tak neodvodim
  
### TIP:
- Testovat Blazor stranku se neda lehce pomoci printu ale staci pri spustenem dotnet watch vytvorit promennou, kterou jen vypisu do html a sleduji, co se v ni objevuje (misto tisknuti pisu do teto promenne)
  
## 28.8.2024

### TODO: 
- [x] Pridat stranku na update informaci o ucastnikovy dle id
- [ ] Zprovoznit button, kterym kliknu na ucastnika a budu moci upravit prave jeho udaje
- [x] Vymyslet, jak zprovoznit constraints bez pouziti prasacke reflection

### MORNING TODO:
- [x] Vytvorit z formulare  na zadavani informaci samostatnou komponentu

### MORNING DONE:
- Prevadim formular, aby fungovala validace pomoci EditForm, kterou poskytuje Blazor
  - Tim se zbavim Reflections
- Bylo potreba zalozit model pro Client side participanta, ktery slouzi k anotaci dat tim zpusobem, aby je pak mohl Blazerovsky EditForm validovat
- Zprovoznil jsem validation pomoci EditForm a custom constraintu na BirthNumber
- HTML je bohuzel hardcodnute, ale nevim, jak bych to mohl udelat lepe

### MORNING PROBLEM:
- Nejprve jsem omylem reagoval na event OnInvalidSubmittion a dlouho polemizoval, proc to funguje blbe
- Dale jsem si neuvedomil, ze je mozne submitnout jak enterem, tak i tlacitkem a mel jsem tam duplicitni zaznamy, nejlepsi je informovat uzivatele o successful submittion
- Muj custom constraint na edit form nefungoval spravne, boxik svitil zelene i kdyz constraint nebyl splnen
- Resenim bylo pouzit jiny overload metody IsValid u meho custom AttributeValidatoru
  - Myslim si, ze to maji v .NETu spatne, tak by bylo dobre to pak nahlasit

### Afternoon TODO:
- [x] Remake the component so it fires its own events (use EventCallback)
- [x] Zprovoznit editaci ucastniku dle id
- [x] Pridat event, ktery mohu hooknout na OnInvalidSubmit
- [x] Zmenit vypisovani ucastniku, aby vyuzivalo Blazorovskou QuickTable
- [ ] Add buttons to view single participant into QuickTable
  
### Afternoon DONE:
- Upravil jsem komponentu s validovanym formularem 
  - aby vysilala vlastni event pri uspesne validaci
  - abych se na hodnotu ucastnika z jejich policek zadanych uzivatelem mohl bindnout a pouzivat ho zvenci
  - pomoci paired tagu se da nastavit text na buttonu (je dobre ho nastavit jako required?)
- Participanty vypisuji pomoci QuickGridu, ale nevim, jak ten grid udelat hezky 
  - ukazovat na buttonku, zda sortime vzestupne/sestupne atd.
  
## 29.8.2024

### TODO:
- [x] Upravit quick grid, aby vypadal hezky
  - [x] Hezke buttonky
  - [x] Ukazoval vzestupne/sestupne
- [x] Pridat do quick gridu buttonky na update ucastniku
- [x] Pridat button na add participant do sekce participants a vymazat sekci add participant
- [x] Zprovoznit filterovani cisel v quick gridu
- [x] Zprovoznit vyhledavani v quick gridu
- [x] Pridat remove button do quick gridu
  - [x] Pridat confirmation modal box na odstraneni ucastnika
  - [ ] Udelat ten modal box aby prekryval zbytek
- [ ] Vytvorit jednoduchou kostru pro sekci meals
  
### DONE:
- Z nejakeho duvodu, horni buttonky quick gridu pres ktere se sorti uz vypadaji v pohode (ukazuji sipecku)
- Pridal jsem buttonky na update ucastniku 
  - pouzil jem TemplateColumn, ktera pri kliknuti zavola funkci NavigateToEditPage s Id od contextu (context je ucastnik)
  - K navigaci je treba injectnout si NavigationManagera, abych mohl chodit na dynamicky vygenerovany link
- Filterovani v quick gridu
  - Funguje tak, ze jako Items nastavime vyfilterovanou IQueryable, kterou pak v getteru filterujeme
  - Jednotlive filterovaci inputy pak nabindujeme k odpovidajicim promennym tech filteru
  - Vytvoril jsem interface pro tridy, ktere poskytuji filterovani na participantech
  - Cisleny range
    - Pridal jsem tridu, ktera filteruje ciselny range, od ni si pak mohu vyrobit filter pro custom pocatecni range

### Afternoon DONE:
- Pridal jsem button na deletovani
  - Nejprve bylo potreba pridat moznost deletovani dle id do meho api controlleru
  - Pote stacilo zavolat tuto api metodu z klienta
- Pridal jsem komponentu, ktera reprezentuje confirmacni boxik tykajici se akce na ucastnikovi
  - Je mozne dovnitr zadat text
  - Ta komponenta pote vysila event do ktereho preda id ucastnika
  - Komponenta si pamatuje take jmeno, aby mohla vytisknout jmeno toho ucastnika, ktereho se chystame odstranit
- DULEZITE: je mozne debuggovat v Clientovi, Console.WriteLine() se objevuje v console v inspect toolbaru browseru
- Aby byl boxik hezci, tak jsem pouzil css styl a flexboxy