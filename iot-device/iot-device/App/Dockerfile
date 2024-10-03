# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build-env
# Set the working directory in the container
WORKDIR /App

# Copy the entire project and build the release version
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff AS runtime

# Set the working directory in the runtime container
WORKDIR /App

# Copy the published files from the build stage
COPY --from=build-env /App/out .

# Expose the port that the application will run on
EXPOSE 8080

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "App.dll"]
