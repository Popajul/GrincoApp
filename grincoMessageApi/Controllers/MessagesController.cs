using grincoAppModels;
using grincoMessageApi.CIModels;
using grincoMessageApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace grincoMessageApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IMessageRepository _messageRepository;

    public MessagesController(ILogger<UsersController> logger, IMessageRepository messageRepository, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _messageRepository = messageRepository;
    }

    [HttpGet]
    [Route("{participantId}")]
    public async Task<IActionResult> GetMessagesByParticipantIdOrderByConversation([FromRoute] int participantId)
    {
        var messages =  await _messageRepository.GetMessagesByParticipantIdOrderByConversation(participantId);
        return Ok(messages);
    }

    [HttpGet]
    [Route("conversations/{conversationId}")]
    public async Task<IActionResult> GetMessagesByConversation([FromRoute] int conversationId)
    {
        var messages =  await _messageRepository.GetMessagesByConversation(conversationId);
        return Ok(messages);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateMessage([FromBody] PostCreateMessageModel body)
    {
        Message message  = new()
        {
            Content = body.Content,
            ConversationId = body.ConversationId,
            SenderId = body.Senderid
        };
        int id =  await _messageRepository.CreateMessage(message);
        return Created("api/messages/{id}", new {Id = id});
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] int id)
    {
        
        _ =  await _messageRepository.DeleteMessageById(id);
        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateMessage([FromRoute] int id, [FromBody] string content)
    {
        
        _ =  await _messageRepository.UpdateMessageById(id, content);
        return NoContent();
    }
}
