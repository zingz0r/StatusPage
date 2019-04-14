FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["StatusPage.Api/StatusPage.Api.csproj", "StatusPage.Api/"]
RUN dotnet restore "StatusPage.Api/StatusPage.Api.csproj"
COPY . .
WORKDIR "/src/StatusPage.Api"
RUN dotnet build "StatusPage.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StatusPage.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StatusPage.Api.dll"]