# ifc2geojson

Experimental tool for converting IFC model to GeoJSON (per storey)

## Parameters

-i : input file


Sample:

```
$ ifc2geojson -i spaces_all.ifc

tool ifc2geojson
Input file: spaces_all.ifc
Buildings: 1
description: Amsterdam ArenA model
Processing storey Peil=0.000
Processing storey +4250
Processing storey NIVO 2
Processing storey +9150 nivo 2A fase 1
Processing storey +11175 nivo 3A fase 1
Processing storey +14210 bk staal fase 1
Processing storey +19330 bk vloer nivo 5 fase 1
Processing storey +21770 fase 1
Processing storey +25775 bk staal fase 1
Processing storey +29360 nivo 8 Hoofdgebouw
Converting to GeoJSON per storey finished.
Elapsed: 00:00:01.9402988
```

## Dependencies

- GeoJSON.NET

- XBim.Essentials

- CommandLineParser