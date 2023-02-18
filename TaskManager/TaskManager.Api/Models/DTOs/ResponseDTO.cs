using TaskManager.Api.Enums;

namespace TaskManager.Api.Models.DTOs
{
    public class ResponseDTO<T>
    {
        public ResponseDTO() { }

        public ErrorDTO Error { get; set; }
        public PaginationDTO Pagination { get; set; }
        public T Data { get; set; }
    }

    public class ErrorDTO
    {
        public ErrorDTO() { }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }

        public ErrorDTO(ErrorCodes Error)
        {
            ErrorCode = (int)Error;
            ErrorMessage = EnumHelper.GetDescription(Error);
        }

        public ErrorDTO(Exception exception, ErrorCodes Error)
        {
            ErrorCode = (int)Error;
            ErrorMessage = exception.Message;
        }
    }
}
