FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-environment
WORKDIR /app

COPY . ./
WORKDIR /app/AcmeShipmentRouter
RUN dotnet restore
RUN dotnet publish -c Release -o ../out


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-environment /app/out .
#ENTRYPOINT ["dotnet", "AcmeShipmentRouter.dll"]