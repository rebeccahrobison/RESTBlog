namespace RESTBlog.Models;

public class Article
{
  public int Id { get; set; }
  public string Title { get; set; }
  public int BlogId { get; set; }
  public List<Comment> Comments { get; set; }
    public DateTime PostedOnDate { get; set; }
}