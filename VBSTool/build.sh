#!/bin/bash

dotnet build
dotnet publish \
    --output ./Artifacts/vbstool \
    --runtime win-x64 \
    --configuration Release \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    --self-contained true