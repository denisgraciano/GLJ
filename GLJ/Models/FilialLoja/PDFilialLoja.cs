using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.FilialLoja
{
    public class PDFilialLoja : ProcessData
    {
        public BEFilialLoja FilialLoja { get; set; }
        public List<BEFilialLoja> ListFilialLoja { get; set; }
        public FEFilialLoja FEFilialLoja { get; set; }
        public BELoja Loja { get; set; }

        public PDFilialLoja()
        {
            this.FilialLoja = new BEFilialLoja();
            this.ListFilialLoja = new List<BEFilialLoja>();
            this.FEFilialLoja = new FEFilialLoja();
            this.Loja = new BELoja();
        }

    }
}