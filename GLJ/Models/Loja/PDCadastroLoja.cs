using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.Loja
{
    public class PDCadastroLoja : ProcessData
    {
        public BELoja Loja { get; set; }
        public List<BELoja> listLoja { get; set; }
        public FELoja FELoja { get; set; }
        public BEContatoLoja Contato { get; set; }


        public PDCadastroLoja()
        {
            this.FELoja = new FELoja();
            this.listLoja = new List<BELoja>();
            this.Loja = new BELoja();
            this.Contato = new BEContatoLoja();
        }
    }
}