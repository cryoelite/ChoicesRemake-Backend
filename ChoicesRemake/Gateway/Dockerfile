#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#COPY ./certs/aspnetapp.pfx /https:ro/
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gateway/Gateway.csproj", "Gateway/"]
RUN dotnet restore "Gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/Gateway"
RUN dotnet build "Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]

ENV ASPNETCORE_ENVIRONMENT Production
#ENV ASPNETCORE_URLS "https://+:443;http://+:80"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password "Kimono2#"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path "../https:ro/aspnetapp.pfx"