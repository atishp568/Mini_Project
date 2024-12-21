using MiniProject.Config;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MiniProject.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult GetProducts(int page= 1, int pagesize= 10)
        {
            List<Product> products = new List<Product>();
            ViewBag.Currentpage= page;
            ViewBag.Pagesize= pagesize;
            using(SqlConnection conn= new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd = new SqlCommand("SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName " +
                    "FROM Products p " +
                    "JOIN Categories c ON p.CategoryId = c.CategoryId " +
                    "ORDER BY p.ProductId " +
                    "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Offset", (page-1) * pagesize);
                    cmd.Parameters.AddWithValue("@PageSize", pagesize);
                    SqlDataReader sdr= cmd.ExecuteReader();
                    while(sdr.Read())
                    {
                        products.Add(new Product()
                        {
                            ProductId = (int)sdr["ProductId"],
                            ProductName = (string)sdr["ProductName"],
                            CategoryId = (int)sdr["CategoryId"],
                            CategoryName = (string)sdr["CategoryName"]
                        });
                    }
                }
            }
            return View(products.ToList());
        }

        public static List<Product> PopulateCategory()
        {
            List <Product> category = new List<Product>();
            using(SqlConnection conn = new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("select * from Categories",conn))
                {
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while(sdr.Read())
                    {
                        category.Add(new Product()
                        {
                            CategoryId = (int)sdr["CategoryId"],
                            CategoryName = (string)sdr["CategoryName"]
                        });
                    }
                }
            }
            return category;
        }

        public ActionResult AddProduct()
        {
            ViewBag.Category= PopulateCategory();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            using(SqlConnection conn = new SqlConnection(StoreConnection.GetConnection()))
            {
                using( SqlCommand cmd= new SqlCommand("insert into Products(ProductName,CategoryId) values(@ProductName,@CategoryId)",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetProducts");
        }

        public ActionResult EditProduct(int id)
        {
            ViewBag.Category= PopulateCategory();
            Product product = new Product();
            using(SqlConnection conn = new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("select * from Products where ProductId= @Id",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read()) 
                    {
                        product.ProductId = (int)sdr["ProductId"];
                        product.ProductName = (string)sdr["ProductName"];
                        product.CategoryId = (int)sdr["CategoryId"];
                    }
                }
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(int id, Product product) 
        {
            using(SqlConnection conn= new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("update Products set ProductName= @ProductName, CategoryId= @CategoryId where ProductId= @Id",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetProducts");
        }

        public ActionResult DeleteProduct(int id) 
        {
            using(SqlConnection conn= new SqlConnection(StoreConnection.GetConnection()))
            {
                using(SqlCommand cmd= new SqlCommand("delete from Products where ProductId= @Id",conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetProducts");
        }


    }
}