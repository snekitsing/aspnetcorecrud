using ASPNET_CORE_CRUD_APP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ASPNET_CORE_CRUD_APP.Pages
{
    public class CreateModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";

        public Client client = new Client();
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            var builder = WebApplication.CreateBuilder();
            client.Name = Request.Form["name"];
            client.Email = Request.Form["email"];
            client.Phone = Request.Form["phone"];


            if (client.Name.Length == 0 || client.Email.Length == 0 || client.Phone.Length == 0) 
            {
                errorMessage = "All feilds are required";
                return;
            }

            //Save to db
            string query = "INSERT INTO App.Clients (Name,Email,Phone) VALUES (@name,@email,@phone)";
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", client.Name);
                        command.Parameters.AddWithValue("@email", client.Email);
                        command.Parameters.AddWithValue("@phone", client.Phone);

                        command.ExecuteNonQuery();
                    }
                }

            }catch(Exception ex) 
            {
                errorMessage = ex.ToString();
            }

            client.Name = "";
            client.Email = "";
            client.Phone = "";

            successMessage = "Client added successfully!";
            Response.Redirect("/Clients");
        }
    }
}
