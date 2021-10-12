using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLJ.Business.Entity;
using MVC.Controls.Grid;

namespace GLJ.Models.Loja
{
    public static class GridLojaModel
    {
        #region BuscaLoja
        public static GridColumnModelList<BELoja> BELojaColumns
        {
            get
            {
                return CreatePersonColumns();
            }
        }
        private static GridColumnModelList<BELoja> CreatePersonColumns()
        {
            GridColumnModelList<BELoja> cn = new GridColumnModelList<BELoja>();
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

        public static GridColumnModelList<BEContatoLoja> BEContatoLojaColumns
        {
            get
            {
                return CreateContatoColumns();
            }
        }
        private static GridColumnModelList<BEContatoLoja> CreateContatoColumns()
        {
            GridColumnModelList<BEContatoLoja> cn = new GridColumnModelList<BEContatoLoja>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Loja.Nome);
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