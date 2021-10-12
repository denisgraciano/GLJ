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
    public class BPFCartaoCredito : Singleton<BPCartaoCredito>
    {
        internal BECartaoCredito Salvar(BECartaoCredito beCartaoCredito)
        {
            if (beCartaoCredito.Codigo > 0)
            {

                BECartaoCredito beCartaoCreditoOld = Instance.ObterTodos(new FECartaoCredito() { Codigo = beCartaoCredito.Codigo, CodigoLoja = beCartaoCredito.CodigoLoja }).ResultList.FirstOrDefault();
                beCartaoCredito = (BECartaoCredito)beCartaoCreditoOld.AlterProperties(beCartaoCredito);
            }
            return Instance.Salvar(beCartaoCredito);
        }
    }
}