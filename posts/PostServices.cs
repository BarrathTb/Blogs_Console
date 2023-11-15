public class PostServices
{
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

    public void DeletePost(int postId)
    {
        var post = _db.Posts.FirstOrDefault(p => p.PostId == postId);
        if (post != null)
        {
            _db.Posts.Remove(post);
            _db.SaveChanges();
        }
    }

    public void UpdatePost(Post updatedPost)
    {
        var existingPost = _db.Posts.FirstOrDefault(p => p.PostId == updatedPost.PostId);
        if (existingPost != null)
        {
            existingPost.Content = updatedPost.Content;
            existingPost.Title = updatedPost.Title;
            _db.SaveChanges();
        }
    }

    public List<Post> GetPosts()
    {
        return _db.Posts.ToList();
    }

    public Post GetPost(int id)
    {
        return _db.Posts.FirstOrDefault(p => p.PostId == id);
    }
}
