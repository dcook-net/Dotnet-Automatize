# Dotnet-AutoUpgrade

AutoUpgrade is a .Net Global tool that will automatically update your Project File(s), DockerFile(s) and .env file(s) with the changes required for migrating to .Net 2.1.

Because it's a .Net global tool, it works on Mac and Windows.

## To Install:

```
dotnet tool install autoupgrade -g --add-source https://nuget.meer-spacestation.co.uk/nuget 
```

you can leave out the --add-source option if you alread have the nuget server in your nuget feed.

## To Run:

As this is a global tool, you can run it from anywhere, supplying the full path the folder containing the solution you wish to upgrade:

```
autoUpgrade upgrade [PathToDirectory]
```

Alternatively, is you navigate to the desired folder, you can omit the Path and it will default to your current location.
