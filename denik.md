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

