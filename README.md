# LongPd Semantic Agent

A .NET 8 Web API that uses **Microsoft Semantic Kernel** with **Google Gemini** (`gemini-1.5-flash`) to provide an AI chat endpoint.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- A **Google Gemini API Key** — get one at [Google AI Studio](https://aistudio.google.com/)

## Getting Started

### 1. Configure the API Key

**Option A — appsettings.json** *(quick local testing only, never commit the key)*

Open `appsettings.json` and fill in your key:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
  }
}
```

**Option B — .NET User Secrets** *(recommended for development)*

```bash
dotnet user-secrets set "Gemini:ApiKey" "YOUR_GEMINI_API_KEY_HERE"
```

**Option C — Environment Variable** *(recommended for production / CI)*

```bash
# Windows PowerShell
$env:Gemini__ApiKey = "YOUR_GEMINI_API_KEY_HERE"

# Linux / macOS
export Gemini__ApiKey="YOUR_GEMINI_API_KEY_HERE"
```

### 2. Run the API

```bash
dotnet run
```

Swagger UI will be available at `https://localhost:<port>/swagger`.

### 3. Call the Endpoint

**POST** `/api/chat`

```json
{
  "userMessage": "Hello! What can you do?"
}
```

**Response:**

```json
{
  "reply": "I can help you with a wide range of tasks..."
}
```

#### Example with curl

```bash
curl -X POST https://localhost:7000/api/chat \
  -H "Content-Type: application/json" \
  -d '{"userMessage": "Tell me a fun fact about Vietnam."}'
```

## Project Structure

```
├── Controllers/
│   └── AgentController.cs   # POST /api/chat endpoint
├── appsettings.json          # Configuration (ApiKey left blank — fill locally)
├── appsettings.Development.json
├── Program.cs                # DI setup: Semantic Kernel + Gemini
├── LongPd.SemanticAgent.csproj
└── .gitignore                # Excludes secrets, build artifacts
```

## Security Notes

- ✅ `appsettings.json` is committed with **empty** `ApiKey` — safe.
- ✅ `.gitignore` excludes `appsettings.Local.json`, `.env`, and other secrets files.
- ✅ Use **User Secrets** locally or **environment variables** in CI/CD — never hardcode keys.
- ⚠️ `Microsoft.SemanticKernel.Connectors.Google` is currently in alpha (`1.77.0-alpha`). API may change.

## NuGet Packages

| Package | Version |
|---|---|
| `Microsoft.SemanticKernel` | 1.77.0 |
| `Microsoft.SemanticKernel.Connectors.Google` | 1.77.0-alpha |
| `Microsoft.AspNetCore.OpenApi` | 8.0.x |
| `Swashbuckle.AspNetCore` | 6.x |