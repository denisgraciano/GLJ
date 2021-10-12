using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.Cliente
{
    public class PDCliente : ProcessData
    {
        public BECliente Cliente { get; set; }
        public FECliente FECliente { get; set; }
        
        public BEDadosCliente DadosCliente { get; set; }
        public FEDadosCliente FEDadosCliene { get; set; }

        public BETelefoneCliente TelefoneCliente { get; set; }

        public PDCliente()
        {
            this.Cliente = new BECliente();
            this.FECliente = new FECliente();

            this.DadosCliente = new BEDadosCliente();
            this.FEDadosCliene = new FEDadosCliente();
            this.TelefoneCliente = new BETelefoneCliente();
        }
    }
}