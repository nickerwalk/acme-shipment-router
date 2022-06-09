# acme-shipment-router

Coding Challenge by Platform Science

Build Instructions

Download .NET Core 5.0
https://dotnet.microsoft.com/en-us/download/dotnet/5.0

Navigate to the AcmeShipmentRouter folder
Run
`dotnet restore`
`dotnet publish -c Release -o ../out`
`dotnet ../out/AcmeShipmentRouter.dll <Address list File Path> <Name list File Path>`

Test
`dotnet test`

Docker instructions: 
Build the Image
`docker build -t acme-shipment-router -f .\dockerfile .`
Run the container
