using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using Onion.Business;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFConfiguracaoLoja : Singleton<BPConfiguracaoLoja>
    {
        internal BEConfiguracaoLoja Salvar(BEConfiguracaoLoja beConfiguracaoLoja)
        {
            if (beConfiguracaoLoja.Codigo > 0)
            {
                BEConfiguracaoLoja beConfiguracaoLojaOld = Instance.ObterTodos(new FEConfiguracaoLoja() { CodigoLoja = beConfiguracaoLoja.CodigoLoja }).ResultList.FirstOrDefault();
                beConfiguracaoLoja = (BEConfiguracaoLoja)beConfiguracaoLojaOld.AlterProperties(beConfiguracaoLoja);
            }
            return Instance.Salvar(beConfiguracaoLoja);
        }
    }
}