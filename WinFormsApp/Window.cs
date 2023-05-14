using Data.Access;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WinFormsApp
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseContext databaseContext = new DatabaseContext();

            //databaseContext.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            //databaseContext.SaveChanges();

            //DbSet<Blog> blogs = databaseContext.Blogs;

            //var data = from blog in blogs
            //           where blog.BlogId == 1
            //           select blog;

            //foreach (Blog blog in blogs)
            //{
            //    Console.Write(blog.BlogId);
            //    Console.WriteLine(blog.Url);
            //}
        }
    }
}