using System.Security.Claims;

namespace Service.Blog;

public interface IBlogService
{
    Dto.PostDetail GetById(long id);
    IEnumerable<Dto.Post> Newest(Dto.PostsQuery query);
    Task<long> CreateComment(ClaimsPrincipal principal, long postId, Dto.CommentFormData data);
}
