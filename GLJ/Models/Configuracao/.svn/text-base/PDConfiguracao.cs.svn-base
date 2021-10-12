using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.Configuracao
{
    public class PDConfiguracao : ProcessData   
    {
        public BEConfiguracaoLoja Configuracao { get; set; }
        public List<BEConfiguracaoLoja> ListConfiguracao { get; set; }
        public FEConfiguracaoLoja FEConfiguracaoLoja { get; set; }

        public PDConfiguracao()
        {
            this.Configuracao = new BEConfiguracaoLoja();
            this.ListConfiguracao = new List<BEConfiguracaoLoja>();
            this.FEConfiguracaoLoja = new FEConfiguracaoLoja();
        }
    }
}