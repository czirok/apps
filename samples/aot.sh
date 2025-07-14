#/bin/bash

cd ./EasyUIBinding
dotnet publish -p:PublishProfile=Release.pubxml
cd ../GraphicsTester.Skia.GirCore
dotnet publish -p:PublishProfile=Release.pubxml
cd ../QuickStart1
dotnet publish -p:PublishProfile=Release.pubxml
cd ../Yaml.Localization/GirCoreApp
dotnet publish -p:PublishProfile=Release.pubxml
cd ../../publish

rm *.pdb

ls -lh
ls -l
