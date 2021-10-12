using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Process;
using GLJ.Business.Entity;
using GLJ.Filter.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFFilialLoja : Onion.Business.Singleton<BPFilialLoja>
    {
        internal BEFilialLoja Salvar(BEFilialLoja beFilialLoja)
        {
            if (beFilialLoja.Codigo > 0)
            {

                BEFilialLoja beFilialLojaOld = Instance.ObterTodos(new FEFilialLoja() { Codigo = beFilialLoja.Codigo, CodigoLoja = beFilialLoja.CodigoLoja }).ResultList.FirstOrDefault();
                beFilialLoja = (BEFilialLoja)beFilialLojaOld.AlterProperties(beFilialLoja);
            }
            return Instance.Salvar(beFilialLoja);
        }
    }
}