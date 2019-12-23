# ifc2geojson

Experimental tool library for converting IFC model to GeoJSON (per storey)

## Parameters

-i : input file


Sample:

```
$ ifc2geojson -i AC20-FZK-Haus.ifc

tool ifc2geojson
Input file: AC20-FZK-Haus.ifc
Projekt-FZK-Haus
Erdgeschoss
Dachgeschoss
Elapsed: 00:00:01.4505950

```

## Dependencies

- Wkx - https://www.nuget.org/packages/Wkx/

- XBim.Essentials - https://www.nuget.org/packages/Xbim.Essentials/

- CommandLineParser - https://www.nuget.org/packages/CommandLineParser/