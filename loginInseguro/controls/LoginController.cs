using loginInseguro.models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace loginInseguro.controls
{
    public class LoginController : Controller
    {

        [HttpPost]
        public ActionResult Create([FromBody]ArmazenamentoSeguro user)
        {
            try
            {
                user.CadastraUser();
                return new StatusCodeResult(201);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);                
            }

        }
         

        [HttpPost]
        // POST: LoginController/Edit/5
        public ActionResult Login([FromBody]ArmazenamentoSeguro user)
        {
            try
            {
                user.RealizaLogin();
                return Ok(Json(user.nome).Value);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public ActionResult PegarSalt(string login) {

            try
            {
                string salt;
                using (SqlConnection cnn = new SqlConnection(variaveisGlobais.connStr))
                {
                    cnn.Open();

                    // Pegando salt do login
                    string query1 = $"select top 1 salt from ArmazenamentoSeguro where login = '{login}'";
                    SqlCommand cmd = new SqlCommand(query1, cnn);

                    SqlDataReader dreader = cmd.ExecuteReader();

                    if (dreader.Read())
                    {
                        salt = (string)dreader["salt"];
                    }
                    else
                    {
                        return StatusCode(401);
                    }
                    dreader.Close();


                }

                return Ok(salt);
            }
            catch (Exception)
            {

                return new StatusCodeResult(500);
            }

        }


    }
}
