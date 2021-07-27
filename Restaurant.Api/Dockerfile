FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 433

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Restaurant.Api.csproj", "./"]
RUN dotnet restore "Restaurant.Api.csproj"
COPY . .
RUN dotnet publish "Restaurant.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# ENTRYPOINT ["dotnet", "Restaurant.Api.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Restaurant.Api.dll