namespace API.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currenctPage, int totalItems, int itemsPerPage, int totalPages)
        {
            CurrenctPage = currenctPage;
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
        }

        public int CurrenctPage { get; set; }
        
        public int TotalItems { get; set; }
        
        public int ItemsPerPage { get; set; }
        
        public int TotalPages { get; set; }
    }
}