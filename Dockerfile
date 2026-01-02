FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /DFDExam

COPY DFDExam/DFDExam.App/DFDExam.App.csproj .
RUN dotnet restore "DFDExam.App.csproj"

COPY DFDExam/DFDExam.App/. .

RUN dotnet build "DFDExam.App.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "DFDExam.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DFDExam.App.dll"]