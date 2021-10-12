using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Produto
{
    public class GridProdutoModel
    {
        #region BuscaLoja
        public static GridColumnModelList<BEProduto> BEProdutoColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEProduto> CreatePersonColumns()
        {
            GridColumnModelList<BEProduto> cn = new GridColumnModelList<BEProduto>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.Fornecedor.Nome);
            cn.Add(p => p.Descricao);
            cn.Add(p => p.TipoProduto.Descricao);
            cn.Add(p => p.CodigoFornecedorProduto);
            cn.Add(p => p.Ativo).SetFormatter("'checkbox'"); ;
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}