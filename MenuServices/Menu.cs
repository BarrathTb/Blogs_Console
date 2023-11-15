using NLog.Targets.Wrappers;

public static class Menu
{
    static BloggingContext dbContext = new BloggingContext();
    static BlogServices blogServices = new BlogServices(dbContext);

    public static void DisplayMainMenu()
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Display all blogs");
        Console.WriteLine("2. Add Blog");
        Console.WriteLine("3. Create Post");
        Console.WriteLine("4. Display Posts");
    }

    public static void DisplaySelectBlogForPostMenu()
    {
        var displayBlogs = blogServices.GetBlogList();
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
