using System.ComponentModel.DataAnnotations;

namespace TravelingApp.Application.Response
{
    public interface IFrameworkResponse<T> 
    {
        T? Data { get; set; }
        int Count { get; set; }
    }


    public class FrameworkResponse<TResponseDto> : IFrameworkResponse<TResponseDto> where TResponseDto : ResponseDto
    {
        public TResponseDto? Data { get; set; }
        public int Count { get; set; }
        public IEnumerable<ValidationResult>? Errors { get; set; } = [];
        public bool Success
        {
            get { return Errors == null || !Errors.Any(); }
        }
    }
}
