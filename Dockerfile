FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WorkDuckyApi/WorkDuckyAPI.csproj", "WorkDuckyApi/"]
COPY ["WorkduckyLib/WorkduckyLib.csproj", "WorkduckyLib/"]
RUN dotnet restore "WorkDuckyApi/WorkDuckyAPI.csproj"
COPY . .
WORKDIR "/src/WorkDuckyApi"
RUN dotnet build "WorkDuckyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkDuckyAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkDuckyAPI.dll"]