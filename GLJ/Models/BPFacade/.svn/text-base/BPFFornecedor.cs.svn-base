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
    public class BPFFornecedor : Singleton<BPFornecedor>
    {
        internal BEFornecedor Salvar(BEFornecedor beFornecedor)
        {
            if (beFornecedor.Codigo > 0)
            {

                BEFornecedor beFornecedorOld = Instance.ObterTodos(new FEFornecedor() { Codigo = beFornecedor.Codigo, CodigoLoja =beFornecedor.CodigoLoja }).ResultList.FirstOrDefault();
                beFornecedor = (BEFornecedor)beFornecedorOld.AlterProperties(beFornecedor);
            }
            return Instance.Salvar(beFornecedor);
        }
    }
}