## WebSocket Server (ASP.NET Core)

A minimal ASP.NET Core application that provides:
A WebSocket endpoint
CLI-based WebSocket testing using wscat
This project is backend-focused and does not serve a frontend UI.

## Requirements
.NET 8 SDK
Node.js & npm
macOS / Linux / Windows (WSL supported)

## Run Application
From the project root:
1. Restore dependencies
dotnet restore

2. Build the application
dotnet build

3. Run the application
dotnet run

Expected output:
Now listening on: http://localhost:5031
Accessing http://localhost:5031/ in a browser may return 404 — this is expected.

## WebSocket
Endpoint
ws://localhost:5031/ws

## WebSocket Testing with wscat
Install wscat
npm install -g wscat


Verify:
wscat --version

## Connect to WebSocket
wscat -c ws://localhost:5031/ws


Successful connection:
connected (press CTRL+C to quit)

Send Message

Type a message and press Enter:

hello


Server response:

Echo from server: hello


## How to Confirm the App Is Working
Test HTTP endpoint (recommended)

Open in browser:

http://localhost:5031/health


You should see something like:

{
  "status": "ok",
  "service": "SimpleWebSocket"
}
