rmdir /s /q dist

dotnet clean
dotnet publish -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --output .\dist\WithSDK --self-contained true
dotnet publish -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --output .\dist\WithoutSDK --no-self-contained