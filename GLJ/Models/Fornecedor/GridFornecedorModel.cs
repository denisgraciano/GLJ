using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Fornecedor
{
    public class GridFornecedorModel
    {
        #region BuscaFornecedor
        public static GridColumnModelList<BEFornecedor> BEFornecedorColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BEFornecedor> CreatePersonColumns()
        {
            GridColumnModelList<BEFornecedor> cn = new GridColumnModelList<BEFornecedor>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();
            cn.Add(p => p.CNPJ);
            cn.Add(p => p.Nome);
            cn.Add(p => p.RazaoSocial);
            cn.Add(p => p.Cidade.UF.SiglaUF);
            cn.Add(p => p.Cidade.NomeCidade);
            cn.Add(p => p.Ativo).SetFormatter("'checkbox'"); ;
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion

        #region Grid Contatos

        public static GridColumnModelList<BEContatoFornecedor> BEContatoForncedorColumns
        {
            get
            {
                return CreateContatoColumns();
            }
        }
        private static GridColumnModelList<BEContatoFornecedor> CreateContatoColumns()
        {
            GridColumnModelList<BEContatoFornecedor> cn = new GridColumnModelList<BEContatoFornecedor>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Fornecedor.Nome);
            cn.Add(p => p.Nome);
            cn.Add(p => p.Email);
            cn.Add(p => p.Fone);
            cn.Add(p => p.CEL);
            cn.Add(p => p.FAX);
            cn.Add(p => p.TrackingState).SetHidden(true);


            return cn;
        }

        #endregion

    }
}