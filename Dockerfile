FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /api
COPY . .
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /api
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
        ffmpeg \
     && rm -rf /var/lib/apt/lists/*
COPY --from=build /api/output .
RUN mkdir {db_files,logs}

ENTRYPOINT ["dotnet", "MyNAS.Site.dll"]
EXPOSE 5000
