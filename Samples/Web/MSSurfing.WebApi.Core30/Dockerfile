FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Samples/Web/MSSurfing.WebApi.Core30/MSSurfing.WebApi.Core30.csproj", "Samples/Web/MSSurfing.WebApi.Core30/"]
RUN dotnet restore "Samples/Web/MSSurfing.WebApi.Core30/MSSurfing.WebApi.Core30.csproj"
COPY . .
WORKDIR "/src/Samples/Web/MSSurfing.WebApi.Core30"
RUN dotnet build "MSSurfing.WebApi.Core30.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSSurfing.WebApi.Core30.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSSurfing.WebApi.Core30.dll"]