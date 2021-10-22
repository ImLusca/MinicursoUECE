using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace loginInseguro.models
{
    public class ArmazenamentoNaoSeguro : IArmazenamento
    {
        public int Id { get; protected set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { private get; set; }

        public void CadastraUser() {
            string query = $"insert into ArmazenamentoNaoSeguro(login,nome,senha) values('{login}','{nome}','{senha}')";
            try
            {
                using (SqlConnection cnn = new SqlConnection(variaveisGlobais.connStr))
                {
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void RealizaLogin() {
            string query = $"select top 1 Id, nome from ArmazenamentoNaoSeguro where login = '{login}' and senha='{senha}' ";

            try
            {
                using (SqlConnection cnn = new SqlConnection(variaveisGlobais.connStr)) {
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cnn.Open();
                    SqlDataReader dreader = cmd.ExecuteReader();

                    if (dreader.Read()) {
                        Id   = (int)dreader["Id"];
                        nome = (string)dreader["nome"];
                    }
                    dreader.Close();
                }

            }
            catch (Exception ex)
            {
                throw;   
            }


        }

    }
}
