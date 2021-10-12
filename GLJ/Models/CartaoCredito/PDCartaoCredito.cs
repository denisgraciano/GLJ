using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.CartaoCredito
{
    public class PDCartaoCredito : ProcessData
    {
        public BECartaoCredito CartaoCredito { get; set; }
        public List<BECartaoCredito> ListCartaoCredito { get; set; }
        public FECartaoCredito FECartaoCredito { get; set; }

        public PDCartaoCredito()
        {
            this.CartaoCredito = new BECartaoCredito();
            this.ListCartaoCredito = new List<BECartaoCredito>();
            this.FECartaoCredito = new FECartaoCredito();
        }
    }
}