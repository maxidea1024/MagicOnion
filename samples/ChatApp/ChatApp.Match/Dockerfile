FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
#FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
EXPOSE 12346
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ChatApp.Match/ChatApp.Match.csproj", "ChatApp.Match/"]
RUN dotnet restore "ChatApp.Match/ChatApp.Match.csproj"
COPY . .
WORKDIR "/src/ChatApp.Match"
RUN dotnet build "ChatApp.Match.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ChatApp.Match.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChatApp.Match.dll"]