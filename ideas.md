# Improvement ideas

- Add exception handling
- Add other sections needed for my RP
- Add switching between CZ and ENG
- Add authentication
- Add posibility to hide column with allergen list
- Generalize confirm delete modal so I can reuse code for delete participant and delete meal (maybe use generic types and single AddTypeModal.razor component)
- Make the width of my div table meal columns adjust themselves automatically based on the longest entry
- Add male/female 
- Create api manager - all methods to communicate with api will be stored there in one place
- Create allergen manager - will load possible allergens and will provide them to classes that need them
- Add possibility to work with file instead of database (generalize controllers)

## Useful commands to calculate file sizes
- find . -name "*.cs" -print0 | xargs -0 du -shcA
- find . -name "*.razor" -print0 | xargs -0 du -shcA