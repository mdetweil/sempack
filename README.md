# sempack

sempack is a dotnet core cli tool to help version and pack nupkgs.

Updates the Version property of the .csproj file


## Installation

Use the command line with the following command:

```bash
dotnet tool install --global sempack --version 0.2.0 
```

## Usage

```bash
sempack -f foo/foo.csproj --major --minor --build-version --revision
```
## `sempack` Commands

`<Version>{MAJOR}.{MINOR}.{BUILD-VERSION}.{REVISION}</Version>` 

`major` - increments the major version by one

`minor` - increments the minor version by one

`build-version` - increments the build version by one

`revision` - increments the revision version by one

`v` or `verbose` - will turn on verbose logging


## `dotnet pack` Commands

sempack will pass through all [dotnet pack](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-pack?tabs=netcore2x) commands.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
