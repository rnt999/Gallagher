# Probability Calculator

Full-stack probability calculator with Azure Functions backend and React frontend.

## Quick Start

### Prerequisites
- .NET 10 SDK
- Node.js 18+

### Backend
```bash
cd ProbabilityCalculator.Api
dotnet build
dotnet run
```
Runs on: http://localhost:7179/api

### Frontend
```bash
cd probability-calculator-ui
npm install
npm run dev
```
Runs on: http://localhost:5173

## Testing
```bash
# Backend
dotnet test ProbabilityCalculator.Tests

# Frontend
cd probability-calculator-ui
npm test
```

## Production Build
```bash
dotnet publish -c Release
cd probability-calculator-ui && npm run build
```

## Endpoints
- `POST /api/v1/combined-with` — P(A) × P(B)
- `POST /api/v1/either` — P(A) + P(B) − P(A)P(B)

## Project Structure
- **ProbabilityCalculator.Api/** — Azure Functions backend
- **ProbabilityCalculator.Tests/** — Backend tests
- **probability-calculator-ui/** — React frontend

## Architecture
- Azure Functions (isolated worker) with ASP.NET Core
- FluentValidation, global exception middleware
- React + TypeScript with Vite

## Deployment
```bash
# Azure Functions
func azure functionapp publish <app-name>

# Static Web Apps
npm run build  # Creates dist/ folder
```

---
