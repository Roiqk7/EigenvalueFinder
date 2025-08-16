#!/bin/bash

# --- Start the Backend API ---
echo "▶️ Starting the ASP.NET Core Web API..."
ASPNETCORE_URLS="https://localhost:5001" dotnet run --project src/EigenvalueFinder.WebAPI &

echo "✅ Backend API is running on https://localhost:5001"
echo "-----------------------------------"

# --- Start the Frontend Static Server ---
echo "🌐 Serving the static frontend files..."
cd src/EigenvalueFinder.Web/wwwroot || { echo "Error: wwwroot directory not found."; exit 1; }
python3 -m http.server 8080 &

echo "✅ Frontend is available at http://localhost:8080"
echo "-----------------------------------"

echo "✨ Both the backend and frontend are now running. Press [Ctrl+C] to exit."

# Wait indefinitely for the user to press Ctrl+C
wait
