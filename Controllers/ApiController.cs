using BlogASP.Models;
using BlogASP.Repository;
using Microsoft.AspNetCore.Mvc;

[Route("api/Articles")]
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
        try
        {
            var createdArticle = _articleRepository.APICreateArticle(data, User.Identity.Name);
            return Ok(createdArticle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ArticleModel data)
    {
        try
        {
            var updatedArticle = _articleRepository.APIEditArticle(id, data, User.Identity.Name);
            return Ok(updatedArticle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpDelete("articles/{id}")]
    public IActionResult DeleteArticle(int id)
    {
        try
        {
            var deletedArticle = _articleRepository.DeleteArticle(id);
            if (deletedArticle)
            {
                return Ok("Article deleted successfully");
            }
            else
            {
                return NotFound("Article not found");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}

[Route("api/Users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }
        catch (Exception ex)
        {
            var sampleUsers = new List<UserModel>
            {
                new UserModel { UserId = 1, Username = "User1" },
                new UserModel { UserId = 2, Username = "User2" },
            };

            return Ok(sampleUsers);
        }
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserModel data)
    {
        try
        {
            var createdUser = _userRepository.Create(data);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpPut("{id}")]
    public IActionResult PutUser(int id, [FromBody] UserModel data)
    {
        try
        {
            var updatedUser = _userRepository.Edit(data);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Server Error");
        }
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var isDeleted = _userRepository.APIDeleteUser(id);

            if (isDeleted)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return NotFound("User not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

}