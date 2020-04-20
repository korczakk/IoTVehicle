FROM pomma89/dotnet-mono:dotnet-2-mono-5-sdk AS build-env
WORKDIR /app

ENV FrameworkPathOverride /usr/lib/mono/4.7.2-api/

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out -f net472

# Build runtime image for ARM v5 using mono
FROM arm32v5/mono:5.20
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT [ "mono", "dotnet.mvc.exe" ]