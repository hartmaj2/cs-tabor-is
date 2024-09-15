# Coding Diary

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

## 30.8.2024

### TODO:
- [x] Vytvorit entity pro jidla, alergeny a objednavky dle ChatGPT
- [x] Vytvoti meals controller, ktery bude respondovat na get requesty ohledne meals
  - [x] Register new allergen - save the allergen to database (name, id chosen automatically)
  - [x] Create meal - send meal and allergens
  - [ ] Get meals for a certain day 


### DONE:
- Vytvoril jsem entity dle ChatGPT - je nutne, nezapomenout, ze je potreba pridat odpovidajici DbSety do DbContextu!
- Z entit se mi automaticky vytvorili databaze
- Zakladam nove kontrollery - na alergeny a na jidla
- U jidla jsem nastavil Json serialization aby se mi enumy psali jako stringy 
- Chtel jsem, abych pri postovani requestu na jidla mohl zadavat seznam allergenu
  - bylo potreba vytvorit tzv. DTO, ktery reprezentoval data, ktera bude posilat klient, server si pak tato data prevede na svoji vnitrni tabulkovou reprezentaci, ve ktere jidla nemaji seznamy alergenu ale tato relace je vyjadrena separatni tabulkou


## 2.9.2024

### TODO:
- [x] Upravit datum, aby nebylo potreba hodiny (staci mi jenom den)
- [x] Implementovat vraceni meals jen pro urcity den (v Meals controlleru)
- [x] Otestovat vraceni meals pro urcity den
  - [x] Pridat funkci na automaticke pridani nekolika jidel s ruznymi datumy
  - [x] Pridat odpovidajici request

### DONE:
- Bylo potreba zmenit int v testovani delitelnosti na long, protoze jinak se nejaka rodna cisla nevesla do intu
- Stacilo zmenit DateTime na DateOnly a pote nastavit novou migration a Entity Framework mi vhodne opravil databazi
- Request na ziskani vsech jidel a seznamu alergenu s kazdym jidlem je tezsi, seznam alergenu totiz neni primo soucasti objektu a musi se vycist z dalsi tabulky, coz potencialne vyzaduje dalsi query
  - Reseni je: pri ziskavani jidel si rovnou eager loadnout i jejich seznam MealAllergens a pro MealAllergen jeho konkretni allergen, at s nim pak mohu pracovat 
- Request na ziskani jidel pro konkretni den
- Request na pridani mnoha ruznych jidel (vygenerovane ChatGPT)  

## 3.9.2024

### TODO:
- [x] Create food page 
- [x] Create inner nav bar on top to choose between two subsections - menu, diets
  - [x] Create the top nav bar
  - [x] Add navigation buttons to the top bar
  - [x] Add pages that use the top nav bar
- [x] Make side navbar food icon active also when on food/diet
- [x] Create component for date selector
- [x] Make different foods appear on menu page based on the current date selected
- [ ] Make component that holds food of given MealTime (Lunch,Dinner)
  - [ ] Has button to add new food of component MealTime - will work through a modal this time
  - [x] Inside the component make the foods sorted (Soup,Main)
  - [ ] Button to edit/remove given food (edit name/allergens)

### DONE:
- Request to delete all meals - deletes also all MealAllergens as well
- Code to set focus on first form element after first render
- Creating a layout inside a layout
  - Has its own .css file
    - Name must be same as the razor file + .css at end
    - For Blazor to detect the file, rebuild the project
  - Top bar - when adding a border don't forget border-width, border-style and border-color
- Nav link ma Match attribute - umoznuje nastavit, zda je active pokud se namatchuje prefix/cela cesta
- Komponenta - ktera umoznuje prepinat mezi datumy, je mozne se na ni bindnout (CurrentDate)
- Nacitani jidel pomoci api requestu, kdyz se zmeni current date - je potreba upravit ToString toho DateOnly na "yyyy-MM-dd"
- Added passable additional attributes to DateOnlySelector (so I can align it to the middle)
- Added a MealTimeContainer component 
  - pass DateOnly CurrentDate, MealTime MealTime and IQueryable of MealDtos
  - added dummy buttons for adding new meal, editing and removing meals

## 4.9.2024

### TODO:
- [ ] Improve MealTimeContainer
  - [x] Make columns with buttons appear at very right
  - [ ] Make buttons work
    - [ ] Add modal that lists all allergens after button click
    - [ ] Add modals with input forms and verification
- [ ] Remove old modal and use the Bootstrap version
  - [x] Remove .css styles for modal
  - [ ] Edit code in Participants.razor page
- [ ] Fix style issue after removing `wwwroot/css/bootstrap` folder and `<link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />` link in index.html 

### DONE:
- Used div with flex instead of quick grid so I can align the buttons to the right (each row is a flex box and columns are items)
- Add id to meal dto so I can use it to delete it from database (the db needs the id for find function)
- Put styles for MealTimeContainer to separate .css file (some hard coded styles could be replaced by a rule that applied the same style to multiple components with the same class/relation to parent component)
- Tried to use Blazor.Bootstrap package so I can use premade modals - for some reason I am getting an error with `Could not find 'window.blazorBootstrap.modal.initialize'` but when I use `JSRuntime.InvokeVoidAsynx("blazorBootstrap.modal.initialize")` in overriden `OnAfterRenderAsync` then it works

## 5.9.2024

### TODO:
- [x] Fix styling after removing `wwwroot/css/bootstrap` folder
- [x] Implement modal to delete participant using Blazor Bootstrap package
- [x] Create add food modal skeleton
  - [x] Create modal to display all allergens after clicking
  - [x] Make the allergens into checkboxes so you can pick what allergens the food has
  - [x] Pass possible meal types to Client from Server using api request
- [x] Make add food modal work
  - [x] Connect labels to their corresponding inputs
  - [x] Connect meal type radio buttons as one group
  - [x] Use edit form to validate inputs (validate name and type were entered)
  - [x] Create new food based on validated meal form data object (modal will get passed the meal time on appearance)
- [ ] Display allergens next to Meal Type in MealTimeContainer
- [x] Implement Delete meal button
- [ ] Implement Edit meal button

### DONE:
- Fixed nav-link style by adding .css style selector in app.css file (needed to add padding left and right)
- Implemented modal delete participant modal
  - Add .modal-div and .modal-buttons selectors to display modal content aligned to center
  - Need to pass parameters using a Dictionary<string,object>
- Added http get method to get all possible meal types (used by the add food modal to display possible meal types to select)
  - Done using `Enum.GetNames<MealType>()` and is passed as a list of strings
- Created a list of AllergenSelecion items so I can bind allergen names to the values of the checkboxes
  - To be able to bind to a list of something, it can't be IEnumerable but must be a concrete List or Array or something
- Bound radio buttons to single group using same name attribute value of their input tags
- Added `MealFormData` class so I can use EditForm to validate inputs to my AddFoodModal
- Made EditForm that uses MealFormData as its model
  - Had to edit .css file to remove annoying green outline around radio buttons
- Made submit button inside EditForm work, the request method for a given MealFormData object is connected to its OnValidSubmit event
  - IMPORTANT: check the names of the EventCallback functions passed <= it is easy to make a mistake
  - Don't forget to pass some of the necessary properties to the Json request 
  - After submit I need to create new instance of the MealFormData object otherwise the validation won't reset
    - This means I also have to load the AllergenSelection list again after each submission
- Implemented remove button - basically same as last time
- Tried to implement edit button 
  - mostly works but allergens are not loading properly to the form
  - Also the text should change to Edit meal on the submit button when editing
  - Should try to reuse editMealModal instead of having both editMealModal and addMealModal


## 6.9.2024

### TODO:
- [x] Display allergens next to meal (also good for debugging)
- [x] Fix edit meal modal
  - [x] Fix allergens not loading
  - [x] Fix allergen choices not changing on edit
  - [x] Make both edit/add use the same modal reference
  - [x] Make edit modal have correct text
- [ ] Make participants table shrinkable (maybe allow hiding some of the columns)


### DONE:
- Made meal container display allergens
- Fixed error with non loading allergens - the problem was that I was overriding it with emtpy allergen selections over again (that needs to be done only when adding a meal not on edit)
- Fix error with changes not made to allergens in database - this is because the .Entry(entity).CurrentValues.SetValues(newEntity) only sets simple values (no associacion values)
  - The MealAllergens must be loaded and removed manually and then added manually as well
- Implemented column hiding
  - Didn't want to use reflection to go through possible properties because I might add some later
  - Unfortunately trick with binding to dictionary didn't work (bind to values for attribute names as keys)
  - Also, for (int i ...) loop is bad for binding because the last i value is used for binding (i think, because binding is done after the render maybe)

## 7.9.2024

### TODO:
- [x] Update participant model so participants have list of allergens
  - [x] Add ParticipantAllergen model 
  - [x] To participant add ICollection of MealAllergens
- [x] Probably make ParticipantDietDto where allergens will be a list of AllergenDtos
- [x] Move Dtos to their own folder and separate the classes into their own files

### DONE:
- Created ParticipantDietDto so I can transfer only relevant data when communicating diets to Client
- Tested git branching and merging so I can develop safer
- Changed ParticipantDietDto to ParticipantDto - the ParticipantFromData might have to change as well
  - The reason is consistency with how I communicate meals
- Migrating to a local database (so I don't have to connect online every time)
  - Need to install docker so I can run the mysql server inside a container (because I would need windows otherwise)
  - Accidentaly created MySql docker container but I need Sql
    - So I need to install sql express image
    - Remove the Migrations folder
    - Renamed ParticipantDbContext to TaborIsDbContext
- For some reason needed to fix request to create allergens (probably because first I had only allergens and mealallergens where added later so now I have to add MealAllergen empty list when creating new allergen)

## 8.9.2024

### TODO:
- [x] Implement communication using ParticipantDto instead of Participant directly
  - [x] Update http requests accordingly if necessary
- [x] Communicate AllergenDto instead of Allergens directly 
  - [x] Add Id to AllergenDto
  - [x] Update http requests accordingly
- [ ] Implement diet section
  - [x] Load a list of participant diet dtos 
  - [x] Show them in table that lists allergens 
  - [x] Implement sorting in my custom div table
  - [x] Add sort direction indicators to column headers
  - [ ] Implement filtering by checking which diets I want to see (use dropdown box with checkboxes)
    - [ ] Add div row with filtering options with text fields or dropdowns
    - [ ] Make dropdown with Blazor Bootstrap
- [ ] Work with ParticipantDto everywhere on client side
- [ ] Rename conversion methods from ConvertToSomething to ToSomething

### DONE:
- Added Id to AllergenDto and made Allergen controller to communicate using the dtos
- Changed all methods that use Participant to use ParticipantDto
  - Had issue with participant.Diets not loading (ParticipantAllergens) -> had to use .Include and .ThenInclude
- Also changed AllAllergens in client side to use AllergenDto instead of Allergen directly
- Renamed allergens to diets in participant dto and also added the diets to populate http request
- Diets section implemented by reusing my grid table .css style
  - Had to move the styles to app.css so I can use it everywhere
- Implemented diet editing in participant diet section
  - There was a mistake in my edit participant api endpoint method (.Find() was called without test predicate)
  - When editing another participant after edit of different participant, the checkboxes resemble still the old participants diets
    - Fix was to pass dietSelections to the component directly and not load them OnInitialize

## 9.9.2024

### TODO:
- [ ] Implement filtering by checking which diets I want to see (use dropdown box with checkboxes)
  - [ ] Add div row with filtering options with text fields or dropdowns
  - [ ] Filter using my interfaces I created for participant filtering
  - [ ] Make dropdown with Blazor Bootstrap
- [ ] Work with ParticipantDto everywhere on client side
- [ ] Rename conversion methods from ConvertToSomething to ToSomething

#### DONE:
- Made allergens and diets in div tables sorted for nicer UI
- Added ObjectSwitchableComparer abstract class to be used by ParticipantSorters
  - Abstracts reversing the sort
  - Has an abstract method that needs to be overriden by all members
  - All the comparers used by ParticipantSorters are used with the method on general objects (I couldn't figure out another way) so they must implement this general object switchable sorter and then when receiving the object cast them to their desired type and make the comparison

## 11.9.2024

### TODO:
- [x] Implement filtering by checking which diets I want to see (use dropdown box with checkboxes)
  - [x] Add div row with filtering options with text fields or dropdowns
  - [x] Filter using my interfaces I created for participant filtering
  - [ ] Make dropdown with Blazor Bootstrap
- [ ] Work with ParticipantDto everywhere on client side (fix this in Participants.razor)
- [ ] Rename conversion methods from ConvertToSomething to ToSomething

### DONE:
- Didn't want to use IQueryable in my filtering interface => created a new interface IParticipantFilter and moved the old one to IQueryableParticipantFilter
- Improved TextFilter to be able to filter all text properties of a participant using a key selector that returns string values
- Applied all filters using a fold

## 12.9.2024

### TODO:
- [x] Fix dropdown bind in Blazor Bootstrap
- [ ] Edit ParticipantFormData
  - [ ] Add DietSelections to ParticipantFormData
  - [ ] Make ParticipantFormData convert to ParticipantDto instead of Participant
- [ ] Fix adding new participants 
  - [ ] Add diet selections to participant form data class
  - [ ] Change conversion method to convert to participant dto
- [ ] Implement adding participants with diets

### DONE:
- Fixed the bind of the dropup box
  - The fix was to move the code directly to the Diets page instead of having a separate component, for some reason that makes the bind event not work oninput

## 14.9.2024

### TODO:
- [x] Edit ParticipantFormData
  - [x] Add DietSelections to ParticipantFormData
  - [x] Make ParticipantFormData convert to ParticipantDto instead of Participant
- [x] Fix adding new participants 
  - [x] Add diet selections to participant form data class
  - [x] Change conversion method to convert to participant dto
- [x] Implement adding participants with diets
- [x] Make columns in my div table sortable
- [ ] Add SubLayout to Participants also

### DONE:
- Tested, that the api part of adding/editing participants works (using my http requests)
- Implemented all conversion to Dto instead of the database model objects directly - all went smoothly
- Needed to add DietSelections to ParticipantFormData which in the end meant that I have to pass allergens list to conversion method that I use even when editing only other values of the participants (that could be fixed somehow)
- Removed the Home section from the Client app template
- Changed confirmation texts to "Confirm"
- Added smarter first and last name check
  - Used regexp
  - Allowed . or - in some cases 
- Switched to my div table instead of quick grid
  - Added style for columns whose content needs to be aligned to center 
  - Made my IntegerBoundFilter to work with any integer participant property
  - Deleted IQueryableParticipantFilter
- Implemented sorting in my div table 
  - Added IntegerSwitchableSorter
  - Copied rest of the code from diets page

## 15.9.2024

### TODO:
- [x] Get rid of warnings
- [x] Add SubLayout to Participants
  - [x] Rename SubLayout to FoodSubLayout
  - [x] Create ParticipantsSubLayout
- [ ] Add ParticipantMealOrders to database
  - [ ] Add DbSet and apply migration
  - [ ] Add method to place order to my api
- [x] Implement participant editing using Edit Modal
- [ ] Implement participant adding using Add Modal

### DONE:
- Got rid of warnings so I can see the actual warnings always
- Added Sublayout to participants
  - Needed to move .css scoped style to be global (needed to remove ::deep)
- Organized files 
  - Pages and Components according to section
  - Created DBModels folder
- Implemented editing participant using Edit Modal
  - Had to add Id to ParticipantFormData and conversion methods
  - Broken the port so had to change launchSettings