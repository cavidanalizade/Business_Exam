using FluentValidation;

namespace BusinessExam.Areas.Manage.ViewModels
{
    public class CreateBlogVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime CreatedAt { get; set; }

    }
    public class CreateBlogValidator : AbstractValidator<CreateBlogVM>
    {
        public CreateBlogValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title bos ola bilmez");
            RuleFor(x => x.Title).MinimumLength(3).WithMessage("Title 3 den kicik ola bilmez");
            RuleFor(x => x.Title).MaximumLength(25).WithMessage("Title 25 den boyuk ola bilmez");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description  bos ola bilmez");
            RuleFor(x => x.Description).MinimumLength(3).WithMessage("Description 3 den kicik ola bilmez");
            RuleFor(x => x.Description).MaximumLength(100).WithMessage("Description 100 den boyuk ola bilmez");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image bos olmamalidi");
        }
    }
}
