using aplicacaoUECE.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aplicacaoUECE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            IArmazenamento usuario = new ArmazenamentoSeguro();
            //ArmazenamentoSeguro usuario = new ArmazenamentoSeguro();

            usuario.login = txt_login.Text;
            usuario.senha = txt_password.Password;
            //usuario.loginHash = true;
            //await usuario.PegarSalt();

            usuario.RealizaLogin();
        }
        private void Cadastro(object sender, RoutedEventArgs e)
        {
            IArmazenamento usuario = new ArmazenamentoSeguro();


            usuario.nome  = txt_nome.Text;
            usuario.login = txt_login.Text;
            usuario.senha = txt_password.Password;

            usuario.CadastraUser();
        }
    }
}
