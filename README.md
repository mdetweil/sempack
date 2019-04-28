# sempack

sempack is a dotnet core cli tool to help version and pack nupkgs.

Updates the VersionPrefix property of the .csproj file.

If the Version property is set, Version will be carried over to VersionPrefix.

If both VersionPrefix and Version properties are set, the highest value for each will be chosen.

## Status

 Build  |  Status
 -------|--------
develop | [![Build Status](https://travis-ci.com/mdetweil/sempack.svg?branch=develop)](https://travis-ci.com/mdetweil/sempack)
 master | [![Build Status](https://travis-ci.com/mdetweil/sempack.svg?branch=master)](https://travis-ci.com/mdetweil/sempack)
## Installation

Use the command line with the following command:

```bash
dotnet tool install --global sempack
```

## Usage

```bash
sempack -f foo/foo.csproj --major --minor --build-version --revision
```
## `sempack` Commands

`<Version>{MAJOR}.{MINOR}.{BUILD-VERSION}.{REVISION}</Version>` 

| Command | Description |
| --- | --- |
| `f` or `source-file` | the csproj file to pack |
| `major` | increments the major version by one |
| `minor` | increments the minor version by one |
| `build-version` | increments the build version by one |
| `revision` - increments the revision version by one |
| `v` or `verbose` | will turn on verbose logging |


## `dotnet pack` Commands

sempack will pass through all [dotnet pack](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-pack?tabs=netcore2x) commands.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
