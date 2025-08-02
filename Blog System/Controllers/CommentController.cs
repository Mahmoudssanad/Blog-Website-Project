using Blog_System.Models.Entities;
using Blog_System.Servicies;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog_System.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int postId)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comments = await _commentService.GetCommentsAsync(postId);

            return View(comments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int postId)
        {
            if (postId <= 0)
                return BadRequest("postId is required");

            var post = await _commentService.PostDetail(postId);

            var comments = await _commentService.GetCommentsAsync(postId);

            PostDetailsViewModel postDetails = new PostDetailsViewModel
            {
                Post = post,
                Comments = comments,
                NewComment = new Comment()
                {
                    PostId = postId
                }
            };
            postDetails.NewComment.PostId = postDetails.Post.Id;

            return View(postDetails);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                ModelState.AddModelError("Content", "محتوى التعليق مطلوب.");
                return RedirectToAction("Details", new { postId = comment.PostId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.Now;



            await _commentService.AddAsync(comment);

            return RedirectToAction("Index", "Home");
        }
    }
}
