# Pyramid Structure Transformation

This work is implementation of recursive (fractal) pyramid data type transformation:
collapse using predefined transformation maps (see `applyCheckSum` function). 
The application is written in F# using .NET Core and VS Code. The project works 
on Rider (tested) and should work on Visual Studio also. The application was developed 
and tested on Linux, but has to work with no issues on Windows and Mac as Core 
is multiplatform.

The key idea in this implementation is `Pyramid` sum type:


```
type Pyramid = 
| Pyramids of (Pyramid * Pyramid * Pyramid * Pyramid)
| Cell of int
```

It has two possible constructors:
1. cell with number (smallest possible pyramid):

    `Cell 1`

2. pyramid which consists from 4 subpiramides:

    `Pyramids(Cell 1, Cell 1, Cell 0, Cell 1)`

  or more complex:

```
Pyramids(
    Pyramids(Cell 1, Cell 1, Cell 0, Cell 1),
    Pyramids(Cell 0, Cell 0, Cell 0, Cell 1),
    Pyramids(Cell 1, Cell 1, Cell 1, Cell 1),
    Pyramids(Cell 1, Cell 1, Cell 0, Cell 0)
)
```

Consider this pyramid:

    `Pyramids(p1, p2, p3, p4)`

The corresponding subpiramids are located in this order:

```
          ^^
        / p1 \
       *------*
      /p2\p3/p4\
     *----------*
```

or in this when pyramid is upside down:

```
     *----------*
      \p2/p3\p4/
       *------*
        \ p1 /
          **
```

## Prerequisites

.NET Core 3.0 SDK

## Build

`dotnet build`

## Run from sources

`dotnet run 1001000010111111`

or 

`dotnet run -- 1001000010111111`

## Run from build binaries

Publish:

`dotnet publish`

Make binary file executable (optional for Linux):

`chmod +x bin/Debug/netcoreapp3.0/publish/pyramid-transform`

Run binary:

`./bin/Debug/netcoreapp3.0/publish/pyramid-transform 1101`

or

`./bin/Debug/netcoreapp3.0/publish/pyramid-transform.exe 1101`

## Run tests

The project contains a set of unit tests. To run the tests from console:

`dotnet test`

## Sample output dumps

Located in dumps folder. First line of dump is original input argument.

