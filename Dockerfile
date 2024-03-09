
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./

RUN dotnet publish Asset-Service -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

ENV PORT=80
EXPOSE ${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ./Asset-Service --urls "http://[::]:${PORT}"