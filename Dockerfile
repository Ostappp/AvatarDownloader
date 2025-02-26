# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER root
WORKDIR /app


# Install dependencies
RUN apt-get update && apt-get install -y \
    libglib2.0-0 \
    libnss3 \
    libgconf-2-4 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxcomposite1 \
    libxcursor1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxi6 \
    libxrandr2 \
    libxrender1 \
    libxss1 \
    libxtst6 \
    fonts-liberation \
    libappindicator1 \
    libdbusmenu-glib4 \
    libdbusmenu-gtk4 \
    libatk1.0-0 \
    libatspi2.0-0 \
    libgtk-3-0 \
    libpango-1.0-0 \
    libpangocairo-1.0-0 \
    libcairo2 \
    libasound2 \
    wget \
    apt-transport-https 

# Install .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AvatarDownloader.csproj", "."]
RUN dotnet restore "./AvatarDownloader.csproj"
COPY . .
WORKDIR "/src/."
RUN

# Install EdgeDriver
RUN wget -q https://msedgedriver.azureedge.net/$(microsoft-edge --version | cut -d' ' -f2)/edgedriver_linux64.zip -O edgedriver_linux64.zip 
RUN unzip edgedriver_linux64.zip -d /usr/local/bin/ &
RUN rm edgedriver_linux64.zip



# Install Microsoft Edge
RUN apt-get update && apt-get install -y wget gnupg iputils-ping net-tools
RUN wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /usr/share/keyrings/microsoft-archive-keyring.gpg
RUN wget -q https://packages.microsoft.com/config/debian/12/prod.list -O /etc/apt/sources.list.d/microsoft-prod.list
RUN apt-get update && apt-get install -y microsoft-edge-stable
RUN rm -rf /var/lib/apt/lists/*



# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AvatarDownloader.csproj", "."]
RUN dotnet restore "./AvatarDownloader.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./AvatarDownloader.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AvatarDownloader.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chmod +x /app/selenium-manager/linux/selenium-manager
RUN chown app:app /app/selenium-manager/linux/selenium-manager
RUN touch /app/out.log
RUN chown app:app /app/out.log
RUN chmod -R 777 /tmp
USER app

ENTRYPOINT ["dotnet", "AvatarDownloader.dll"]