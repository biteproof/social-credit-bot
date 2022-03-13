FROM mcr.microsoft.com/dotnet/sdk:5.0
 
WORKDIR /home/app
ENV DOTNET_URLS=http://+:5000
ENV DOTNET_ENVIRONMENT=Production

EXPOSE 5000
 
COPY . .
 
RUN dotnet restore
 
RUN dotnet publish ./RottenBot.Web/RottenBot.Web.csproj -o /publish/
 
WORKDIR /publish

ENTRYPOINT ["dotnet", "RottenBot.Web.dll"]