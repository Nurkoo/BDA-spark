using FluentValidation;

namespace BigDataAcademy.Api.Utilities;

public class PagedCriteria
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;

    public class Validator : AbstractValidator<PagedCriteria>
    {
        public Validator()
        {
            this.RuleFor(o => o.Page)
                .GreaterThan(0);

            this.RuleFor(o => o.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(50);
        }
    }
}
