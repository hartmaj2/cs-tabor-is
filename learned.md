# Things I learned

## Git
- `git fetch` downloads changes that should be made to working dir but doesn't integrate them
- `git pull` integrates the changes right away, `git pull = git fetch + git merge`

## Blazor
- To mark parameters for components as required when creating a component using the html tag use `[Parameter, EditorRequired]` and set it as required

## Zsh
- Used a command to prepend a text to all components (needed to add a using directive)
`for file in ./Client/Components/*.razor ; do`
`cat prepend.txt "$file" > temp.txt ; mv temp.txt "$file"`
`done`

## C#
- `[start..end]` is a way to get substring using the range operator