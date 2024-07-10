
namespace MyOwnWebsite.Application.Dtos
{
    public class PaginationResponseDto<T>
    {
        public PaginationResponseDto(List<T> data, int count)
        {
            Data = data;
            Count = count;
        }
        public List<T> Data { get; set; }
        public int Count { get; set; }
    }
}
