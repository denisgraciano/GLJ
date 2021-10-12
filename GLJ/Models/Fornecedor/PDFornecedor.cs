using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.Fornecedor
{
    public class PDFornecedor : ProcessData
    {
        public BEFornecedor Fornecedor { get; set; }
        public List<BEFornecedor> ListFornecedor { get; set; }
        public FEFornecedor FEFornecedor { get; set; }
        public BEContatoFornecedor Contato { get; set; }

        public PDFornecedor()
        {
            this.Fornecedor = new BEFornecedor();
            this.ListFornecedor = new List<BEFornecedor>();
            this.FEFornecedor = new FEFornecedor();
            this.Contato = new BEContatoFornecedor();
        }

    }
}