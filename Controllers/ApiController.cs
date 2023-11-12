using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Mvc;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;

    public ApiController(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var articles = _articleRepository.GetAllArticles();
            return Ok(articles);
        }
        catch (Exception ex)
        {
            var articles = new List<ArticleModel>
            {
                new ArticleModel { ArticleId = 1, Title = "Sample Article 1" },
                new ArticleModel { ArticleId = 2, Title = "Sample Article 2" },
            };

            return Ok(articles);
        }
    }

    [HttpPost]
    public IActionResult Post([FromBody] ArticleModel data)
    {
        return Ok("Data received!");
    }
}
