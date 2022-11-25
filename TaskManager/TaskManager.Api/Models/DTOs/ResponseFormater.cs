using TaskManager.Api.Enums;

namespace TaskManager.Api.Models.DTOs
{
    public class ResponseFormater
    {
        public static ResponseDTO<T> OK<T>(T data, PaginationDTO page = null)
        {
            var response = new ResponseDTO<T>()
            {
                Data = data,
                Pagination = page
            };

            return response;
        }

        public static ResponseDTO<Exception> Error(ErrorCodes statusCode)
        {
            var response = new ResponseDTO<Exception>
            {
                Error = new ErrorDTO(statusCode),
            };

            return response;
        }
    }

}
