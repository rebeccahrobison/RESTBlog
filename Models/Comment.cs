namespace RESTBlog.Models;

public class Comment
{
  public int Id { get; set; }
  public int BlogId { get; set; }
  public int ArticleId { get; set; }
  public int AuthorId { get; set; }
}
