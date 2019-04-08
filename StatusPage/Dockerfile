FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base

WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY StatusPage/StatusPage.csproj StatusPage/
COPY StatusPage.Data/StatusPage.Data.csproj StatusPage.Data/
RUN dotnet restore StatusPage/StatusPage.csproj
COPY . .
WORKDIR /src/StatusPage
RUN dotnet build StatusPage.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish StatusPage.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "StatusPage.dll"]