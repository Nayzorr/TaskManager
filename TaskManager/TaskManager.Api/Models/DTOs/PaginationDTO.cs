using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models.DTOs
{
    public class PaginationDTO
    {
        public int RecordsReturned { get; set; }
        public long TotalRecordsFound { get; set; }

        [Required, DefaultValue(1)]
        public int CurrentPage { get; set; }

        [Required, DefaultValue(25)]
        public int RecordsPerPage { get; set; }
    }
}
