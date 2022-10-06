using ElasticSearchSample.Models;

namespace ElasticSearchSample.Pages
{
    public partial class FetchData
    {
        private List<Book>? books = null;
        private string keyword = "";
        private int TotalCount= 0;
        

        //protected override async Task OnInitializedAsync()
        //{
        //            var response = await client.GetAsync("https://localhost:7212/api/Products");

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadFromJsonAsync<List<Book>>();

        //                books = content;
        //            }
        //}

        /// <summary>
        /// 검색
        /// </summary>
        /// <returns></returns>
		public async Task ExecuteSearch()
		{
            if (keyword.Length > 0)
            {
                var response = await client.GetAsync($"https://localhost:7212/api/Products/Search/{keyword}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<Book>>();

                    books = content;
                    TotalCount = books == null ? 0 : books.Count();
                    StateHasChanged();
                }
            }
            else {
                TotalCount = 0;
                StateHasChanged();
            }
    
        }
    }
}
