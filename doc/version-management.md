# VeloCity - Version Management

How Version number is propagated throughout the whole project at build time?

- `sources\Directory.build.props` file;
  - All net5.0 projects (assemblies)
- `sources\AssemblyInfo.Shared.cs` file;
  - All net framework projects (assemblies) (installer's custom actions project)
- `release\VeloCity.proj` file.
  - `VeloCity.Installer.wixproj` -> `<DefineConstants>` tag.
    - `Product.wxs` file.