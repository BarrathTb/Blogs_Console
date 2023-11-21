using System.ComponentModel.DataAnnotations;
using NLog.LayoutRenderers;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

public class PostServices
{
    private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    private readonly BloggingContext _db;

    public PostServices(BloggingContext db)
    {
        _db = db;

    }

    public void AddPost(Post post)
    {
        _db.Posts.Add(post);
        _db.SaveChanges();
    }

    public void DeletePost(int postId, NLog.Logger logger)
    {
        var post = _db.Posts.FirstOrDefault(p => p.PostId == postId);
        if (post != null)
        {
            _db.Posts.Remove(post);
            _db.SaveChanges();
        }
        logger.Info($"Post (id: {postId}) deleted");
    }

    public void UpdatePost(Post updatedPost, NLog.Logger logger)
    {
        var existingPost = _db.Posts.FirstOrDefault(p => p.PostId == updatedPost.PostId);
        if (existingPost != null)
        {
            existingPost.Content = updatedPost.Content;
            existingPost.Title = updatedPost.Title;
            _db.SaveChanges();
        }
        logger.Info($"Post (id: {updatedPost.PostId}) updated");
    }

    public List<Post> GetPosts(NLog.Logger logger)
    {
        logger.Info("Getting all posts");
        return _db.Posts.ToList();
    }

    public Post GetPost(int id, NLog.Logger logger)
    {
        logger.Info($"Post (id: {id}) returned");
        return _db.Posts.FirstOrDefault(p => p.PostId == id);

    }

    public Post InputPost(BloggingContext db, NLog.Logger logger)
    {
        Post post = new Post();
        Console.WriteLine("Enter the Blog id");
        post.BlogId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the Post title");
        post.Title = Console.ReadLine();
        Console.WriteLine("Enter the Post content");
        post.Content = Console.ReadLine();

        ValidationContext context = new ValidationContext(post, null, null);
        List<ValidationResult> results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(post, context, results, true);
        if (isValid)
        {
            return post;
        }

        foreach (var result in results)
        {
            logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
        }

        return null;
    }

}


