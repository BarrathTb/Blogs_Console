using NLog;
using System.Linq;
using System.ComponentModel.DataAnnotations;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");



try
{
    string choice;

    do
    {
        var db = new BloggingContext();
        var blogServices = new BlogServices(db);
        var postServices = new PostServices(db);
        var menu = new Menu();
        menu.DisplayMainMenu();

        choice = Console.ReadLine();

        Console.Clear();
        logger.Info("Option {choice} selected", choice);

        if (choice == "1")
        {
            // Display blogs
            var blogs = blogServices.GetBlogList();
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.Name);
            }
        }
        else if (choice == "2")
        {
            // Add blog
            Blog createdBlog = blogServices.InputBlog(db, logger);

            if (createdBlog != null)
            {
                blogServices.AddBlog(createdBlog);
                logger.Info("Blog added - {name}", createdBlog.Name);
            }
        }
        else if (choice == "3")
        {
            menu.DisplaySelectBlogForPostMenu();

            Post newPost = postServices.InputPost(db, logger);
            if (newPost != null)
            {
                postServices.AddPost(newPost);
                logger.Info("Post created: {title}", newPost.Title);
            }
        }

        else if (choice == "4")
        {
            menu.DisplaySelectBlogForPostMenu();

            int blogId = Convert.ToInt32(Console.ReadLine());

            var blogPosts = postServices.GetPosts(logger).Where(p => p.BlogId == blogId);
            foreach (var post in blogPosts)
            {
                Console.WriteLine($"{post.PostId}: {post.Title}");
            }
        }
        else if (choice == "5")
        {
            blogServices.DisplayAllBlogs(db, logger);
            // Delete blog
            Console.WriteLine("Enter the id of the blog to delete:");
            int blogId = Convert.ToInt32(Console.ReadLine());

            blogServices.DeleteBlog(blogId);
            logger.Info($"Blog (id: {blogId}) deleted");
        }
        else if (choice == "6")
        {
            blogServices.DisplayAllBlogs(db, logger);

            // Edit blog
            Console.WriteLine("Enter the id of the blog to edit:");
            int blogId = Convert.ToInt32(Console.ReadLine());

            Blog updatedBlog = blogServices.InputBlog(db, logger);
            updatedBlog.BlogId = blogId;

            blogServices.EditBlog(updatedBlog);
            logger.Info($"Blog (id: {blogId}) updated");
        }

        Console.WriteLine();

    } while (choice.ToLower() != "q");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");




