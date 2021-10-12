using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Acesso.Business.Entity;
using GLJ.Acesso.Filter.Entity;

namespace GLJ.Models.Vendedor
{
    public class PDVendedor : ProcessData   
    {
        public BEVendedor Vendedor { get; set; }
        public List<BEVendedor> ListVendedor { get; set; }
        public FEVendedor FEVendedor { get; set; }

        public PDVendedor()
        {
            this.Vendedor = new BEVendedor();
            this.ListVendedor = new List<BEVendedor>();
            this.FEVendedor = new FEVendedor();
        }
    }
}