# Probability Calculator UI

React + TypeScript + Vite frontend for the Probability Calculator, featuring a clean interface for calculating combined and either probability operations.

## Features

- 🧮 Two-operation probability calculator (P(A) × P(B) and P(A) + P(B) − P(A)P(B))
- ⚛️ React 19 with TypeScript
- ⚡ Vite for fast development and builds
- ✅ Comprehensive unit tests with Vitest + React Testing Library
- 🎨 Accessible UI with semantic HTML and ARIA labels

## Development

### Setup

```bash
npm install
npm run dev        # Start dev server (http://localhost:5173)
npm run build      # Production build
npm run lint       # Check code style
npm test           # Run unit tests
npm run test:ui    # Open test UI dashboard
```

### Project Structure

```
src/
├── App.tsx           # Main calculator component
├── App.css           # Component styles
├── api.ts            # Backend API client
├── types.ts          # TypeScript types
├── main.tsx          # Entry point
└── App.test.tsx      # Unit tests
```

## Testing

Tests cover:
- Component rendering and operations
- Tab switching between calculation types
- Form submission and validation
- Error handling and display
- Loading states
- API integration with mocked endpoints

Run tests:
```bash
npm test            # Watch mode
npm run test:ui     # Interactive dashboard
```

## Architecture

- **State Management**: React hooks (useState)
- **API Integration**: Async calculate() function with error handling
- **Testing**: Vitest with React Testing Library for user-centric tests
- **Accessibility**: Semantic HTML, ARIA attributes, keyboard navigation

## Environment Variables

Connect to the backend API by setting the base URL in `api.ts`. Default: `http://localhost:7079` (Azure Functions local)