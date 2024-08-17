using FluentValidation;

namespace Service.Blog.Dto;

public record PostsQuery(int Page = 0);

public record CommentFormData(string Content);

public class CommentFormDataValidator : AbstractValidator<CommentFormData>
{
    public CommentFormDataValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
    }
}
