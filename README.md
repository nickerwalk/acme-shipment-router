# acme-shipment-router

Coding Exercise by Platform Science

# Instructions

### [Download .NET 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

### Navigate to the AcmeShipmentRouter folder

## Run

- `dotnet restore`

- `dotnet publish -c Release -o ../out`

- `dotnet ../out/AcmeShipmentRouter.dll <Address list File Path> <Name list File Path>`
 (Parameters are optional, you can provide these later)


## Test

- `dotnet test`


# Docker instructions: 

Build the Image

- `docker build -t acme-shipment-router -f .\dockerfile .`

You will want to run a container with a volume to access your files. Once your container is in place you can run the following command
- `dotnet AcmeShipmentRouter.dll`