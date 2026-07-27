# ============================================
# STAGE 1: BASE (The Runtime Environment)
# ============================================
# Pulls the lightweight .NET 10.0 runtime image from Microsoft Container Registry (MCR). This is all you need to *run* the app.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 as base

# Switches from the default root user to a restricted user for better security.
USER $APP_UID 


# Create and sets the active directory inside the container
WORKDIR /src

# Documents that this container will listen for HTTP/HTTPS traffic on the specified ports.
EXPOSE 8080
EXPOSE 8081


# ============================================
# STAGE 2: BUILD (The Compilation Environment)
# ============================================
# Pulls the heavy .NET 10.0 SDK image from Microsoft Container Registry (MCR). This is all you need to *build* the code.
FROM mcr.microsoft.com/dotnet/sdk:10.0 as build
WORKDIR /src

# Layer Caching Trick: Copy ONLY the project files first.
# If C# code changes but the packages don't, Docker reuses the cached NuGet packages.
COPY ["NexusBooks.Author.Api/NexusBooks.Author.Api.csproj", "NexusBooks.Author.Api/"]
COPY ["NexusBooks.Author.Application/NexusBooks.Author.Application.csproj", "NexusBooks.Author.Application/"]
COPY ["NexusBooks.Author.Domain/NexusBooks.Author.Domain.csproj", "NexusBooks.Author.Domain/"]
COPY ["NexusBooks.Author.Infraestructure/NexusBooks.Author.Infraestructure.csproj", "NexusBooks.Author.Infraestructure/"]
COPY ["../../NexusBooks.Shared/NexusBooks.Shared.csproj", "NexusBooks.Shared"]

# Download all required external libraries and dependencies.
RUN dotnet restore "./NexusBooks.Author.Api/NexusBooks.Author.Api.csproj"

# Now copy all the actual source code into the container
COPY . .

# Move into the main API project folder
WORKDIR "/src/NexusBooks.Author.Api"

# Compile the code in Release mode (optimized) and output it to /src/build
RUN dotnet build "./NexusBooks.Author.Api.csproj" -c Release -o /src/build

# ============================================
# STAGE 3: PUBLISH (Preparing for Production)
# ============================================
# Pick up where the 'build' stage left off.
FROM build as publish

# Pack the compiled code and dependencies into a deployment-ready folder.
# /p:UseAppHost=false prevents generating unnecessary OS-specific executable files.
RUN dotnet publish "./NexusBooks.Author.Api.csproj" -c Release -o /src/publish /p:UseAppHost=false

# ==========================================
# STAGE 4: FINAL (The Magic Step)
# ==========================================
# Abandon the heavy build/publish stages and start fresh from the tiny 'base' image.
FROM base as final
WORKDIR /app

# Reach back into the 'publish' stage and copy ONLY the finished production files.
COPY --from=publish /src/publish .

# The command that actually boots up the API when the container starts.
ENTRYPOINT ["dotnet", "NexusBooks.Author.Api.dll"]