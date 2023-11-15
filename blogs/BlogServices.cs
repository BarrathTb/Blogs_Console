public class BlogServices
{
    private readonly BloggingContext _db;

    public BlogServices(BloggingContext db)
    {
        _db = db;
    }

    public void AddBlog(Blog blog)
    {
        _db.Blogs.Add(blog);
        _db.SaveChanges();
    }

    public List<Blog> GetBlogList()
    {
        return _db.Blogs.ToList();
    }

    public static void DisplayAllBlogs(BloggingContext db)
    {
        var query = db.Blogs.OrderBy(b => b.Name);
        Console.WriteLine("All blogs in the database:");
        foreach (var item in query)
        {
            Console.WriteLine($"{item.BlogId.ToString()}:  {item.Name}");
        }
    }

    public Blog GetBlog(int id)
    {
        return _db.Blogs.FirstOrDefault(b => b.BlogId == id);
    }

}


