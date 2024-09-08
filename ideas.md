# Improvement ideas

- Add exception handling
- Add other sections needed for my RP
- Add switching between CZ and ENG
- Add authentication
- Make quick grid with participants adjust when window size changed
- Add posibility to hide column with allergen list
- Generalize confirm delete modal so I can reuse code for delete participant and delete meal (maybe use generic types and single AddTypeModal.razor component)
- Rename ParticipantsDbContext to CampDbContext
- Make the width of my div table meal columns adjust themselves automatically based on the longest entry
- Add male/female 
- Remove age and calculate it from birth number instead
- Create api manager - all methods to communicate with api will be stored there in one place
- Create allergen manager - will load possible allergens and will provide them to classes that need them

## Useful commands to calculate file sizes
- find . -name "*.cs" -print0 | xargs -0 du -shcA
- find . -name "*.razor" -print0 | xargs -0 du -shcA