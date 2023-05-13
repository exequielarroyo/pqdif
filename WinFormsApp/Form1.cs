using Data.Access;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseContext databaseContext = new DatabaseContext();

            //databaseContext.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            //databaseContext.SaveChanges();

            List<Blog> blogs = databaseContext.Blogs.ToList();
            foreach (Blog blog in blogs)
            {
                Console.Write(blog.BlogId);
                Console.WriteLine(blog.Url);
            }
        }
    }
}