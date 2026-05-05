#!/bin/bash

set -e

# Restore NuGet packages
dotnet restore

# Restore local dotnet tools
if [ -f "./.config/dotnet-tools.json" ]; then
  dotnet tool restore
fi
