<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<script runat="server">
  private void Page_Load(object sender, System.EventArgs e)
  {
      RelatorioAssistenciaPorProduto.ProcessingMode = ProcessingMode.Local;

      RelatorioAssistenciaPorProduto.LocalReport.ReportPath = @"Reports\EspelhoPedido.rdlc";
      List<Object> lst = new List<Object>();
      List<Object> lstLoja = new List<Object>();
      List<Object> lstDetPed = new List<Object>();
      List<Object> lstPedPag = new List<Object>();     
      
      if (Session["FEEspelhoPedido"] != null)
      {
          Relatorio.Business.Process.BPEspelhoPedido RelAssistenciaPorProduto = new Relatorio.Business.Process.BPEspelhoPedido();
          lst = RelAssistenciaPorProduto.GetEspelhoPedidoCliente(Session["FEEspelhoPedido"] as Relatorio.Filter.Entity.FEEspelhoPedido);
      }

      if (Session["FEDadosLoja"] != null)
      {
          Relatorio.Business.Process.BPDadosLoja RelDadosLoja = new Relatorio.Business.Process.BPDadosLoja();
          lstLoja = RelDadosLoja.GetDadosLoja(Session["FEDadosLoja"] as Relatorio.Filter.Entity.FEDadosLoja);          
      }
      
      if (Session["FEEspelhoPedidoDetalhe"]!= null)
      {
          Relatorio.Business.Process.BPEspelhoPedidoDetalhe RelDetalhePedido = new Relatorio.Business.Process.BPEspelhoPedidoDetalhe();
          lstDetPed = RelDetalhePedido.GetDetalhePedido(Session["FEEspelhoPedidoDetalhe"] as Relatorio.Filter.Entity.FEEspelhoPedidoDetalhe);          
      }

      if (Session["FEEspelhoPedidoPagamento"] != null)
      {
          Relatorio.Business.Process.BPEspelhoPedidoPagamento RelPedidoPagamento = new Relatorio.Business.Process.BPEspelhoPedidoPagamento();
          lstPedPag = RelPedidoPagamento.GetEspelhoPedidoPagamento(Session["FEEspelhoPedidoPagamento"] as Relatorio.Filter.Entity.FEEspelhoPedidoPagamento);
      }
      
      
      ReportDataSource dataSource = new ReportDataSource("DataSet1", lst);
      ReportDataSource dscLoja = new ReportDataSource("DataSet2", lstLoja);
      ReportDataSource dscDetPed = new ReportDataSource("DataSet3", lstDetPed);
      ReportDataSource dscPedPag = new ReportDataSource("DataSet4", lstPedPag);
      
      RelatorioAssistenciaPorProduto.LocalReport.DataSources.Add(dataSource);
      RelatorioAssistenciaPorProduto.LocalReport.DataSources.Add(dscLoja);
      RelatorioAssistenciaPorProduto.LocalReport.DataSources.Add(dscDetPed);
      RelatorioAssistenciaPorProduto.LocalReport.DataSources.Add(dscPedPag);

      Session["FEDadosLoja"] = null;
      Session["FEEspelhoPedido"] = null;
      Session["FEEspelhoPedidoPagamento"] = null;
  }

</script>
<form id="Form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<rsweb:ReportViewer ID="RelatorioAssistenciaPorProduto" runat="server" 
    AsyncRendering="false" Width="100%"></rsweb:ReportViewer>
</form>