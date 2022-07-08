#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["11Binary.Blob.Provider/11Binary.Blob.Provider.csproj", "11Binary.Blob.Provider/"]
RUN dotnet restore "11Binary.Blob.Provider/11Binary.Blob.Provider.csproj"
COPY . .
WORKDIR "/src/11Binary.Blob.Provider"
RUN dotnet build "11Binary.Blob.Provider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "11Binary.Blob.Provider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "11Binary.Blob.Provider.dll"]