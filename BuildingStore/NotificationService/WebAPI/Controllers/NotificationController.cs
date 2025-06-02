using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.MediatConfiguration.Queries;
using NotificationService.Domain.Collections;

namespace NotificationService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Отправка уведомлений.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="cancellationToken"></param>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromQuery] string subject, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SendEmailCommand(subject), cancellationToken);

            return Ok(new { result.Message });
        }

        /// <summary>
        /// Добавление уведомления.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddEmail([FromBody] MessageModel model, CancellationToken cancellationToken)
        {
            var message = await _mediator.Send(new AddMessageCommand(model), cancellationToken);
            return Ok(new { message.Message });
        }

        /// <summary>
        /// Получение всех уведомлений.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailMessage>>> GetMessages([FromQuery] int page, [FromQuery] int size, CancellationToken cancellation)
        {
            var messages = await _mediator.Send(new GetAllMessagesQuery(page, size), cancellation);
            return Ok(new { messages.Data });
        }

        /// <summary>
        /// Удаление уведомления по идентификатору.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage([FromRoute] string messageId, CancellationToken cancellation)
        {
            var messages = await _mediator.Send(new DeleteMessageCommand(messageId), cancellation);
            return Ok(new { messages.Message });
        }

        /// <summary>
        /// Получение сообщения по идентификатору.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpGet("{messageId}")]
        public async Task<ActionResult> GetMessageById([FromRoute] string messageId, CancellationToken cancellation)
        {
            var messages = await _mediator.Send(new GetMessageByIdQuery(messageId), cancellation);
            return Ok(new { messages.Data });
        }

        /// <summary>
        /// Обновление сообщения.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellation"></param>
        [Authorize(Policy = "AdminManagerPolicy")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMessage([FromRoute] string id, [FromBody] MessageModel model, CancellationToken cancellation)
        {
            var message = await _mediator.Send(new UpdateMessageCommand(id, model), cancellation);
            return Ok(new { message.Message });
        }
    }
}
