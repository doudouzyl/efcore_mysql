using Microsoft.EntityFrameworkCore;

// create new blog posts
using (var context = new BlogDataContext())
{
    var john = new Author { Name = "John T. Author", Email = "john@example.com" };
    context.Authors.Add(john);

    var jane = new Author { Name = "Jane Q. Hacker", Email = "jane@example.com" };
    context.Authors.Add(jane);

    var post = new Post { Title = "Hello World", Content = "I wrote an app using EF Core!", Author = jane };
    context.Posts.Add(post);
    post = new Post { Title = "How to use EF Core", Content = "It's pretty easy", Author = john };
    context.Posts.Add(post);

    context.SaveChanges();
}

// query the blog posts, using a join between the two tables
using (var context = new BlogDataContext())
{
    var posts = context.Posts
        .Include(p => p.Author)
        .ToList();

    foreach (var post in posts)
    {
        Console.WriteLine($"{post.Title} by {post.Author.Name}");
    }
}

public class BlogDataContext : DbContext
{
    static readonly string connectionString = "Server=; UserID=; Password=; Database=blog; SslMode=2; SslCa=/home/my-MySQL8.pem";
    public DbSet<Author> Authors { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Author Author { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public List<Post> Posts { get; set; }
}