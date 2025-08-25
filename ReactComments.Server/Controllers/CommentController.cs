using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactComments.Server.Extensions;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace ReactComments.Server.Controllers
{
    [AutoValidation]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly ILogger<CommentController> logger;

        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            this.commentService = commentService;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO comment)
        {
            var result = await commentService.AddCommentAsync(comment);

            return this.ActionResultByApiResult(result, logger);
        }

        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveComment([FromBody] object id)
        {
            var result = await commentService.DeleteCommentAsync(id);

            return this.ActionResultByApiResult(result, logger);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO comment)
        {
            var result = await commentService.UpdateCommentAsync(comment);

            return this.ActionResultByApiResult(result, logger);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromBody] CommentUploadFile uploadFile)
        {
            var result = await commentService.UploadFileAsync(uploadFile);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListComments()
        {
            var result = await commentService.GetCommentsAsync();

            return this.ActionResultByApiResult(result, logger);
        }
    }
}
