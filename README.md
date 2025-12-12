# Connect4 Web & Console Application

This project is a Connect4-style game implemented as both a **Web Application** and a **Console Application** using **C# and .NET**.

## Features

### General
- Configurable board size (width & height)
- Configurable win condition (e.g. 3, 4, 5 in a row)
- Two board modes:
  - Classical board
  - Cylindrical board (horizontal wrap-around)
- Game state persistence using a database (EF Core + SQLite)

### Web Application
- Razor Pages based web app
- Create unlimited parallel games
- Saved games list with active/completed status
- Continue or view completed games
- Multiplayer support (turn-based, two players)
- Human vs Human and Human vs AI modes

### Console Application
- Interactive console UI
- Game modes:
  - Human vs Human (H2H)
  - Human vs AI (H2AI)
  - AI vs AI (AI2AI)
- AI uses Minimax algorithm with Alpha-Beta pruning
- Configurable AI difficulty (Easy / Normal / Hard)
- Save and load games from persistent storage

### Artificial Intelligence
- Minimax algorithm with alpha-beta pruning
- Difficulty levels control search depth
- Works on both classical and cylindrical boards

## Architecture
- **BLL (Business Logic Layer)** – Game rules, board logic, AI
- **DAL (Data Access Layer)** – Repositories (EF Core / JSON)
- **WebApp** – Razor Pages UI
- **ConsoleApp** – Console-based UI
- Each game has its own independent state, enabling unlimited parallel games

## Technologies
- C# (.NET)
- ASP.NET Core Razor Pages
- Entity Framework Core (SQLite)
- Console UI

## Notes
- Multiplayer is turn-based (not real-time)
- Parallel games are handled by separate game configurations stored in the database
