FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/Services/UserProfileService/UserProfileService.API/UserProfileService.API.csproj
RUN dotnet publish src/Services/UserProfileService/UserProfileService.API/UserProfileService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "UserProfileService.API.dll"]
