# Stručná dokumentace

## Obsah <!-- omit from toc -->

- [Anotace](#anotace)
- [Uživatelská dokumentace](#uživatelská-dokumentace)
  - [Spuštění programu](#spuštění-programu)
  - [Navigace v programu](#navigace-v-programu)
  - [Kritéria pro jednotlivé atributy účastníků](#kritéria-pro-jednotlivé-atributy-účastníků)
  - [Možnost přidání dalších alergenů](#možnost-přidání-dalších-alergenů)


## Anotace

Program je informační systém, který slouží vedoucím tábora ke správě informací o účastnících tohoto tábora a také přidávání pokrmů na menu pro jednotlivé dny. Jedná se o webovou aplikaci, která má jak serverovou, tak klientskou část.

## Uživatelská dokumentace

### Spuštění programu

Jelikož Serverová část programu využívá ke správě dat, databázi, tak je nutné, mít v souboru *appsettings.json* (nachází se ve složce *Server*) v sekci `ConnectionStrings` správně nakonfigurovanou `DefaultConnection`. V `DefaultConnection` je potřeba zapsat všechny potřebné konfigurační údaje pro připojení k databázi, kterou chcete používat.

Program se spouští příkazem `dotnet run` nebo `dotnet watch` z adresáře *Server*, kde se nachází serverový projekt, který automaticky po spuštění spustí i klienta. Po spuštění aplikace se v terminálu ukáže zpráva obsahující řádek `Now listening on: http://localhost:xxxx`, kde `xxxx` je číslo portu, na kterém server poslouchá (port lze nastavit v souboru */Properties/launchSettings.json*). Danou adresu stačí zkopírovat do webového prohlížeče, skrz který nyní můžete s aplikací interagovat.

### Navigace v programu

Web se dělí na sekce, které se dále dělí na podsekce. Po spuštění ve webovém prohlížeči se objevíte automaticky v sekci *Participants*. Vybírat mezi jednotlivými sekcemi můžete v levém panelu, kde se nachází kromě sekce *Participants*, také sekce *Food*. Právě aktivní sekci je vždy možné poznat pomocí bílého zbarvení pozadí tlačítka na levém panelu odpovídající dané sekci. Nacházíte-li se právě v nějaké sekci, tak přepínat mezi podsekcemi můžete pomocí horního panelu, který obsahuje klikatelný seznam podsekcí. Stejně jako u panelu se sekcemi poznáte aktivní podsekci bílým zbarvením pozadí tlačítka dané podsekce.

#### Sekce Participants

Sekce *Participants* obsahuje pouze jednu podsekci jménem *All participants*.

##### Podsekce All participants

Podsekce *All participants* sestává z tabulky, která poskytuje seznam účastníků a jejich základních informací jako id, jméno, příjmení a další. 

Každý sloupec podporuje třídění záznamů v tabulce podle hodnoty atributu tohoto sloupce. Stačí kliknout na název daného sloupce. Na pravé straně se pak objeví šipečka, která značí, zda jsou záznamy setříděné vzestupně či sestupně. Vzestupně se značí šipečkou nahoru, kde jako první záznam bereme ten první od shora. 

Všechny sloupce také umožňují filtrovat záznamy v tabulce na základě určitých kritérií, týkajících se hodnoty záznamu v daném sloupci. Sloupce obsahující hodnoty textové povahy je možné filtrovat na základě zadaného textového řetězce. Tento filtrovací řetězec se zadává do textového pole s popiskem *search...*. V tabulce se pak ukáží jen takové záznamy, jejichž hodnota v daném sloupci obsahuje řetězec, který ja zadaný v textovém poli nehledě na velká či malá písmena. Sloupce, které obsahují hodnoty číselné povahy umožňují filtrovat kliknutím na tlačítko filter a následným zadáním dolní a horní meze, kterou si přejeme, aby měla hodnota všech vyfiltrovaných záznamů. Tyto meze se dají zadat buď pomocí posuvníků nebo exaktním zapsáním dané hodnoty do textového pole. Navolené filtrovací hodnoty se dá všechny zrušit kliknutím na tlačítko *Reset filters*. 

Přidávat nové účastníky do tabulky lze kliknutím na tlačítko *Add a participant*. Po kliknutí na něj se zobrazí dialogové okno, které na levé straně obsahuje textová pole na zadání základních údajů o účastníkovi. Program automaticky kontroluje, zda hodnoty zadávané do těchto polí dávají smysl. Pokud navíc uživatel do políčka *Birth Number* zadá platné české rodné číslo, tak se mu jeho věk spočítá automaticky. Rodné číslo je také možné úplně vynechat a zadat věk manuálně (např. pro cizince). Na pravé straně je pak možné pomocí zaškrtávacích políček navolit diety, které daný účastník má. Potvrdit volbu lze stisknutím klávesy Enter, nebo kliknutím na tlačítko *Confirm*. Pokud jsou nějaké ze zadaných hodnot neplatné, program daná políčka zvýrazní červeně a vypíše, co konkrétně je na nich špatně.

Účastníky v tabulce je možné editovat pomocí tlačítka *Edit*. Pomocí tohoto tlačítka je možné upravit hodnoty pouze těch atributů, které jsou vidět v tabulce. Pokud si přejete upravit diety účastníka, tak je nutné toto provést v podsekci *Diets*, která se nachází v sekci *Food*. Pomocí tlačítka *Delete* je pak možné účastníka úplně vymazat z databáze.

#### Sekce Food

Sekce *Food* obsahuje dvě podsekce: *Menu* a *Diets*. Tato sekce obsahuje vše, co se tématicky týká pokrmů na táboře.

##### Podsekce Menu

Podsekce *Menu* obsahuje informace o tom, které dny jsou jaká jídla v nabídce na menu. V horní části ihned pod lištou s podsekcemi je možné přepínat datum, pro které chceme zobrazit jídelníček. Směrem do historie se přepínáme pomocí tlačítka označeného symbolem < nacházejícího se vlevo od nadpisu se zvoleným datem. Naopak pomocí tlačítka > lze posouvat datum směrem do budoucnosti.

Samotné denní menu je rozdělené na dvě tabulky nazývající se *Lunch* a *Dinner* a odpovídají obědu a večeři. Záznamy v tabulkách pak odpovídají jednotlivým pokrmům pro daný den a daný čas, kde časem je myšlen oběd či večeře. Každé jídlo má název, typ (vyjadřuje, zda se jedná o polévku či hlavní chod), alergeny v něm obsažené a počet objednávek, které učinili účastníci tábora. Allergeny v pokrmech přímo odpovídají možným dietám, které můžou účastníci mít, a jsou setříděné lexikograficky. Pokrmy v tabulkách jsou nejprve řazeny dle pořadí chodu vzestupně (nejprve polévka, pak hlavní chod) a následně lexikograficky dle jména.

Přidat nový pokrm do dané tabulky kliknutím na tlačítko *+* v pravém horním rohu odpovídající tabulky. Po kliknutí se zobrazí dialogové okno s popiskem odpovídajícím danému datu a času. Na jméno jídla nejsou kladeny žádné restrikce kromě toho, že nesmí být prázdné, což ocení především kreativní tvůrci jídelníčků. Typ jídla (polévka, hlavní chod) je však nutné zadat. Dále je pak možné libovolně navolit alergeny pomocí zaškrtávacích políček. Pokrmy lze mazat a upravovat stejně, jako to lze v podsekci *All participants* v tabulce s účastníky.

##### Podsekce Diets

Podsekce *Diets* obsahuje tabulku s účastníky, která zobrazuje jejich diety. Diety každého účastníka jsou setříděny lexikograficky. Sloupce podobně jako v podsekci *All participants* umožňují řazení a filtrování. Řazení sloupce s dietami funguje také lexikograficky a porovnávají se textové řetězce vzniklé zřetězení všech diet za sebou tak, jak jsou zapsány. V sloupci s dietami lze také filtrovat kliknutím na tlačítko *Filter diets* a zvolením diet, které chceme filtrovat pomocí zaškrtávacích políček. V tabulce se pak zobrazí pouze ti účastníci, kteří mají všechny ze zvolených diet (můžou jich však mít i více). Všechny filtrovací kritéria můžeme zrušit pomocí tlačítka *Reset filters* stejně tak, jako tomu bylo v podsekci *All participants*.

Diety účastníků je možné pomocí tlačítka *Edit diets* u daného účastníka.

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

Rodné číslo není nutné zadat. Pokud se však uživatel rozhodne ho zadat. Pak musí jít o platné české rodné číslo ve formátu odpovídajícímu rodným číslům vydaným po 1.1.1954. Číslo lze zadat bez lomítka nebo i s ním. Lomítko se však musí nacházet mezi 6. a 7. cifrou počínaje zleva a čílsováno od 1. Po zadání je automaticky odstraněno.

### Možnost přidání dalších alergenů

Aplikace umožňuje také přidat další alergeny, pokud by aktuální volba alergenů nebyla dostačující. Toto však nejde provést interaktivně pomocí uživatelského rozhraní a je nutné poslat http request serveru. Tento json request musí být formátu:
```
POST http://localhost:xxxx/api/allergens/add
Content-Type: application/json

{
    "Name" : "JmenoAlergenu"
}
```
kde `xxxx` je port, na kterém server běží a `JmenoAlergenu` je libovolný textový řetězec popisující daný alergen.

Přidání dalších typů jídel (např. dezert) nějakým uživatelsky aspoň trochu přívětivým způsobem program zatím nepodporuje.