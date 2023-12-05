namespace RESTBlog.Models;

public class Blog
{
  public int Id { get; set; }
  public string Title { get; set; }
  public List<Article> Articles { get; set; }
  public List<Author> Authors { get; set; }
}