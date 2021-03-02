FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /api
COPY . .
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /api
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
        ffmpeg \
     && rm -rf /var/lib/apt/lists/*
COPY --from=build /api/output .
RUN mkdir db_files

ENTRYPOINT ["dotnet", "MyNAS.Site.dll"]
EXPOSE 5000
