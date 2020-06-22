FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY . /app
WORKDIR /app
RUN apt-get update
RUN apt install wget --assume-yes
RUN wget https://packages.microsoft.com/config/ubuntu/19.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get install apt-transport-https
RUN apt-get update
RUN apt-get install vim --assume-yes
RUN apt-get install dotnet-sdk-3.1 --assume-yes
RUN dotnet tool install --global dotnet-trace 
ENV PATH="${PATH}:~/.dotnet/tools"
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.API.dll"]