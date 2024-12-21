using MiniProject.Config;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniProject.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult GetCategories()
        {
            List<Category> categories = new List<Category>();
            using(SqlConnection conn = new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd = new SqlCommand("select * from Categories",conn))
                {
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while(sdr.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = (int)sdr["CategoryId"],
                            CategoryName = (string)sdr["CategoryName"]
                        });
                    }

                }
            }
            return View(categories);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            using(SqlConnection conn= new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("insert into Categories(CategoryName) values(@CategoryName)",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetCategories");
        }

        public ActionResult EditCategory(int id)
        {
            Category category = new Category();
            using(SqlConnection conn = new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("select * from Categories where CategoryId= @Id",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while(sdr.Read()) 
                    {
                        category.CategoryId = (int)sdr["CategoryId"];
                        category.CategoryName = (string)sdr["CategoryName"];
                    }
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(int id, Category category)
        {
            using(SqlConnection conn= new SqlConnection(StoreConnection.GetConnection()))
            {
                using( SqlCommand cmd= new SqlCommand("update Categories set CategoryName= @CategoryName where CategoryId= @Id",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetCategories");
        }

        public ActionResult DeleteCategory(int id) 
        {
            using(SqlConnection conn= new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("delete from Categories where CategoryId= @Id", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetCategories");
        }


    }
}