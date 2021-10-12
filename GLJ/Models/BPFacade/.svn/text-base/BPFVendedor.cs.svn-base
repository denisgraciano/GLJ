using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Acesso.Business.Process;
using GLJ.Acesso.Filter.Entity;
using GLJ.Filter.Entity;
using GLJ.Acesso.Business.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFVendedor : Singleton<BPVendedor>
    {
        internal BEVendedor Salvar(BEVendedor beVendedor)
        {
            if (beVendedor.Codigo > 0)
            {
                BEVendedor beFilialLojaOld = Instance.ObterTodos(new FEVendedor() { Codigo = beVendedor.Codigo, CodigoLoja = beVendedor.CodigoLoja }).ResultList.FirstOrDefault();
                beVendedor = (BEVendedor)beFilialLojaOld.AlterProperties(beVendedor);
            }
            return Instance.Salvar(beVendedor);
        }
    }
}