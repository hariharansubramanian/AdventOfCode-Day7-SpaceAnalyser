FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpaceAnalyser-AdventOfCodeDay7.csproj", "./"]
RUN dotnet restore "SpaceAnalyser-AdventOfCodeDay7.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SpaceAnalyser-AdventOfCodeDay7.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpaceAnalyser-AdventOfCodeDay7.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpaceAnalyser-AdventOfCodeDay7.dll"]
