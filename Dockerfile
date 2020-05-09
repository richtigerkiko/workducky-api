FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN mkdir WorkDuckyAPI
RUN mkdir WorkduckyLib
COPY /WorkDuckyApi/*.csproj ./WorkDuckyAPI
COPY /WorkduckyLib/*.csproj ./WorkduckyLib
RUN ls /app 
RUN dotnet restore ./WorkDuckyAPI/*.csproj

# Copy everything else and build
COPY /WorkDuckyApi ./WorkDuckyApi
COPY /WorkduckyLib ./WorkduckyLib
WORKDIR /app/WorkDuckyApi
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app/WorkDuckyApi
#COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WorkDuckyAPI.dll"]
