#!/bin/bash

cd ./EasyUIBinding
dotnet publish -p:PublishProfile=Release.pubxml
cd ../GraphicsTester.Skia.GirCore
dotnet publish -p:PublishProfile=Release.pubxml
cd ../QuickStart1
dotnet publish -p:PublishProfile=Release.pubxml
cd ../Yaml.Localization/GirCoreApp
dotnet publish -p:PublishProfile=Release.pubxml
cd ../../LiveChartsCore/GirCoreSample
dotnet publish -p:PublishProfile=Release.pubxml
cd ../../publish

rm *.pdb
rm *.dbg
mv GirCoreSample LiveChartsCore

echo ""
for file in *; do
    if [ -f "$file" ]; then
        size_bytes=$(stat -c%s "$file")
        size_human=$(numfmt --to=iec --suffix=B $size_bytes | sed 's/B$//' | sed 's/\./,/')
        printf "%8d bytes %4s %s\n" $size_bytes $size_human $file
    fi
done