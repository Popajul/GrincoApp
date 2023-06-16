using grincoAppModels;
using grincoMessageApi.CIModels;
using grincoMessageApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace grincoMessageApi.Controllers
{

[ApiController]
[Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
            private readonly ILogger<ConversationsController> _logger;
            private readonly IConversationRepository _conversationRepository;
        
            public ConversationsController(ILogger<ConversationsController> logger, IConversationRepository ConversationRepository)
            {
                _logger = logger;
                _conversationRepository = ConversationRepository;
            }

            [HttpGet]
            [Route("by_participant/{participantId}")]
            public async Task<IActionResult> GetAllByParticipantId([FromRoute] int participantId, [FromQuery] bool withParticipants = false)
            {
                List<Conversation>conversations =  (await _conversationRepository.GetConversationsByParticipantId(participantId)).ToList();
                if(withParticipants)
                {
                    foreach (var conv in conversations)
                    {
                        conv.Participants = (await _conversationRepository.GetParticipants(conv.Id)).ToList();
                    }
                }
                
                return Ok(conversations);
            }

            [HttpGet]
            [Route("{id}")]
            public async Task<IActionResult> GetConversationById([FromRoute] int id)
            {
                var conversation =  await _conversationRepository.GetConversationById(id);
                conversation.Participants = (await _conversationRepository.GetParticipants(conversation.Id)).ToList();
                return Ok(conversation);
            }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateConversation([FromBody] PostCreateConversationModel body)
        {
            Conversation conversation  = new()
            {
                Description = body.Desc
            };
            var participantIds = body.ParticipantIds;
            int id =  await _conversationRepository.CreateConversation(conversation);
            if(participantIds != null && participantIds.Any())
            {
                _ = await _conversationRepository.AddParticipants(id,participantIds);
            }
            return Created("api/conversations/{id}", new {Id = id});
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteConversation([FromRoute] int id)
        {
            
            _ =  await _conversationRepository.DeleteConversation(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}/_addParticipants")]
        public async Task<IActionResult> AddParticipants([FromRoute] int id, [FromBody] List<int> participantIds)
        {
            
            _ =  await _conversationRepository.AddParticipants(id, participantIds);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}/_removeParticipant/{participantId}")]
        public async Task<IActionResult> RemoveParticipant([FromRoute] int id, [FromRoute] int participantId)
        {
            _ =  await _conversationRepository.RemoveParticipant(id, participantId);
            return NoContent();
        }
    }
}
