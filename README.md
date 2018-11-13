[![Build Status](https://dev.azure.com/davidcook0284/davidcook/_apis/build/status/cookie1981.Dotnet-Automatize)](https://dev.azure.com/davidcook0284/davidcook/_build/latest?definitionId=1)
[![nuget](https://img.shields.io/nuget/v/Automatize.svg)](https://www.nuget.org/packages/Automatize/)

# Dotnet-Automatize

Automatize is a .Net Global tool for automatically updating your .Net Core projects and solutions to the latest version of the the .Net Core Framework, in line with the migrations
docs: https://docs.microsoft.com/en-us/aspnet/core/migration/20_21?view=aspnetcore-2.1.

It's a .Net global tool, so it works on Mac and Windows.


## What does 

Automatize is a fairly blunt instrument.
It will update all of your Project File(s), DockerFile(s) and .env file(s) within the target folder with the changes required for migrating to .Net 2.1. 
(This currently only supports upgrades from 2.0 - 2.1).
If the target folder contains many projects/solutions, then they will all be updated. So, if all your code live under one root folder, it is possible to update them all with a single instruction.

If your project uses any MicroMachines.Common packages, then these will be upgraded to the minimum required version to support the update, if they are lower than the minimum version. 

Automatize will always update to the latest Major/Minor version. So if the latest version of 2.1 is 2.1.6, you'll get 2.1.6. Any previous patch versions will be skipped are effectively ignored.


## What it doesn't do

It will not resolve namespace conflicts caused as a result of the update.
It will not apply new code features, such as applying [ApiController] attribute for example.
It does not currently build or test your solutions after update.


## Pre-reqs:
You'll need .Net Core 2.1 SDK installed locally. Download at www.dot.net



## To Install:

```
dotnet tool install automatize -g --version 1.0.8
```

## Usage:

As this is a global tool, you can run it from anywhere, supplying the full path the folder containing the solution you wish to upgrade:

```
automatize upgrade [PathToDirectory]
```

Alternatively, if you navigate to the desired folder, you can omit the Path and it will default to your current location.

## Params

PathToDirectory - Fully qualified Path to the directory you wish to upgrade. Defaults to current location.
--useLinux - By Default the Base image to use in your DockerFile is assumed to be Alpine. If you would rather use a Linux base image include the --useLinux switch, or -l. Windows is not currently supported.

## Upgrading your version:

To update to the latest version: 

```
dotnet tool update automatize -g
```

## Coming updates:

Currently, Automatize only works with migration from 2.0 to 2.1, but the plan is to keep updating for each new version of .Net Core, (possibly even with preview releases). Updates will be applied incrementally, so upgrading from 2.0 to 2.2 Automatize will apply updates for 2.1, then 2.2, all with a single command.

I'd also like to extend the behaviour to automatically build & test your upgraded application, and commit to git supplying an optional Jira ticket number to support smart commits.

PR's welcome :)