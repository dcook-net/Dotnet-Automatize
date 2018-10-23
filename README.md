[![Build Status](https://dev.azure.com/davidcook0284/davidcook/_apis/build/status/cookie1981.Dotnet-Automatize)](https://dev.azure.com/davidcook0284/davidcook/_build/latest?definitionId=1)

# Dotnet-Automatize

Automatize is a .Net Global tool that will automatically update your Project File(s), DockerFile(s) and .env file(s) with the changes required for migrating to .Net 2.1. (This currently only supports upgrades from 2.0)

Because it's a .Net global tool, it works on Mac and Windows.

## Pre-reqs:
You'll need .Net Core 2.1 SDK installed locally. Download at www.dot.net



## To Install:

```
dotnet tool install automatize -g --version 1.0.2
```

## Usage:

As this is a global tool, you can run it from anywhere, supplying the full path the folder containing the solution you wish to upgrade:

```
automatize upgrade [PathToDirectory] [BaseImage]
```

Alternatively, if you navigate to the desired folder, you can omit the Path and it will default to your current location.

## Params

PathToDirectory - Fully qualified Path to the directory you wish to upgrade. Defaults to current location, but you'll need to specify the path if you are specifying 'BaseImage' 
BaseImage - The Base image to use in your DockerFile. Valid options are 'Alpine' or 'Linux'. Defaults to Alpine.

## Upgrading your version:

To update to the latest version: 

```
dotnet tool update automatize -g
```

## Coming updates:

Currently, Automatize only works with migration from 2.0 to 2.1, but the plan is to keep updating for each new version of .Net Core, (possibly even with preview releases). Updates will be applied incrementally, so upgrading from 2.0 to 2.2 Automatize will apply updates for 2.1, then 2.2, all with a single command.

I'd also like to extend the behaviour to automatically build & test your upgraded application, and commit to git supplying an optional Jira ticket number to support smart commits.
