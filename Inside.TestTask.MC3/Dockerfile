#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Inside.TestTask.MC3/Inside.TestTask.MC3.csproj", "Inside.TestTask.MC3/"]
COPY ["Inside.TestTask.Model/Inside.TestTask.Model.csproj", "Inside.TestTask.Model/"]
COPY ["Org.OpenAPITools/Org.OpenAPITools.csproj", "Org.OpenAPITools/"]
RUN dotnet restore "Inside.TestTask.MC3/Inside.TestTask.MC3.csproj"
COPY . .
WORKDIR "/src/Inside.TestTask.MC3"
RUN dotnet build "Inside.TestTask.MC3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inside.TestTask.MC3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inside.TestTask.MC3.dll"]
