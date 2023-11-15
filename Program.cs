using NLog;
using System.Data.Common;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

var db = new BloggingContext();
var blogs = db.Blogs.ToList();
while (true)
{
    Menu.DisplayMainMenu();


    string option = Console.ReadLine();
    switch (option)
    {
        case "1":
            {
                var blogService = new BlogServices(db);
                var blogsList = blogService.GetBlogList();
                foreach (var blogItem in blogsList)
                    Console.WriteLine($"Blog: {blogItem.Name}");

                break;
            }
        case "2":
            {
                Console.WriteLine("Enter name for new blog:");
                string blogName = Console.ReadLine();
                var blogService = new BlogServices(db);
                blogService.AddBlog(new Blog { Name = blogName });
                Console.WriteLine("Blog added successfully!");
                break;
            }

        case "3":
            {
                Menu.DisplaySelectBlogForPostMenu();
                if (int.TryParse(Console.ReadLine(), out int blogId))
                {
                    var blogService = new BlogServices(db);
                    var selectedBlog = blogService.GetBlog(blogId);


                    if (selectedBlog != null)
                    {
                        Console.WriteLine("Enter title for new post:");
                        string postTitle = Console.ReadLine();

                        Console.WriteLine("Enter content for new post:");
                        string postContent = Console.ReadLine();

                        var postService = new PostServices(db);
                        postService.AddPost(new Post { BlogId = blogId, Title = postTitle, Content = postContent });

                        Console.WriteLine("Post added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Blog not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid ID.");
                }
                break;
            }
        case "4":
            {
                Menu.DisplaySelectBlogForViewingPostsMenu();
                if (int.TryParse(Console.ReadLine(), out int blogId))
                {
                    var postService = new PostServices(db);
                    var posts = postService.GetPosts().Where(p => p.BlogId == blogId).ToList();

                    if (posts.Any())
                    {
                        Console.WriteLine($"Total {posts.Count} post(s) found for blog ID {blogId}");
                        foreach (var post in posts)
                            Console.WriteLine($"Blog: {post.Blog.Name}, Title: {post.Title}, Content: {post.Content}");
                    }
                    else
                    {
                        Console.WriteLine("No posts found for this blog.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid ID.");
                }
                break;
            }

    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey(true);
    Console.Clear();
    logger.Info("Program ended");
}


