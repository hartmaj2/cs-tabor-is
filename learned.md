# Things I learned

## Git
- `git fetch` downloads changes that should be made to working dir but doesn't integrate them
- `git pull` integrates the changes right away, `git pull = git fetch + git merge`

## Blazor
- To mark parameters for components as required when creating a component using the html tag use `[Parameter, EditorRequired]` and set it as required
- [Service lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes:~:text=Singleton-,Service%20lifetimes,-Services%20can%20be)

## Zsh
- Used a command to prepend a text to all components (needed to add a using directive)
`for file in ./Client/Components/*.razor ; do`
`cat prepend.txt "$file" > temp.txt ; mv temp.txt "$file"`
`done`

## C#
- `[start..end]` is a way to get substring using the range operator
- The check by edit form is done on the backing field after the getter is applied

## Markdown
- Extension Markdown All In One allows to automatically create Table Of Contents, I can also set what levels of headings I can include or not using either a special comment `<!-- omit from toc -->` or by setting the Toc: Levels 
- Pomocí admonitions lze v .md souborech hezky vypisovat tipy, poznámky a warningy `> [!NOTE]` nebo `> [!WARNING]` atd.

## Useful commands to calculate file sizes
- find . -name "*.cs" -print0 | xargs -0 du -shcA
- find . -name "*.razor" -print0 | xargs -0 du -shcA

- `for file in ./**/*.cs ; do stat -f "%z" "$file" ; done | paste -s -d + - | bc`
- `for file in ./**/Migrations/**/*.cs ; do stat -f "%z" "$file" ; done | paste -s -d + - | bc`
- `for file in ./**/obj/**/*.cs ; do stat -f "%z" "$file" ; done | paste -s -d + - | bc`
- `for file in ./**/*.razor ; do stat -f "%z" "$file" ; done | paste -s -d + - | bc`