using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aplicacaoUECE.models
{
    public class ArmazenamentoSeguro : IArmazenamento
    {
        string _hashSenha, _salt;

        public int Id { get; protected set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        [JsonIgnore]
        public bool loginHash { get; set; }
        
        public string hashSenha { 
            get { 
                return string.IsNullOrEmpty(_hashSenha) && loginHash ? GeraHash(salt + senha) : _hashSenha; 
            } 
            set {
                _hashSenha = value;
            }
        }
        public string salt{
            get{
                return _salt;
            }
            set{
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
        public async Task PegarSalt() {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endereco = variaveisGlobais.SiteCriptografado + "/Login/PegarSalt";
                    Uri enderecoServidor = new Uri(string.Format(endereco, string.Empty));

                    HttpResponseMessage resposta = await client.GetAsync(enderecoServidor + "?login=" + login);

                    if(resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var resultado = await resposta.Content.ReadAsStringAsync();
                        salt = resultado;
                    }
                    else if(resposta.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new Exception("Login inexistente");
                    }
                    else
                    {
                        throw new Exception("Erro interno no servidor");
                    }

                }

            }
            catch (Exception )
            {
                throw;
            }
        }

        public async void RealizaLogin()
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endereco = variaveisGlobais.SiteNaoCriptografado + "/Login/Login";
                    Uri enderecoServidor = new Uri(string.Format(endereco, string.Empty));                    

                    string json = JsonConvert.SerializeObject(this);
                    StringContent conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage resposta = await client.PostAsync(enderecoServidor, conteudo);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var resultado = await resposta.Content.ReadAsStringAsync();
                        nome = resultado;
                        //Encaminhar para tela de login
                    }
                    else { 
                        //Login incorreto
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public async void CadastraUser()
        {
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    string endereco = variaveisGlobais.SiteNaoCriptografado + "/Login/Create";
                    Uri enderecoServidor = new Uri(string.Format(endereco, string.Empty));

                    string json = JsonConvert.SerializeObject(this);
                    StringContent conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage resposta = await client.PostAsync(enderecoServidor, conteudo);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {                        
                        //Encaminhar para página de acesso
                    }
                    else
                    {
                        //Erro interno
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}
