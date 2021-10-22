using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace aplicacaoUECE.models
{
    public class ArmazenamentoNaoSeguro : IArmazenamento
    {
        public int Id { get; protected set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }

        public async void CadastraUser() {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endereco = variaveisGlobais.SiteCriptografado + "/Login/Create";
                    Uri enderecoServidor = new Uri(string.Format(endereco, string.Empty));

                    string json = JsonConvert.SerializeObject(this);
                    StringContent conteudo = new StringContent(json, Encoding.UTF8, "application/json");
                                        
                    HttpResponseMessage resposta = await client.PostAsync(enderecoServidor, conteudo);

                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {                        
                        //Encaminhar para página de acesso
                    }
                    else { 
                        //Erro interno
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void RealizaLogin() {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string endereco = variaveisGlobais.SiteCriptografado + "/Login/Login";
                    Uri enderecoServidor = new Uri(string.Format(endereco, string.Empty));


                    string json = JsonConvert.SerializeObject(this);
                    StringContent conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage resposta = await client.PostAsync(enderecoServidor, conteudo);


                    if (resposta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var resultado = await resposta.Content.ReadAsStringAsync();
                        nome = resultado;
                        //Encaminhar para página de acesso
                    }
                    else { 
                        //Login não encontrado
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
