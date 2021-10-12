using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using Onion.Business;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFPreco : Singleton<BPPreco>
    {
        internal BEPreco Salvar(BEPreco bePreco)
        {
            if (bePreco.Codigo > 0)
            {
                BEPreco beFilialLojaOld = Instance.ObterTodos(new FEPreco() { Codigo = bePreco.Codigo, CodigoLoja = bePreco.CodigoLoja }).ResultList.FirstOrDefault();
                bePreco = (BEPreco)beFilialLojaOld.AlterProperties(bePreco);
            }
            return Instance.Salvar(bePreco);
        }
    }
}