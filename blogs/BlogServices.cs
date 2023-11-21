using NLog;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using NLog.LayoutRenderers;

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


    public Blog DisplayAllBlogs(BloggingContext db, Logger logger)
    {
        // display all blogs
        var blogs = db.Blogs.OrderBy(b => b.BlogId);
        foreach (Blog b in blogs)
        {
            Console.WriteLine($"{b.BlogId}: {b.Name}");
        }
        if (int.TryParse(Console.ReadLine(), out int BlogId))
        {
            Blog blog = db.Blogs.FirstOrDefault(b => b.BlogId == BlogId);
            if (blog != null)
            {
                logger.Info($"Blog (id: {blog.BlogId}) selected");
                return blog;
            }
        }
        logger.Error("Invalid Blog Id");

        return null;
    }

    public Blog GetBlog(int id, NLog.Logger logger)
    {
        logger.Info("Getting blog with id {id}", id);
        return _db.Blogs.FirstOrDefault(b => b.BlogId == id);
    }

    public Blog InputBlog(BloggingContext db, Logger logger)
    {
        Blog blog = new Blog();
        Console.WriteLine("Enter the Blog name");
        blog.Name = Console.ReadLine();

        ValidationContext context = new ValidationContext(blog, null, null);
        List<ValidationResult> results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(blog, context, results, true);
        if (isValid)
        {
            // prevent duplicate blog names
            if (db.Blogs.Any(b => b.Name == blog.Name))
            {
                // generate error
                results.Add(new ValidationResult("Blog name exists", new string[] { "Name" }));
            }
            else
            {
                return blog;
            }
        }
        foreach (var result in results)
        {
            logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
        }

        return null;
    }

    // Method to delete a blog. Assumes that the blog exists.
    public void DeleteBlog(int id)
    {
        var blog = _db.Blogs.FirstOrDefault(b => b.BlogId == id);

        if (blog != null)
        {
            _db.Blogs.Remove(blog);
            _db.SaveChanges();
        }
    }

    // Method to edit/update a blog. Assumes that the blog exists.
    public void EditBlog(Blog updatedBlog)
    {
        var existingBlog = _db.Blogs.FirstOrDefault(b => b.BlogId == updatedBlog.BlogId);

        if (existingBlog != null)
        {
            existingBlog.Name = updatedBlog.Name;
            _db.SaveChanges();
        }
    }

}


