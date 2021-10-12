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
    public class BPFEstoque : Singleton<BPEstoque>
    {
        internal BEEstoque Salvar(BEEstoque beEstoque)
        {
            if (beEstoque.Codigo > 0)
            {
                
                BEEstoque beEstoqueOld = Instance.ObterTodos(new FEEstoque() { Codigo = beEstoque.Codigo, CodigoLoja = beEstoque.CodigoLoja }).ResultList.FirstOrDefault();
                beEstoque = (BEEstoque)beEstoqueOld.AlterProperties(beEstoque);
            }
            return Instance.Salvar(beEstoque);
        }
    }
}