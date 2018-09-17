#!/bin/bash

cd /build/test/$1/bin/Release/net461

mono /root/.nuget/packages/nunit.consolerunner/3.7.0/tools/nunit3-console.exe $1.dll ${@:2}