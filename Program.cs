#pragma warning disable SKEXP0070 // AddGoogleAIGeminiChatCompletion is experimental

using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// ── Configuration ────────────────────────────────────────────────────────────
var geminiApiKey = builder.Configuration["Gemini:ApiKey"]
    ?? throw new InvalidOperationException(
        "Gemini:ApiKey is not configured. " +
        "Set it in appsettings.json or via User Secrets / environment variables.");

// ── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "LongPd Semantic Agent API",
        Version = "v1",
        Description = "Web API powered by Microsoft Semantic Kernel and Google Gemini."
    });
});

// Register Semantic Kernel with Google AI Gemini chat completion
builder.Services.AddKernel()
    .AddGoogleAIGeminiChatCompletion(
        modelId: "gemini-1.5-flash",
        apiKey: geminiApiKey);

// ── Build & Middleware ────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
