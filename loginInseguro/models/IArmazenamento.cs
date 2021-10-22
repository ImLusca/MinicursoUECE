using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loginInseguro.models
{
    public interface IArmazenamento
    {
        public int Id { get;  }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { set; }

        public void CadastraUser();
        public void RealizaLogin();

    }
}
