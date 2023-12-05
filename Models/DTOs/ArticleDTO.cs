namespace RESTBlog.Models.DTOs;

public class ArticleDTO
{
  public int Id { get; set; }
  public string Title { get; set; }
  public int BlogId { get; set; }
  public List<CommentDTO> Comments { get; set; }
  public DateTime PostedOnDate { get; set; }
}