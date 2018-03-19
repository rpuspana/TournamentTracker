Organise a tournament with various teams(each having various members), rounds and prizes.

The application can save this data inside various files, locally, or inside a database (at March 2018 only a Microsoft SQL database can be used).

Technologies used: C# .Net Windows Forms SQL Management Studio

Don't forget to include the file /TournamentTracker/TrackerUI/App.config for the application to work.
The content of the App.config file should be this:

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <appSettings>
        <add key="filePath" value="D:\path where the fileStorage folder with all the saved datat about a tournament will be stored"/>
    </appSettings>
    <connectionStrings>
        <add name="Tournaments" connectionString="Server=.;Database=Tournaments;User Id=UserIdOfTheTournamentsDatabase;Password=UserPasswordOfTheTournamentsDatabase;" providerName="System.Data.SqlClient" />
    </connectionStrings>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>   
</configuration>
```
