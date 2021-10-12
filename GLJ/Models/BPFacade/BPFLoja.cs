using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFLoja : Onion.Business.Singleton<BPLoja>
    {
        internal BELoja Salvar(BELoja beLoja)
        {
            if (beLoja.Codigo > 0)
            {
                BELoja beLojaOld = Instance.ObterTodos(new FELoja() { Codigo = beLoja.Codigo }).ResultList.FirstOrDefault();
                beLoja = (BELoja)beLojaOld.AlterProperties(beLoja);
            }
            return Instance.Salvar(beLoja);
        }
    }
}