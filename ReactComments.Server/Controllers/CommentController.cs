using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactComments.Server.Extensions;
using ReactComments.Server.Filters;
using ReactComments.Services.Abstraction;
using ReactComments.Services.Model;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace ReactComments.Server.Controllers
{
    
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
        [AutoValidation]
        [HttpPost("add")]
        public async Task<IActionResult> AddComment([FromForm] SubmitComment submitComment)
        {
            var result = await commentService.AddCommentAsync(submitComment);

            return this.ActionResultByApiResult(result, logger);
        }

        [AutoValidation]
        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveComment([FromBody] object id)
        {
            var result = await commentService.DeleteCommentAsync(id);

            return this.ActionResultByApiResult(result, logger);
        }

        [AutoValidation]
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment([FromBody] SubmitComment comment)
        {
            var result = await commentService.UpdateCommentAsync(comment);

            return this.ActionResultByApiResult(result, logger);
        }

        [AutoValidation]
        [Authorize]
        [HttpPost("attach-file")]
        public async Task<IActionResult> UploadFile([FromBody] CommentUploadFile uploadFile)
        {
            var result = await commentService.UploadFileAsync(uploadFile);

            return this.ActionResultByApiResult(result, logger);
        }

        [AutoValidation]
        [Authorize]
        [HttpPost("add-reply")]
        public async Task<IActionResult> AddReply([FromForm] SubmitComment comment)
        {
            var result = await commentService.AddReplyAsync(comment);

            return this.ActionResultByApiResult(result, logger);
        }

        [AutoValidation]
        [HttpGet("list")]
        public async Task<IActionResult> ListComments()
        {
            var result = await commentService.GetCommentsAsync();

            return this.ActionResultByApiResult(result, logger);
        }

        [AutoValidation]
        [HttpGet("list-top-level")]
        public async Task<IActionResult> ListTopLevelComments()
        {
            var result = await commentService.GetTopLevelCommentsAsync();

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpGet("comment-details")]
        public async Task<IActionResult> CommentDetails([FromQuery] string id)
        {
            var result = await commentService.CommentDetailsAsync(id);

            return this.ActionResultByApiResult(result, logger);
        }
    }
}
