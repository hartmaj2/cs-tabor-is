# Stručná dokumentace

## Obsah <!-- omit from toc -->

- [Anotace](#anotace)
- [Uživatelská dokumentace](#uživatelská-dokumentace)
  - [Spuštění programu](#spuštění-programu)
  - [Navigace v programu](#navigace-v-programu)
  - [Sekce Participants](#sekce-participants)
  - [Sekce Food](#sekce-food)
  - [Kritéria pro jednotlivé atributy účastníků](#kritéria-pro-jednotlivé-atributy-účastníků)
  - [Možnost přidání dalších alergenů](#možnost-přidání-dalších-alergenů)
  - [Možnost přidání dalších typů jídel](#možnost-přidání-dalších-typů-jídel)
  - [Zadávání objednávek](#zadávání-objednávek)
- [Programátorská dokumentace](#programátorská-dokumentace)
  - [Struktura solution](#struktura-solution)
  - [Server](#server)
  - [Client](#client)
  - [Shared](#shared)
  - [UnitTests](#unittests)


## Anotace

Program je informační systém, který slouží vedoucím tábora ke správě informací o účastnících tohoto tábora a také přidávání pokrmů na menu pro jednotlivé dny. Jedná se o webovou aplikaci, která má jak serverovou, tak klientskou část.

## Uživatelská dokumentace

### Spuštění programu

Jelikož Serverová část programu využívá ke správě dat, databázi, tak je nutné, mít v souboru **appsettings.json** (nachází se ve složce **Server**) v sekci `ConnectionStrings` správně nakonfigurovanou `DefaultConnection`. V `DefaultConnection` je potřeba zapsat všechny potřebné konfigurační údaje pro připojení k databázi, kterou chcete používat.

Program se spouští příkazem `dotnet run --project Server`, který kromě serveru automaticky po spuštění spustí i klienta. Po spuštění aplikace se v terminálu ukáže zpráva obsahující řádek `Now listening on: http://localhost:xxxx`, kde `xxxx` je číslo portu, na kterém server poslouchá. Danou adresu stačí zkopírovat do webového prohlížeče, skrz který nyní můžete s aplikací interagovat.

> [!NOTE]
> Port, na kterém server bude poslouchat lze nastavit v souboru **/Properties/launchSettings.json**.

### Navigace v programu

Web se dělí na sekce, které se dále dělí na podsekce. Po spuštění ve webovém prohlížeči se objevíte automaticky v sekci **Participants**. Vybírat mezi jednotlivými sekcemi můžete v levém panelu, kde se nachází kromě sekce **Participants**, také sekce **Food**. Právě aktivní sekci je vždy možné poznat pomocí bílého zbarvení pozadí tlačítka na levém panelu odpovídající dané sekci. Nacházíte-li se právě v nějaké sekci, tak přepínat mezi podsekcemi můžete pomocí horního panelu, který obsahuje klikatelný seznam podsekcí. Stejně jako u panelu se sekcemi poznáte aktivní podsekci bílým zbarvením pozadí tlačítka dané podsekce.

### Sekce Participants

Sekce **Participants** obsahuje pouze jednu podsekci jménem **All participants**.

#### Podsekce All participants

Podsekce **All participants** sestává z tabulky, která poskytuje seznam účastníků a jejich základních informací jako id, jméno, příjmení a další. 

Každý sloupec podporuje třídění záznamů v tabulce podle hodnoty atributu tohoto sloupce. Stačí kliknout na název daného sloupce. Na pravé straně se pak objeví šipečka, která značí, zda jsou záznamy setříděné vzestupně či sestupně. Vzestupně se značí šipečkou nahoru, kde jako první záznam bereme ten první od shora. 

Všechny sloupce také umožňují filtrovat záznamy v tabulce na základě určitých kritérií, týkajících se hodnoty záznamu v daném sloupci. Sloupce obsahující hodnoty textové povahy je možné filtrovat na základě zadaného textového řetězce. Tento filtrovací řetězec se zadává do textového pole s popiskem **search...**. V tabulce se pak ukáží jen takové záznamy, jejichž hodnota v daném sloupci obsahuje řetězec, který ja zadaný v textovém poli nehledě na velká či malá písmena. Sloupce, které obsahují hodnoty číselné povahy umožňují filtrovat kliknutím na tlačítko filter a následným zadáním dolní a horní meze, kterou si přejeme, aby měla hodnota všech vyfiltrovaných záznamů. Tyto meze se dají zadat buď pomocí posuvníků nebo exaktním zapsáním dané hodnoty do textového pole. 

> [!TIP]
> Navolené filtrovací hodnoty se dá jednoduše všechny zrušit kliknutím na tlačítko **Reset filters**. 

Přidávat nové účastníky do tabulky lze kliknutím na tlačítko **Add a participant**. Po kliknutí na něj se zobrazí dialogové okno, které na levé straně obsahuje textová pole na zadání základních údajů o účastníkovi. Program automaticky kontroluje, zda hodnoty zadávané do těchto polí dávají smysl. Pokud navíc uživatel do políčka **Birth Number** zadá platné české rodné číslo, tak se mu jeho věk spočítá automaticky. Rodné číslo je také možné úplně vynechat a zadat věk manuálně (např. pro cizince). Na pravé straně je pak možné pomocí zaškrtávacích políček navolit diety, které daný účastník má. Potvrdit volbu lze stisknutím klávesy Enter, nebo kliknutím na tlačítko **Confirm**. Pokud jsou nějaké ze zadaných hodnot neplatné, program daná políčka zvýrazní červeně a vypíše, co konkrétně je na nich špatně.

Účastníky v tabulce je možné editovat pomocí tlačítka **Edit**. Pomocí tohoto tlačítka je možné upravit hodnoty pouze těch atributů, které jsou vidět v tabulce. Pokud si přejete upravit diety účastníka, tak je nutné toto provést v podsekci **Diets**, která se nachází v sekci **Food**. Pomocí tlačítka **Delete** je pak možné účastníka úplně vymazat z databáze.

> [!WARNING]
> Operace vymazání účastníka je nevratná!

### Sekce Food

Sekce **Food** obsahuje dvě podsekce: **Menu** a **Diets**. Tato sekce obsahuje vše, co se tématicky týká pokrmů na táboře.

#### Podsekce Menu

Podsekce **Menu** obsahuje informace o tom, které dny jsou jaká jídla v nabídce na menu. V horní části ihned pod lištou s podsekcemi je možné přepínat datum, pro které chceme zobrazit jídelníček. Směrem do historie se přepínáme pomocí tlačítka označeného symbolem **<** nacházejícího se vlevo od nadpisu se zvoleným datem. Naopak pomocí tlačítka **>** lze posouvat datum směrem do budoucnosti.

Samotné denní menu je rozdělené na dvě tabulky nazývající se **Lunch** a **Dinner** a odpovídají obědu a večeři. Záznamy v tabulkách pak odpovídají jednotlivým pokrmům pro daný den a daný čas, kde časem je myšlen oběd či večeře. Každé jídlo má název, typ (vyjadřuje, zda se jedná o polévku či hlavní chod), alergeny v něm obsažené a počet objednávek, které učinili účastníci tábora. Allergeny v pokrmech přímo odpovídají možným dietám, které můžou účastníci mít, a jsou setříděné lexikograficky. 

> [!NOTE] 
> Pokrmy v odpovídajících tabulkách jsou řazeny nejprve dle pořadí chodu vzestupně (nejprve polévka, pak hlavní chod) a následně lexikograficky dle jména (také vzestupně).

Přidat nový pokrm do dané tabulky kliknutím na tlačítko **+** v pravém horním rohu odpovídající tabulky. Po kliknutí se zobrazí dialogové okno s popiskem odpovídajícím danému datu a času. Na jméno jídla nejsou kladeny žádné restrikce kromě toho, že nesmí být prázdné, což ocení především kreativní tvůrci jídelníčků. Typ jídla (polévka, hlavní chod) je však nutné zadat. Dále je pak možné libovolně navolit alergeny pomocí zaškrtávacích políček. Pokrmy lze mazat a upravovat stejně, jako to lze v podsekci **All participants** v tabulce s účastníky.

#### Podsekce Diets

Podsekce **Diets** obsahuje tabulku s účastníky, která zobrazuje jejich diety. Diety každého účastníka jsou setříděny lexikograficky. Sloupce podobně jako v podsekci **All participants** umožňují řazení a filtrování. Řazení sloupce s dietami funguje také lexikograficky a porovnávají se textové řetězce vzniklé zřetězení všech diet za sebou tak, jak jsou zapsány. V sloupci s dietami lze také filtrovat kliknutím na tlačítko **Filter diets** a zvolením diet, které chceme filtrovat pomocí zaškrtávacích políček. V tabulce se pak zobrazí pouze ti účastníci, kteří mají všechny ze zvolených diet (můžou jich však mít i více). Všechny filtrovací kritéria můžeme zrušit pomocí tlačítka **Reset filters** stejně tak, jako tomu bylo v podsekci **All participants**.

Diety účastníků je možné pomocí tlačítka **Edit diets** u daného účastníka.

### Kritéria pro jednotlivé atributy účastníků
  
#### ID

ID je účastníkům přidělováno automaticky databází, uživatel tedy nemá, jak ho změnit.

#### First name

Křestní jméno musí být neprázdný řetězec, který neobsahuje speciální symboly jako $,+ atd. Také nesmí obsahovat čísla. Křestní jméno se můze skládat z více slov. Poté musí být odděleno právě jendím oddělovačem. Oddělovačem může být mezera, pomlčka nebo apostrof. Tečka je povolena pouze na konci slova. Veškeré bíle znaky na začátku či na konci jména jsou automaticky odstraněny po jeho zadání.

#### Last name 

Pro příjmení platí totožná kritéria, jako pro křestní jméno.

#### Age

Věk je omezený od 0 do 70 let, jelikož se jedná o tábor, na kterém budou účastníky především děti a teoreticky mládež. Kdybych chtěl podporovat i vyšší věk, tak by to znamenalo, že mohu zadávat i osoby narozené před rokem 1954, pro které by nefungovalo automatické dopočítávání věku z rodného čísla. Rodná čísla měla totiž před rokem 1954 devítimístný formát.

#### Phone number

Telefonní číslo je nutné zadat a může být chápáno buď jako číslo na účastníka samotného nebo na jeho rodiče. Pokud se jedná o české telefonní číslo, tak může být zadáno buď bez předpony nebo s předponou 00420 či +420 a zbylá část musí obsahovat přávě devět cifer. Pokud je nutné zadat zahraniční telefonní číslo, tak je nutné, aby začínalo symbolem + a dále musí obsahovat 7 až 15 cifer. Číslo je možné zadat i s pomocnými symboly jako je pomlčka nebo závorky. Ty jsou však ignorovány a po zadání automaticky odstraněny.

#### Birth number

Rodné číslo není nutné zadat. Pokud se však uživatel rozhodne ho zadat. Pak musí jít o platné české rodné číslo. To lze zadat bez lomítka nebo i s ním. Lomítko se však musí nacházet mezi 6. a 7. cifrou počínaje zleva a čílsováno od 1. Po zadání je automaticky odstraněno.

> [!TIP]
> Po zadání platného rodného čísla se hodnota atributu Age dopočítá automaticky.

> [!WARNING]
> Program podporuje pouze desetimístná česká rodná čísla ve formátu odpovídajícímu rodným číslům vydaným po 1.1.1954.

### Možnost přidání dalších alergenů

Aplikace umožňuje také přidat další alergeny, pokud by aktuální volba alergenů nebyla dostačující. Toto však nejde provést interaktivně pomocí uživatelského rozhraní a je nutné poslat http request serveru. Tento json request musí být formátu:
```http
POST http://localhost:xxxx/api/allergens/add
Content-Type: application/json

{
    "Name" : "JmenoAlergenu"
}
```
kde `xxxx` je port, na kterém server běží a `JmenoAlergenu` je libovolný textový řetězec popisující daný alergen.

### Možnost přidání dalších typů jídel

Přidání dalších typů jídel (např. dezert) nějakým uživatelsky aspoň trochu přívětivým způsobem program zatím nepodporuje.

### Zadávání objednávek

Server také dokáže přijímat objednávky účastníků na jednotlivé pokrmy. Toto bude v budoucnu možné provádět pomocí uživatelského rozhraní v jiné klientské aplikaci. Otestovat tuto funkcionalitu však lze již nyní pomocí http reqestu následujícího formátu:
```http
POST http://localhost:xxxx/api/orders/add
Content-Type: application/json

{
    "participantId" : "IdUcastnika",
    "mealId" : "IdPokrmu"
}
```
kde `xxxx` je port, na kterém server běží, `IdUcastnika` je ID účastníka, který si daný pokrm objednává a `IdPokrmu` je ID pokrmu, který si účastník přeje objednat. 

> [!NOTE]
> Id pokrmů se sice v uživatelském rozhraní aplikace nezobrazuje, ale je možné si nechat vypsat všechny pokrmy včetně jejich Id pomocí http requestu: `GET http://localhost:xxxx/api/meals/all`. 


Pokud je http request na objednávku pokrmu úspěšný, bude v uživatelském rozhraní klientské aplikace v podsekci **Menu** v tabulce s denním menu, kde se nachází daný pokrm možno spatřit, že se počet objednávek daného pokrmu navýšil o jedna.

> [!TIP]
> Velmi uživatelsky přívětivé posílání requestů umožňuje ve VS Code rozšíření (Extension) jménem **REST Client**.

## Programátorská dokumentace

### Struktura solution

Solution se skládá ze tří klíčových projektů:
- [Server](#server) - backendová část aplikace, která se stará o komunikaci s databází skrze REST API
- [Client](#client) - frontendová část, umožňuje uživateli komunikovat se serverem pomocí uživatelského rozhraní
- [Shared](#shared) - zde se nachází data, která jsou sdílená mezi serverem a klientem (backendem a frontendem)

Dále solution obsahuje projekt [UnitTests](#unittests), ve kterém se nachází sada unit testů.

> [!NOTE]
> V repozitáři se nachází také soubor **denik.md**, který obsahuje chronologicky řazené záznamy popisující postupný vývoj programu. Dále se zde nachází také soubor **ideas.md** obsahující nápady na možná rozšíření programu do budoucna.

### Server

Projekt **Server** obsahuje tři adresáře s C# kódem:
- **Controllers** - obsahuje api kontrolery, které jsou všechny anotované atributem `[ApiController]` a dědí od třídy `ControllerBase`
- **Data** - obsahuje třídu, která dědí od `DbContext` a popisuje strukturu databáze pro Entity Framework
- **Migrations** - obsahuje kód automaticky vygenerovaný Entity Frameworkem

Dále projekt obsahuje soubor **Program.cs**, který je vstupním bodem pro celou aplikaci.

> [!TIP]
> V adresáři **TestRequest** se nachází http requesty, které se dají využít k otestování správné funkčnosti serverového api, nebo k rychlému zadání vstupních dat do databáze.

### Client

Projekt **Client** sestává z následujících adresářů/souborů:
- **Components** - obsahuje jednotlivé **razor** komponenty
  - **SectionFood** - komponenty týkající se sekce **Food** (dialogová okna, komponenta pro vybírání datumu, tabulka na pokrmy pro daný čas)
  - **SectionParticipants** - komponenty týkající se sekce **Participants** (dialogová okna, formulář na zadávání dat o účastnících)

> [!WARNING]
> Veškeré komponenty, které se nenachází přímo v adresáři **Components**, ale nachází se v nějakém podadresáři musí obsahovat direktiv `@namespace Client.Components`.
> V opačném případě při kompilaci obdržíme výjimku tvaru: `error CS0246: The type or namespace name 'ComponentName' could not be found (are you missing a using directive or an assembly reference?)`

- **Layout** - obsahuje **razor** soubory, které se týkají layoutu webu (sdílené více stránkami)
  - **MainLayout** - kód pro vrchní panel, v jehož prvé části je odkaz na tento github repozitář
  - **NavMenu** - menu, které umožňuje přepínat mezi jednotlivými sekcemi webu; obsahuje také kód, který umožňuje rozbalení a sbalení tohoto menu, pokud se web nachází v režimu pro malé obrazovky
  - **SubLayoutFood** a **SubLayoutParticipants** - menu pro přepínání mezi podsekcemi programu, každá sekce má svůj **SubLayout**

> [!NOTE]
> Od jaké šířky v pixelech se **NavMenu** přepne do režimu pro malou obrazovku lze nastavit v souboru **NavMenu.razor.css** pomocí selektoru `@media (min-width: 1001px)`

- **Pages** - stránky jednotlivých podsekcí a **cs** soubory obsahující pomocné třídy, které tyto sekce využívají
  - **SectionFood** - kód pro podsekce **Menu** a **Diets**
  - **SectionParticipants** - kód pro podsekci **All participants**
  - **MealSorting.cs** - obsahuje třídu implementující `IComparer` a umožňuje v podsekci **Menu**, aby byla jídla setříděná nejprve dle typu a poté dle názvu
  - **ParticipantFilters.cs** - poskytuje třídu `ColumnFilteringManager` a také konkrétní implementace interfacu `IParticipantFilter`, které umožňují filtrování v jednotlivých sloupců
  - **ParticipantSorting.cs** - obsahuje následující:
    - `ColumnSortingManager` - poskytuje funkcionality vhodné pro třídění sloupců tabulky s účastníky u kterých chceme mít možnost zadat, podle kterého atributu účastníka třídíme a jaký chceme používat porovnávač
    - `ParticipantSorter` - odpovídá jednomu sloupci, ve kterém chceme třídit; využíva ji `ColumnSortingManager`
      - drží delegáta, který po poskytnutí `ParticipantDto` poskytne klíč, podle kterého třídíme
      - také drží objekt typu `ISwitchableComparer`, který představuje porovnávač na daných klíčích, jehož výsledek lze obrátit nastavením property `ReverseSort`

> [!NOTE]
> `ParticipantSorter` je sice generický, ale tuto vlastnost zatím v programu nijak nevyužívám, jelikož `ColumnSortingManager` si drží seznam objektů typu `ParticipantSorter<object>` a všude jinde v programu používám také pouze tento odvozený typ.

- **Services** - obsahuje služby, které je možné použít pomocí **dependency injection** v jakékoliv komponentě
  - **AllergenService.cs** - na vyžádání poskytne seznam všech alergenů; výhodou je, že tímto způsobem se po api požaduje seznam všech alergenů pouze jednou za jedno uživatelské sezení (třída si totiž seznam uchová na později poté, co ho získá http requestem od api)
  - **MealService.cs** - podobný jako allergen service, ale poskytuje všechny možné typy jídel

> [!NOTE]
> Oba dva výše zmíněné services by se momentálně daly spojit do jednoho, ale jsou rozdělené z důvodu rozšiřování programu do budoucna v duchu **separation of concerns**.

- **ValidatedFormData** - obsahuje třídy u kterých využívám **ComponentModel** anotací pro **EditForm** componentu, která mi umožňuje tímto způsobem validovat, zda jsou data zadaná do formuláře ve správném formátu 
  - **MealFormData.cs** - obsahuje třídy `MealFormData`, `AllergenSelection` a také extension metody pro převod objektů typu `MealFormData` na `MealDto` a zpět
    - poskytuje zatím pouze `Required` anotace s odpovídajícími `ErrorMessage`, ale v budoucnu by se dalo využít vlastní složitější validace např. pro jméno pokrmu
    - pomocí setteru property `Name` automaticky odřízné zbytečné bílé znaky na začátku nebo na konci jména pokrmu
    - `AllergenSelection` - slouží pro propojení logické hodnoty html inputů typu `checkbox` s daným jménem allergenu
  - **ParticipantFormData.cs** - obsahuje třídy `MealFormData`, `BirthNumberToAgeParser` a extension metody pro převod mezi `ParticipantFormData` a `ParticipantDto`
    - properties obsahují settery, které umožní zpracovat vstup předtím, než se na něj použije validační kritérium (např. z rodného čísla odstranit lomítko)
    - setter property `BirthNumber` navíc automaticky spočítá age s využítím třídy `BirthNumberToAgeParser`
  - **Validators.cs** - obsahuje třídy, které slouží k validaci input fieldů formuláře; všechny dědí od třídy `ValidationAttribute`

> [!WARNING]
> Validace způsobená anotacemi jednotlivých properties se provádějí až poté, co je vyhodnocen celý kód setteru dané property!

> [!NOTE]
> Správnou funkčnost některých validátorů ze souboru **Validators.cs** je testována pomocí unit testů v projektu [UnitTests](#unittests).

- **wwwroot/** - obsahuje soubor **index.html**, stylesheet **app.css** a ikonky
  - **app.css** - selektory v tomto souboru mají následující formát:
    - začínají `.modal` - týkají se stylů dialogových oken
    - začínají `.div-table` - styl pro tabulky s účastníky/pokrmy
    - začínají `.sub-layout` - týkají se SubLayoutů

> [!TIP]
> Při testování různých stylů úpravou souboru `app.css` se hodí spouštět program pomocí příkazu `dotnet watch --project Server`. Tímto způsobem se změny v `app.css` projeví už za běhu programu. (Je však nutné změny v souboru nejprve uložit.)

- **App.razor** - defaultní soubor vytvořený při založení projektu, ponechán beze změny
- **_Imports.razor** - obsahuje defaultní importy a navíc import **Blazor Bootstrap** 
- **Program.cs** - obsahuje mimo jiné kód, kterým zavádíme do klienta služby jako **AllergenService** a **MealService**; zde je také nutné přidat kód pro zavedení **Blazor Bootstrapu**

### Shared 

Obsahuje následující adresáře:
- **DBModels** - obsahuje anotované modely pro databázi, klíče anotuji pomocí `[Key]`, cizí klíče pomocí `[ForeignKey("PropertyName")]`
- **DTOs** - (**Data Transfer Objects**) modely sloužící pro přenos skrz http requesty

> [!NOTE] 
> Oddělit modely pro databázi a DTOs jsem se rozhodl hlavně proto, jelikož pri posílání přímo databázových modelů jsem zbytečně posílal i tzv. **navigation properties**, které bez načtení z databáze (které jsem nechtěl provádět) vždy obsahovaly `null`.

### UnitTests

Obsahuje unit testy dvou vybraných validátorů. 
- **BirthNumberValidatorTests.cs** testuje správnou funkčnost validátoru rodných čísel
- **NameValidatorTests.cs** testuje správnou funkčnost validátoru jmen (jak křestních, tak i příjmení)

> [!WARNING]
> Testy v **BirthNumberValidatorTests.cs** předokládají, že lomítko bylo již odstraněno z rodného čísla před validací.