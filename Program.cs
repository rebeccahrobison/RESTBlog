using System.Reflection.Metadata.Ecma335;
using RESTBlog.Models;
using RESTBlog.Models.DTOs;

List<Author> authors = new List<Author>
{
    new Author {Id = 1, Name = "Neil Gaiman"},
    new Author {Id = 2, Name = "Agatha Christie"},
    new Author {Id = 3, Name = "Gabriel Garcia Marquez"},
    new Author {Id = 4, Name = "Toni Morrison"},
    new Author {Id = 5, Name = "Haruki Murakami"},
    new Author {Id = 6, Name = "Chimamanda Ngozi Adichie"},
};

List<Blog> blogs = new List<Blog>
{
    new Blog {Id = 1, Title = "Musings and Nonsense"},
    new Blog {Id = 2, Title = "Article Insights"},
    new Blog {Id = 3, Title = "Travel Tales"},
    new Blog {Id = 4, Title = "Phrasing Fundamentals"}
};

List<BlogAuthor> blogAuthors = new List<BlogAuthor>
{
    new BlogAuthor {Id = 1, AuthorId = 1, BlogId = 1},
    new BlogAuthor {Id = 2, AuthorId = 2, BlogId = 1},
    new BlogAuthor {Id = 3, AuthorId = 3, BlogId = 1},
    new BlogAuthor {Id = 4, AuthorId = 4, BlogId = 2},
    new BlogAuthor {Id = 5, AuthorId = 5, BlogId = 2},
    new BlogAuthor {Id = 6, AuthorId = 6, BlogId = 3},
    new BlogAuthor {Id = 7, AuthorId = 1, BlogId = 3},
    new BlogAuthor {Id = 8, AuthorId = 2, BlogId = 3},
    new BlogAuthor {Id = 9, AuthorId = 3, BlogId = 4},
    new BlogAuthor {Id = 10, AuthorId = 4, BlogId = 4},
};

List<Article> articles = new List<Article>
{
    new Article {Id = 1, Title = "Crows on my Walk", BlogId = 1, PostedOnDate = new DateTime(2022, 12, 5)},
    new Article {Id = 2, Title = "Rhymes with Orange", BlogId = 1, PostedOnDate = new DateTime(2021, 1, 5)},
    new Article {Id = 3, Title = "Best Ice Cream Bowls", BlogId = 1, PostedOnDate = new DateTime(2020, 12, 5)},
    new Article {Id = 4, Title = "Cuddles and Blankets", BlogId = 1, PostedOnDate = new DateTime(2019, 12, 5)},
    new Article {Id = 5, Title = "Longing for Texts", BlogId = 2, PostedOnDate = new DateTime(2023, 12, 5)},
    new Article {Id = 6, Title = "Writing is for the Birds", BlogId = 2, PostedOnDate = new DateTime(2023, 10, 6)},
    new Article {Id = 7, Title = "Jumping Over Writer's Block", BlogId = 2, PostedOnDate = new DateTime(2022, 1, 15)},
    new Article {Id = 8, Title = "The Dos and Don'ts of Article Writing", BlogId = 2, PostedOnDate = new DateTime(2021, 11, 2)},
    new Article {Id = 9, Title = "Submitting to the Submission Process", BlogId = 2, PostedOnDate = new DateTime(2021, 9, 27)},
    new Article {Id = 10, Title = "Wanderlust Chronicles", BlogId = 3, PostedOnDate = new DateTime(2023, 12, 5)},
    new Article {Id = 11, Title = "Cross-Country Writing", BlogId = 3, PostedOnDate = new DateTime(2023, 2, 22)},
    new Article {Id = 12, Title = "The Odyssey of a Novel", BlogId = 3, PostedOnDate = new DateTime(2023, 1, 23)},
    new Article {Id = 13, Title = "Seaside Reflections and Coastal Retreats", BlogId = 3, PostedOnDate = new DateTime(2022, 12, 25)},
    new Article {Id = 14, Title = "Magical Realms", BlogId = 3, PostedOnDate = new DateTime(2022, 12, 5)},
    new Article {Id = 15, Title = "Building Atmospheric Moods", BlogId = 4, PostedOnDate = new DateTime(2022, 12, 5)},
    new Article {Id = 16, Title = "Character Development in Prose", BlogId = 4, PostedOnDate = new DateTime(2022, 12, 5)},
    new Article {Id = 17, Title = "The Rhythm of Prose", BlogId = 4, PostedOnDate = new DateTime(2022, 12, 5)},
};

List<Comment> comments = new List<Comment>
{
    new Comment {Id = 1, ArticleId = 1, AuthorId = 6, BlogId = 1},
    new Comment {Id = 2, ArticleId = 1, AuthorId = 5, BlogId = 1},
    new Comment {Id = 3, ArticleId = 1, AuthorId = 4, BlogId = 1},
    new Comment {Id = 4, ArticleId = 2, AuthorId = 2, BlogId = 1},
    new Comment {Id = 5, ArticleId = 7, AuthorId = 1, BlogId = 2},
    new Comment {Id = 6, ArticleId = 7, AuthorId = 1, BlogId = 2},
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/blogs", () =>
{
    return blogs.Select(b => new BlogDTO
    {
        Id = b.Id,
        Title = b.Title,
    });
});

app.MapPost("/blogs", (Blog blog) =>
{
    blog.Id = blogs.Max(b => b.Id) + 1;

    blogs.Add(blog);

    return Results.Created($"/blogs/{blog.Id}", new BlogDTO
    {
        Id = blog.Id,
        Title = blog.Title
    });
});

app.MapPost("/blogs/{id}", (int id) =>
{
    Blog blog = blogs.FirstOrDefault(b => b.Id == id);
    if (blog == null)
    {
        return Results.NotFound();
    }
    List<Article> foundArticles = articles.Where(a => a.BlogId == blog.Id).ToList();
    List<BlogAuthor> foundBlogAuthors = blogAuthors.Where(ba => ba.BlogId == id).ToList();
    List<Author> foundAuthors = foundBlogAuthors
        .Select(ba => authors.FirstOrDefault(a => a.Id == ba.AuthorId))
        .Where(a => a != null)
        .ToList();

    return Results.Ok(new BlogDTO
    {
        Id = blog.Id,
        Title = blog.Title,
        Articles = foundArticles.Select(fa => new ArticleDTO
        {
            Id = fa.Id,
            Title = fa.Title,
            BlogId = id
        }).ToList(),
        Authors = foundAuthors.Select(fa => new AuthorDTO
        {
            Id = fa.Id,
            Name = fa.Name
        }).ToList()
    });
});

//Retrieve the articles for a blog
app.MapGet("/blogs/{id}/articles", (int id) =>
{
    Blog blog = blogs.FirstOrDefault(b => b.Id == id);
    if (blog == null)
    {
        return Results.NotFound();
    }

    List<Article> foundArticles = articles.Where(a => a.BlogId == blog.Id).ToList();

    List<ArticleDTO> articleDTOs = foundArticles.Select(fa => new ArticleDTO
    {
        Id = fa.Id,
        Title = fa.Title,
        BlogId = blog.Id,
    }).ToList();
    
    return Results.Ok(articleDTOs);
});


app.MapGet("/articles", () =>
{
    return articles.Select(a => new ArticleDTO
    {
        Id = a.Id,
        Title = a.Title,
        BlogId = a.BlogId,
        PostedOnDate = a.PostedOnDate
    });
});

//Create a comment on a specific article
app.MapPost("/articles/{id}/comments", (int id, Comment comment) =>
{
    comment.Id = comments.Max(c => c.Id) + 1;
    var article = articles.FirstOrDefault(a => a.Id == id);
    if (article == null)
    {
        return Results.NotFound();
    }

    comments.Add(comment);
    return Results.Created($"/articles/{id}/comments/{comment.Id}", new CommentDTO
    {
        Id = comment.Id,
        BlogId = article.BlogId,
        ArticleId = id,
        AuthorId = comment.AuthorId
    });
});

//Update an article
app.MapPut("/articles/{id}", (int id, Article article) =>
{
    Article articleToUpdate = articles.FirstOrDefault(a => a.Id == id);
    if(articleToUpdate == null)
    {
        return Results.NotFound();
    }
    if (id != article.Id)
    {
        return Results.BadRequest();
    }

    articleToUpdate.Title = article.Title;

    return Results.NoContent();
});

app.MapGet("/comments", () =>
{
    return comments.Select(c => new CommentDTO
    {
        Id = c.Id,
        BlogId = c.BlogId,
        ArticleId = c.ArticleId,
        AuthorId = c.AuthorId
    });
});

//Delete a comment
app.MapDelete("/comments/{id}", (int id) =>
{
    Comment commentToDelete = comments.FirstOrDefault(c => c.Id == id);
    if(commentToDelete == null)
    {
        return Results.NotFound();
    }

    comments.Remove(commentToDelete);

    return Results.NoContent();
});

//Retrieve comments by a particular author
app.MapGet("/authors/{id}/comments", (int id) =>
{
    var author = authors.FirstOrDefault(a => a.Id == id);
    if (author == null)
    {
        return Results.NotFound();
    }

    List<Comment> authorComments = comments.Where(c => c.AuthorId == id).ToList();

    return Results.Ok(authorComments.Select(ac => new CommentDTO
    {
        Id = ac.Id,
        BlogId = ac.BlogId,
        ArticleId = ac.BlogId,
        AuthorId = id
    }));
});

//Retrieve Blogs that include a particular author
app.MapGet("/authors/{id}", (int id) =>
{
    Author author = authors.FirstOrDefault(a => a.Id == id);
    if (author == null)
    {
        return Results.NotFound();
    }

    List<BlogAuthor> foundBlogAuthors = blogAuthors.Where(ba => ba.AuthorId == id).ToList();
    List<Blog> foundBlogs = foundBlogAuthors
        .Select(ba => blogs.FirstOrDefault(b => b.Id == ba.BlogId))
        .Where(b => b != null)
        .ToList();

    return Results.Ok(foundBlogs.Select(fb => new BlogDTO
    {
        Id = fb.Id,
        Title = fb.Title,
    }));
});

//Delete all articles for a blog
app.MapDelete("blogs/{id}/articles", (int id) =>
{
    Blog blog = blogs.FirstOrDefault(b => b.Id == id);
    if (blog == null)
    {
        return Results.NotFound();
    }

    List<Article> articlesToDelete = articles.Where(a => a.BlogId == id).ToList();

    articles.RemoveAll(a => articlesToDelete.Contains(a));

    return Results.NoContent();
});

//Retrieve recently posted articles
app.MapGet("/articles/recent", () =>
{
    DateTime threeMonthsAgo = DateTime.Today.AddMonths(-3); 
    List<Article> recentArticles = articles.Where(a => a.PostedOnDate >= threeMonthsAgo).ToList();

    return recentArticles.Select(ra => new ArticleDTO
    {
        Id = ra.Id,
        Title = ra.Title,
        BlogId = ra.BlogId,
        PostedOnDate = ra.PostedOnDate
    });
});

//Add an Author to a Blog
app.MapPost("/blogauthor", (BlogAuthor blogAuthor) =>
{
    blogAuthor.Id = blogAuthors.Max(ba => ba.Id) + 1;
    Blog blog = blogs.FirstOrDefault(b => b.Id == blogAuthor.BlogId);
    Author author = authors.FirstOrDefault(a => a.Id == blogAuthor.AuthorId);
    if (blog == null || author == null)
    {
        return Results.BadRequest();
    }

    blogAuthors.Add(blogAuthor);

    return Results.Created($"/blogauthor/{blogAuthor.Id}", new BlogAuthorDTO
    {
        Id = blogAuthor.Id,
        BlogId = blogAuthor.BlogId,
        AuthorId = blogAuthor.AuthorId
    });
});

//Update all articles in a blog
app.MapPut("blogs/{id}/updatearticles", (int id, List<Article> articlesToUpdate) =>
{
    Blog blog = blogs.FirstOrDefault(b => b.Id == id);
    if (blog == null)
    {
        return Results.NotFound();
    }
    List<Article> blogsArticles = articles.Where(a => a.BlogId == id).ToList();
    foreach (Article atu in articlesToUpdate)
    {
        
        Article existingArticle = blogsArticles.FirstOrDefault(a => a.Id == atu.Id);

        if (existingArticle != null)
        {
            existingArticle.Title = atu.Title;
            existingArticle.PostedOnDate = atu.PostedOnDate;
            existingArticle.Comments = atu.Comments;
        }
    }
    return Results.NoContent();
});

//Get all comments on articles on a blog
app.MapGet("/blogs/{id}/comments", (int id) =>
{
    //make a list of all comments
    Blog blog = blogs.FirstOrDefault(b => b.Id == id);
    if (blog == null)
    {
        return Results.NotFound();
    }

    List<Comment> foundComments = comments.Where(c => c.BlogId == id).ToList();

    return Results.Ok(foundComments.Select(fc => new CommentDTO
    {
        Id = fc.Id,
        BlogId = fc.BlogId,
        ArticleId = fc.ArticleId,
        AuthorId = fc.AuthorId
    }));
});


app.Run();

