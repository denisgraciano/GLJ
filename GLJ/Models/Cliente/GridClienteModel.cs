using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Controls.Grid;
using GLJ.Business.Entity;

namespace GLJ.Models.Cliente
{
    public class GridClienteModel
    {
        #region BuscaEndereco
        public static GridColumnModelList<BEDadosCliente> BEDadosClienteColumns
        {
            get
            {
                return CreateEnderecoColumns();
            }
        }
        private static GridColumnModelList<BEDadosCliente> CreateEnderecoColumns()
        {
            GridColumnModelList<BEDadosCliente> cn = new GridColumnModelList<BEDadosCliente>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.CodigoCliente).SetHidden(true);
            cn.Add(p => p.CodigoTipoDadoCliente).SetFormatter("enumToString").SetColumnRenderer(new ComboColumnRenderer(string.Format(@"" + Utils.Root + "Cliente/CarregarTipoEndereco")));
            cn.Add(p => p.Endereco);
            cn.Add(p => p.Numero);
            cn.Add(p => p.Complemento); 
            cn.Add(p => p.Bairro);
            cn.Add(p => p.CEP);
            cn.Add(p => p.CodigoCidade).SetHidden(true);
            cn.Add(p => p.NomeCidade);
            cn.Add(p => p.UF);
            cn.Add(p => p.Referencia);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion

        #region BuscaTelefone
        public static GridColumnModelList<BETelefoneCliente> BETelefoneCienteColumns
        {
            get
            {
                return CreateTelefoneColumns();
            }
        }
        private static GridColumnModelList<BETelefoneCliente> CreateTelefoneColumns()
        {
            GridColumnModelList<BETelefoneCliente> cn = new GridColumnModelList<BETelefoneCliente>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.CodigoCliente).SetHidden(true);
            cn.Add(p => p.TipoTelefone).SetFormatter("enumTipoTelefoneToString").SetColumnRenderer(new ComboColumnRenderer(string.Format(@"" + Utils.Root + "Cliente/CarregarTipoTelefone")));
            cn.Add(p => p.DDI);
            cn.Add(p => p.DDD);
            cn.Add(p => p.Telefone);
            cn.Add(p => p.NomeRecado);
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion

        #region BuscaCliente
        public static GridColumnModelList<BECliente> BEClienteColumns
        {
            get
            {
                return CreateClienteColumns();
            }
        }

        public static GridColumnModelList<BECliente> BEClientePopUpColumns
        {
            get
            {
                return CreateClientePopUpColumns();
            }
        }

        private static GridColumnModelList<BECliente> CreateClienteColumns()
        {
            GridColumnModelList<BECliente> cn = new GridColumnModelList<BECliente>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey();//.SetHidden(true);
            cn.Add(p => p.Nome);
            cn.Add(p => p.CPF);
            cn.Add(p => p.Email);
            cn.Add(p => p.DataNascimento).SetFormatter("dateFormatGrid,formatoptions:{srcformat:'d/m/Y'}");           
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }

        private static GridColumnModelList<BECliente> CreateClientePopUpColumns()
        {
            GridColumnModelList<BECliente> cn = new GridColumnModelList<BECliente>();
            cn.Add(p => p.Codigo).SetAsPrimaryKey().SetHidden(true);
            cn.Add(p => p.Nome).SetWidth("200");
            cn.Add(p => p.CPF).SetWidth("200");
            cn.Add(p => p.Email).SetWidth("200");
            cn.Add(p => p.DataNascimento).SetFormatter("dateFormatGrid,formatoptions:{srcformat:'d/m/Y'}").SetWidth("100"); ;
            cn.Add(p => p.TrackingState).SetHidden(true);
            return cn;
        }
        #endregion
    }
}