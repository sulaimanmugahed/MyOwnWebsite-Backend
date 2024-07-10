using MyOwnWebsite.Application.Dtos;

namespace MyOwnWebsite.Application.Wrappers
{
    public class PagedResponse<T> : BaseResult<List<T>>
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public PagedResponse()
        {

        }
        public PagedResponse(Error error) : base(error)
        {
        }

        public PagedResponse(PaginationResponseDto<T> model, PaginationRequestParameter request)
        {
            PageNumber = request.PageNumber;
            PageSize = request.PageSize;
            TotalItems = model.Count;
            TotalPages = TotalItems / PageSize;
            if (TotalItems % PageSize > 0) TotalPages++;

            this.Data = model.Data;
            this.Success = true;
        }
        public PagedResponse(PaginationResponseDto<T> model, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = model.Count;
            TotalPages = TotalItems / PageSize;
            if (TotalItems % PageSize > 0) TotalPages++;

            this.Data = model.Data;
            this.Success = true;
        }
    }
}
