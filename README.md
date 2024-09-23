# Stručná dokumentace

## Obsah <!-- omit from toc -->

- [Anotace](#anotace)
- [Uživatelská dokumentace](#uživatelská-dokumentace)
  - [Spuštění programu](#spuštění-programu)
  - [Navigace v programu](#navigace-v-programu)


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

Všechny sloupce také umožňují filtrovat záznamy v tabulce na základě určitých kritérií, týkajících se hodnoty záznamu v daném sloupci. Sloupce obsahující hodnoty textové povahy je možné filtrovat na základě zadaného textového řetězce. Tento filtrovací řetězec se zadává do textového pole s popiskem *search...*. V tabulce se pak ukáží jen takové záznamy, jejichž hodnota v daném sloupci obsahuje řetězec, který ja zadaný v textovém poli nehledě na velká či malá písmena. Sloupce, které obsahují hodnoty číselné povahy umožňují filtrovat kliknutím na tlačítko filter a následným zadáním dolní a horní meze, kterou si přejeme, aby měla hodnota všech vyfiltrovaných záznamů. Tyto meze se dají zadat buď pomocí posuvníků nebo exaktním zapsáním dané hodnoty do textového pole.

