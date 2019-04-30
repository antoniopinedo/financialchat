# Financial Chat
A chatroom to share financial ideas and get stock quotes in real time


## Getting Started

Here you will find some instructions to get you a copy of the project up and running on your local machine for development and testing purposes, as well as a general overview of the project and its functionality.


## Features

* Allows registered users to log in and talk with other users in a chatroom.
* Allow users to post messages as commands into the chatroom with the following format /stock=stock_code
* Includes a bot that will call the Stooq API to get quotes based on the Stock Symbol.  The bot responds to the chatroom using a message broker. 
* Chat messages are ordered by their timestamps and keeps a cache of the last 50 messages.
* The project includes unit tests of the code.
* Bonus:
>* Invalid commands sent to the bot or exceptions raised are handled.
>* .NET identity is used for users authentication


### Project Structure

These projects are included in the solution:

* **FinancialChat** (web): The web application which includes the authentication modules and the chatroom logic.
* **FinancialChat.ChatBotStarter** (console): A console application that initiates the chat bot.  Chatbot will listen to the "request message queue" to receive commands, and will publish the results in the "post message queue".
* **FinancialChat.Database** (database): Contains the scripts to create the MSSQL database.
* **FinancialChat.Tests** (test): Unit tests of the classes.
* **FinancialChat.Utils** (library): Contains helper classes like a command parser, message broker facade and the ChatBot logic.


#### About the ChatBot

ChatBot was designed as an independent functionality and isolated in the "FinancialChat.Utils" library so it can be called from different sources.
The ChatBotStarter project is a console application that instantiates the bot and initiates it.  Multiple instances can be run in parallel for escalability.


## Installation, Configuration and Deployment

### Prerequisites

Financial Chat application connects to MSSQL database and uses message queues.  

Please install the following applications to properly run the application:
- Microsoft SQL Server (Any version)
- RabbitMq Server 3.7.14 (requires OTP v20.3)

To configure and deploy the application, MSSQL and RabbitMQ services should be running.


### Configuration

To configure this application follow these steps:

1. Modify the **web.config** file by changing the Connection String called *"DefaultConnection"* to point to the specified database server, making sure to include a user with privileges on the database and the right password:
```
connectionString="Server=localhost;Initial Catalog=FinancialChat;Persist Security Info=False;
User ID=test;Password=Pass1234;MultipleActiveResultSets=False;Encrypt=True;
TrustServerCertificate=True;Connection Timeout=30;"
```

2. Create the RabbitMQ users 
Create the username and password if needed.  RabbitMq can be configured to run without username and password, and the application supports it.
Use the rabbitmqctl to create the user like this:
```
rabbitmqctl add_user myUser myPass
```

Give permissions to the user on the Vhost with this command:
```
rabbitmqctl set_permissions -p /myvhost myUser ".*" ".*" ".*"
```

3. Configure the RabbitMQ settings in **web.config** (in FinancialChat project) and **App.config** (FinancialChat.ChatBotStarter) files adding your RabbitMQ username and password:
```
<appSettings>
  <add key="mqHost" value="localhost" />
  <add key="mqPort" value="5672" />
  <add key="mqUser" value="test" />
  <add key="mqPass" value="Pass1234" />
  <add key="mqVhost" value="/" />
</appSettings>
```

4. In development environment, both Web and Console applications should be set to start.


### Deployment

To deploy the application follow these steps:

1. Publish the **"FinancialChat.Database"** project to your MSSQL server instance (can be local or Azure DB).
2. Publish the **"FinancialChat"** project to IIS from Visual Studio.
3. Execute the **"FinancialChat.ChatBotStarter"** application manually.



## Technologies and frameworks

* [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet/mvc) - The web framework used
* [ASP.NET Identity](https://docs.microsoft.com/en-us/aspnet/identity/) - Authentication
* [RabbitMQ](https://www.rabbitmq.com/) - The message broker
* [FileHelpers 3.4](https://www.filehelpers.net/) - CVS processing library
* [SignalR](https://dotnet.microsoft.com/apps/aspnet/real-time) - Asynchronous notifications
* [Json.NET](https://www.newtonsoft.com/json) - Json serializing library
* [JQuery](https://jquery.com/) - Javascript library
* [Bootstrap 3.4](https://getbootstrap.com/docs/3.4/) - HTML, CSS and JS framework


## Known limitations and improvement options

* Add unit tests to improve coverage closer to 100%
* SignalR framework is based on WebSockets which allows a limited number of concurrent connections.  For escalability, Azure SignalR service can be used.
* Automate UI tests using selenium framework
* Include Unit Tests for Javascript files


## External resources

* [Stooq.com](https://stooq.com) - Stock quotes service


## Author

* **Antonio Pinedo** - *Developer* - [Antonio Pinedo](https://github.com/antoniopinedo)
