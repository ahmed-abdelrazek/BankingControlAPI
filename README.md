# BankingControlAPI
ASP.NET Core Web API CQRS simple banking control panel with Microsoft Identity Server and MediatR

# How to run
## Windows Visual Studio
open the project and run it 
you might want to change some settings in `appsettings.Development.json` file for debug mode

## VSCode
You might have to install SqlServer specially on linux or mac (native package or docker image)
you have to change Connection String in `appsettings.json`/`appsettings.Development.json` file

# Testing
you can test the api by visiting the swagger page most likly at 'https://localhost:7049/swagger' Visual Studio will launch in swagger
![image](https://github.com/user-attachments/assets/52a63bd8-31b9-4a22-a996-def5d6e453a6)

you can also use included postman collection to do the samething
