using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace LongPd.SemanticAgent.Controllers;

[ApiController]
[Route("api")]
public class AgentController : ControllerBase
{
    private readonly IChatCompletionService _chatCompletionService;
    private readonly ILogger<AgentController> _logger;

    public AgentController(
        IChatCompletionService chatCompletionService,
        ILogger<AgentController> logger)
    {
        _chatCompletionService = chatCompletionService;
        _logger = logger;
    }

    /// <summary>
    /// Send a message to the AI and receive a response.
    /// </summary>
    /// <param name="request">The chat request containing the user message.</param>
    /// <returns>The AI-generated response.</returns>
    [HttpPost("chat")]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserMessage))
        {
            return BadRequest(new { error = "userMessage must not be empty." });
        }

        _logger.LogInformation("Received chat request: {Message}", request.UserMessage);

        try
        {
            var chatHistory = new ChatHistory();
            chatHistory.AddUserMessage(request.UserMessage);

            var result = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);
            var reply = result.Content ?? string.Empty;

            _logger.LogInformation("AI responded successfully.");

            return Ok(new ChatResponse { Reply = reply });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while calling Semantic Kernel.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while processing your request." });
        }
    }
}

public record ChatRequest
{
    /// <summary>The message from the user.</summary>
    public string UserMessage { get; init; } = string.Empty;
}

public record ChatResponse
{
    /// <summary>The AI-generated reply.</summary>
    public string Reply { get; init; } = string.Empty;
}
