using System;
using System.IO;
using System.Linq;
using NLog;
using NLog.Web;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {

                System.Console.WriteLine("1) Display blogs");
                System.Console.WriteLine("2) Add Blog");
                System.Console.WriteLine("3) Display posts");
                System.Console.WriteLine("4) Add Post");

                String input=  Console.ReadLine();
                if(input=="1"){
                    try{
                        var database= new BloggingContext();
                        new Post();

                        foreach(var Blog in database.DisplayBlogs()){
                            System.Console.Write($"ID>   {Blog.BlogId}");
                            System.Console.Write($"Name> {Blog.BlogId}");
                        }
                    }catch(Exception e){
                        logger.Error(e);
                        Console.WriteLine(e);
                        throw;
                    }
                }


                else if(input == "2"){
                    // Create and save a new Blog
                    Console.Write("Enter a name for a new Blog: ");
                    var name = Console.ReadLine();

                    var blog = new Blog { Name = name };

                    var db = new BloggingContext();
                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name);

                    // Display all Blogs from the database
                    var query = db.Blogs.OrderBy(b => b.Name);

                    Console.WriteLine("All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
                else if(input == "3"){
                    Console.WriteLine("Enter BlogId to display posts from: ");
                    
                    int blogId = Console.Read();

                    try
                    {
                        var db = new BloggingContext();

                        new Post();

                        db.FindPosts(blogId);

                        var count = 0;
                        foreach (var post in db.FindPosts(blogId))
                        {
                            if (count == 0) Console.WriteLine($"BlogId: {post.Blog.BlogId}  Name: {post.Blog.Name}");

                            Console.WriteLine($"ID: {post.PostId}");
                            Console.WriteLine($"Title: {post.Title}");
                            Console.WriteLine($"Content: {post.Content}");

                            count++;
                        }

                        Console.WriteLine($"Post(s): {count}");
                    }
                    catch (Exception e)
                    {
                        logger.Error(e);
                        Console.WriteLine(e);
                        throw;
                    }
                }

                else if(input == "4"){
                    System.Console.WriteLine("ID> ");
                    int blogId = Console.Read();

                    System.Console.WriteLine("Title> ");
                    var postTitle = Console.ReadLine();

                    System.Console.WriteLine("Content> ");
                    var Content = Console.ReadLine();

                    try{
                        var Blog = new Blog {BlogId = blogId};

                        var post = new Post {BlogId = Blog.BlogId, Content = Content, Title = postTitle};

                        var database = new BloggingContext();
                        new Post();

                        database.AddPost(post);
                    }catch(Exception e){
                        logger.Error(e);
                        System.Console.WriteLine(e);
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }


            logger.Info("Program ended");
        }
    }
}
