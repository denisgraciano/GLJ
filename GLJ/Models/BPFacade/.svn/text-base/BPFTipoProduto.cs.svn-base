using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onion.Business;
using GLJ.Business.Process;
using GLJ.Filter.Entity;
using GLJ.Business.Entity;

namespace GLJ.Models.BPFacade
{
    public class BPFTipoProduto : Singleton<BPTipoProduto>
    {
        internal BETipoProduto Salvar(BETipoProduto beTipoProduto)
        {
            if (beTipoProduto.Codigo > 0)
            {

                BETipoProduto beFilialLojaOld = Instance.ObterTodos(new FETipoProduto() { Codigo = beTipoProduto.Codigo, CodigoLoja = beTipoProduto.CodigoLoja }).ResultList.FirstOrDefault();
                beTipoProduto = (BETipoProduto)beFilialLojaOld.AlterProperties(beTipoProduto);
            }
            return Instance.Salvar(beTipoProduto);
        }
    }
}