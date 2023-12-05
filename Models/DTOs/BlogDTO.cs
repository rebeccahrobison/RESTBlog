namespace RESTBlog.Models.DTOs;

public class BlogDTO
{
  public int Id { get; set; }
  public string Title { get; set; }
  public List<ArticleDTO> Articles { get; set; }
  public List<AuthorDTO> Authors { get; set; }
}