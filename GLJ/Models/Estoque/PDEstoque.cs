using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.Estoque
{
    public class PDEstoque : ProcessData
    {
        public BEEstoque Estoque { get; set; }
        public List<BEEstoque> ListEstoque { get; set; }
        public FEEstoque FEEstoque { get; set; }

        public PDEstoque()
        {
            this.Estoque = new BEEstoque();
            this.ListEstoque = new List<BEEstoque>();
            this.FEEstoque = new FEEstoque();
        }
    }
}