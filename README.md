# Stručná dokumentace <!-- omit from toc -->

## Obsah <!-- omit from toc -->

- [Anotace](#anotace)
- [Uživatelská dokumentace](#uživatelská-dokumentace)
  - [Spuštění programu](#spuštění-programu)
  - [Navigace v programu](#navigace-v-programu)


## Anotace

Program je informační systém, který slouží vedoucím tábora ke správě informací o účastnících tohoto tábora a také přidávání pokrmů na menu pro jednotlivé dny. Jedná se o webovou aplikaci, která má jak serverovou, tak klientskou část.

## Uživatelská dokumentace

### Spuštění programu

Pro spuštění programu je nutné, mít v souboru *appsettings.json* (nachází se ve složce *Server*) v sekci `ConnectionStrings` správně nakonfigurovanou `DefaultConnection`. V `DefaultConnection` je potřeba zapsat všechny potřebné konfigurační údaje pro připojení k databázi, kterou chcete používat.

Program se spouští příkazem `dotnet run` nebo `dotnet watch` z adresáře *Server*, kde se nachází serverový projekt, který automaticky po spuštění spustí i klienta. Po spuštění aplikace se v terminálu ukáže zpráva obsahující řádek `Now listening on: http://localhost:xxxx`. Danou adresu stačí zkopírovat do webového prohlížeče, skrz který nyní můžete s aplikací interagovat.

### Navigace v programu

Po spuštění ve webovém prohlížeči se objevíte automaticky v sekci *Participants*, která obsahuje jedinou podsekci jménem *All participants*. Vybírat mezi jednotlivými sekcemi můžete v levém panelu, kde se nachází kromě sekce *Participants*, také sekce *Food*. Právě aktivní sekci je vždy možné poznat pomocí bílého zbarvení pozadí tlačítka na levém panelu odpovídající dané sekci.
