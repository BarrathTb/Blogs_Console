using NLog.Targets.Wrappers;

public class Menu
{
    static BloggingContext dbContext = new BloggingContext();
    static BlogServices blogServices = new BlogServices(dbContext);

    public void DisplayMainMenu()
    {
        Console.WriteLine("Enter your selection:");
        Console.WriteLine("1) Display all blogs");
        Console.WriteLine("2) Add Blog");
        Console.WriteLine("3) Create Post");
        Console.WriteLine("4) Display Posts");
        Console.WriteLine("5) Delete Blog");
        Console.WriteLine("6) Edit Blog");
        Console.WriteLine("Enter q to quit");
    }

    private BlogServices _blogServices;

    public Menu()
    {
        _blogServices = new BlogServices(new BloggingContext());
    }

    public void DisplaySelectBlogForPostMenu()
    {
        var displayBlogs = _blogServices.GetBlogList();
        foreach (var blog in displayBlogs)
        {
            Console.WriteLine($"{blog.BlogId}: {blog.Name}");
        }
        Console.WriteLine("\nEnter the ID of the blog you are posting to:");
    }


    public static void DisplaySelectBlogForViewingPostsMenu()
    {
        var displayBlogs = blogServices.GetBlogList();
        foreach (var blog in displayBlogs)
        {
            Console.WriteLine($"{blog.BlogId}: {blog.Name}");
        }
        Console.WriteLine("\nEnter the ID of the blog whose posts you want to view:");
    }

}
