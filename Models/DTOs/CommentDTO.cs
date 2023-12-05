namespace RESTBlog.Models;

public class CommentDTO
{
  public int Id { get; set; }
  public int BlogId { get; set; }
  public int ArticleId { get; set; }
  public int AuthorId { get; set; }
}