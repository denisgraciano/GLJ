using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.Produto
{
    public class PDProduto : ProcessData
    {
        public BEProduto Produto { get; set; }
        public List<BEProduto> ListProduto { get; set; }
        public FEProduto FEProduto { get; set; }

        public PDProduto()
        {
            this.Produto = new BEProduto();
            this.FEProduto = new FEProduto();
            this.ListProduto = new List<BEProduto>();

        }

    }
}