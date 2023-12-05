namespace RESTBlog.Models.DTOs;

public class AuthorDTO
{
  public int Id { get; set; }
  public string Name { get; set; }
  public List<Comment> Comments { get; set; }
}