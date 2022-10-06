using ElasticSearchSample.Models;
using Microsoft.AspNetCore.Components;

namespace ElasticSearchSample.Pages
{
    public partial class FetchData
    {
        private List<Book>? books = null;
        private string keyword = "";
        private int TotalCount= 0;
        //protected override async Task OnInitializedAsync()
        //{

        //    using (var httpClientHandler = new HttpClientHandler())
        //    {
        //        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

        //        using (var client = new HttpClient(httpClientHandler))
        //        {
        //            var response = await client.GetAsync("https://localhost:7212/api/Products");

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadFromJsonAsync<List<Book>>();

        //                books = content;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public async Task ExecuteSearch()
		{
            if (keyword.Length > 0)
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using (var client = new HttpClient(httpClientHandler))
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
                }
            }
            else {
                TotalCount = 0;
                StateHasChanged();
            }
    
        }
    }
}
