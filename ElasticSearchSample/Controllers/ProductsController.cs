using ElasticSearchSample.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchSample.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ProductsController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elasticClient"></param>
        /// <param name="logger"></param>
        public ProductsController(
                            IElasticClient elasticClient,
                            ILogger<ProductsController> logger)
        {
            _logger = logger;
            _elasticClient = elasticClient;
        }

        /// <summary>
        ///  그냥 10개 가져오기 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var result = await _elasticClient.SearchAsync<Book>(s => s.Size(10));

            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);

            return Ok(result.Documents.ToList());
        }


        /// <summary>
        /// 검색 하기
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("Search/{keyword}")]
        public async Task<IActionResult> GetKeyword([FromRoute] string keyword)
        {
            var result = await _elasticClient.SearchAsync<Book>(
                             s => s.Query(
                                 q => q.QueryString(
                                     d => d.Query('*' + keyword + '*')
                                 )).Size(5000));

         
            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);
   
            return Ok(result.Documents.ToList());
        }

        /*
        [HttpGet("Search")]
        public async Task<IActionResult> GetSearch(string Authors) {

            var result = await _elasticClient.SearchAsync<Book>(
                    s => s.Query(
                        a => a.Match( c=> c.Field( doc => doc.Authors == Authors))));



            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);

            return Ok(result.Documents.ToList());
        }
        */

        /// <summary>
        /// Book 인덱스 전체 갯수 가져오기
        /// </summary>
        /// <returns></returns>
        [HttpGet("Count")]
        public async Task<IActionResult> GetTotalCount()
        {
            var result = await _elasticClient.CountAsync<Book>();

            return Ok(result.Count);
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> Post(Book product)
        {

            var indexResponse = await _elasticClient.IndexDocumentAsync(product);

            if (!indexResponse.IsValid)
            {
                var debugInfo = indexResponse.DebugInformation;
                var error = indexResponse.OriginalException;

                return BadRequest(error);
            }
          

            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);
            return Ok();
        }

        /// <summary>
        /// 테스트용 여러개 저장
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost("List")]
        public async Task<IActionResult> PostList(List<Book> products)
        {

            foreach (var item in products)
            {
                // Index product dto
                await _elasticClient.IndexDocumentAsync(item);
            }

            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);
            return Ok();
        }

    }
}
