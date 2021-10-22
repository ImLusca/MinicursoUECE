using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace loginInseguro.models
{
    public class ArmazenamentoSeguro : IArmazenamento
    {
        string _hashSenha, _salt;
        public int Id { get; protected set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha {  get; set; }
        
        public string hashSenha { 
            get { 
                return string.IsNullOrEmpty(_hashSenha) ? GeraHash(salt + senha) : _hashSenha; 
            } 
            set {
                _hashSenha = value;
            }
        }
        public string salt { 
            get {
                if (string.IsNullOrEmpty(_salt)){
                    GeraSalt();
                }
                return _salt;
            }
            set {
                _salt = value; 
            }         
        }

        public string GeraHash(string valor) {
            using (SHA512 sha512Hash = SHA512.Create())
            {                
                byte[] bytesValor = Encoding.UTF8.GetBytes(valor);
                byte[] bytesHash  = sha512Hash.ComputeHash(bytesValor);
                return BitConverter.ToString(bytesHash).Replace("-", String.Empty);                
            }
        }
        public void GeraSalt() {
            const string src = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/*-+.!@#$%¨&*()[]";
            int tamanhoSalt = 256;
            var sb = new StringBuilder();
            //explicar diferença entre pseudo randomico e realmente randomico
            Random pRNG = new Random();
            for (var i = 0; i < tamanhoSalt; i++)
            {
                var c = src[pRNG.Next(0, src.Length)];
                sb.Append(c);
            }
            salt = sb.ToString();
        }

        public void RealizaLogin()
        {

            using (SqlConnection cnn = new SqlConnection(variaveisGlobais.connStr))
            {
                cnn.Open();

                // Pegando salt do login
                string query1 = $"select top 1 salt from ArmazenamentoSeguro where login = '{login}'";
                SqlCommand cmd = new SqlCommand(query1, cnn);

                SqlDataReader dreader = cmd.ExecuteReader();

                if (dreader.Read()){
                    salt = (string)dreader["salt"];
                }
                else {
                    throw new Exception("Login incorreto");
                }
                dreader.Close();

                //Aplicando salt + senha e obtendo o hash
                string query2 = $"select top 1 id, nome from ArmazenamentoSeguro where login = '{login}' and hashSenha='{hashSenha}' ";
                cmd.CommandText = query2;
                dreader = cmd.ExecuteReader();
                if (dreader.Read()){
                    Id   = (int)dreader["Id"];
                    nome = (string)dreader["nome"];
                }
                    
            }
        }

        public void CadastraUser()
        {
            string query = $"insert into ArmazenamentoSeguro(login,nome,hashSenha,salt) values('{login}','{nome}','{hashSenha}','{salt}')";

            using (SqlConnection cnn = new SqlConnection(variaveisGlobais.connStr))
            {
                SqlCommand cmd = new SqlCommand(query, cnn);
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

        }

    }
}
