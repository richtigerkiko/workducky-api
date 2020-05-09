FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY /WorkDuckyApi/*.csproj ./WorkDuckyApi
COPY /WorkduckyLib/*.csproj ./WorkduckyLib
RUN cd WorkDuckyApi && dotnet restore

# Copy everything else and build
COPY /WorkDuckyApi ./WorkDuckyApi
COPY /WorkduckyLib ./WorkduckyLib
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WorkDuckyAPI.dll"]
