using MyOwnWebsite.Application.Dtos;

namespace MyOwnWebsite.Application.Dtos;
public class GetProjectListPagedQuery : PaginationRequestParameter
{
    public string? SearchValue { get; set; }
    public string? FilteredBy { get; set; }
}