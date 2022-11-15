# WorldCup2022-MVC
1. Po sklonowaniu repo trzeba się upewnić czy w Visual Studio 2022 ma się nastęujące pakiety nuget:
   -Microsoft.EntityFrameworkCore
   -Microsoft.EntityFrameworkCore.Design
   -Microsoft.EntityFrameworkCore.SqlServer
   -Microsoft.EntityFrameworkCore.Tools
   -Newtonsoft.Json.Bson
   
 2. Utwórz nową bazę danych o nazwie WorldCup2022 (
 View->SQL Server Object Explorer -> Databases -> Add new database
 )
 3. Do appsettings.json należy dodać connectionString, który znajduje się w we właściwościach utworzonej bazy danych.
 4. Następnie w Package Manager Console należy dodać tabele z załączonych już migracji w repo. Niestety każdy kontekst trzeba dodać osobno.

update-database -context GroupContext
update-database -context GroupStageContext
update-database -context KnockoutStageContext
update-database -context MatchesContext
update-database -context PromotedTeamsContext
update-database -context SimulatedKnockoutPhaseContext
update-database -context TeamContext

5. W folderze "DbScripts" są zamieszczone z danymi wymaganymi do poprawnego działania aplikacji. Należy uruchomić każdy skrypt. (Można użyć MSSQL)

   
6. KONIEC :)
