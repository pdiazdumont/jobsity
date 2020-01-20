# Jobsity chat

Simple chat application built using .Net Core

## Projects

### Jobsity.Web
Main application, it provides the UI and the chat operations. It uses SignalR to enable real time communication.

### Jobsity.Bot.Stock.Api
Api for the Stock bot, it receives an event every time a message is sent and resolves it by returning the stock price.

## Requirements
- Net Core 3.1
- A working sql server database, the connection string is needed in the `appsettings.json` of `Jobsity.Web`
- Ports `44370` and `44308` must be available

## Installation instructions
In order for the project to run properly `Jobsity.Web` and `Jobsity.Bot.Stock.Api` need to run simultaneously, this can be done by setting Visual Studio to run multiple startup projects or by building the solution and manually execute the executables for each application.

Steps:
1. Setup the connection string for the database
2. Run the database migration via the dot net CLI or Visual Studio command line (https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#update-the-database) using the `Jobsity.Web` project
3. Configure Visual Studio with multiple startup projects
4. Execute the solution

## Usage instructions
1. Navigate to `https://localhost:44370/`
2. Create a new user
3. Login
4. The chat window will be displayed, start typing and the message will be sent
5. To trigger the bot the message should be `/stock={stockname}`, sample: `/stock=aapl.us`

## How it works
Once a new message is posted the application checks if it's a normal text or a command (eg /stock), depending on the result an async message is posted to the corresponding message broker to process it. For normal posts the post is saved to the database and published to the UI, for command posts the post is publish to all the registered bots for the application (following a webhook approach). The bot process the command and sends the information as a message using the same api as the UI.

## Considerations
- To work with queues [Rebus](https://github.com/rebus-org/Rebus) was used, it is an abstraction for multiple service bus providers. The project is configured to use sql server as a "service bus" meaning that it will work out of the box using the project's database. Minor changes are required in order to configure Rebus to use Azure service bus or RabbitMq.
- The approach for the bots is that multiple can be registered, due to time it was not implemented, the stock quote bot has been included as part of the original database migration. The bot implementation follows some practices by [Slack](https://api.slack.com/events-api#events_api_request_urls) about request validation.