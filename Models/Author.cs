namespace RESTBlog.Models;

public class Author
{
  public int Id { get; set; }
  public string Name { get; set; }
  public List<Comment> Comments { get; set; }
}