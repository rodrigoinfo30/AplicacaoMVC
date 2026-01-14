using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ComandosRelatorios
    {

        public string NOM_RAZAO_SACB { get; set; }
        public int COD_SACADO { get; set; }
        public string NUM_CGC_CPF_SACB { get; set; }

        public string CONTADOR { get; set; }

        private Database.ComandosSqlRel comandoDb = new Database.ComandosSqlRel();

    }
}
