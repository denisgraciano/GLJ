using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models.Acesso;
using GLJ.Models.BPFacade;
using GLJ.Filter.Entity;
using GLJ.Acesso.Filter.Entity;
using MVC.Controls;
using GLJ.Acesso.Business.Entity;
using MVC.Controls.Grid;
using Onion.Business.Entity;
using GLJ.Acesso.Business.Validation;
using Onion.Business.Validation;

namespace GLJ.Controllers
{
    public class AcessoController : BaseControllerGeneric<PDAcesso>
    {
        #region Métodos Chamada

        [AuthorizeOnion]
        public ActionResult Index()
        {
            this.LimparMensagens();
            return View();
        }

        #region Grupo Permissão
        [AuthorizeOnion]
        public ActionResult CadastroGrupoPermissao(int? id)
        {

            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.GrupoPermissao = BPFGrupoPermissao.Instance.ObterTodos(new FEGrupoPermissao() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();
            }

            return View(this.ProcessData);
        }

        public ActionResult CadastroGrupoView(int idGrupoPermissao)
        {
            this.LimparMensagens();

            this.ProcessData.GrupoView.GrupoPermisao = BPFGrupoPermissao.Instance.ObterTodos(new FEGrupoPermissao() { Codigo = idGrupoPermissao, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

            var viewsGrupo = from f in BPFViewControle.Instance.ObterTodos(new FEViewControle()).ResultList
                             select new { CodigoView = f.Codigo, f.Descricao };

            TempData["Views"] = new SelectList(viewsGrupo, "CodigoView", "Descricao");

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultaGrupoPermissao()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }
        #endregion

        #region Usuário
        [AuthorizeOnion]
        public ActionResult CadastroUsuario(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {

                this.ProcessData.Usuario = BPFUsuario.Instance.ObterTodos(new FEUsuario() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

                this.ProcessData.UsuariosGrupos = BPFUsuarioGrupoPermissao.Instance.ObterTodos(new FEUsuarioGrupoPermissao() { CodigoUsuario = this.ProcessData.Usuario.Codigo }).ResultList;
            }

            this.CarregaCombos();

            Session["Usuario"] = this.ProcessData.Usuario;
            Session["UsuariosGrupos"] = this.ProcessData.UsuariosGrupos;

            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult ConsultarUsuario()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }
        #endregion

        #region Usuario X Filial Loja

        [AuthorizeOnion]
        public ActionResult ConsultarUsuarioFilialLoja()
        {
            this.LimparMensagens();
            return View(this.ProcessData);
        }

        [AuthorizeOnion]
        public ActionResult AssociaUsuarioFilialLoja(int? id)
        {
            if (id.HasValue && id.GetValueOrDefault() > 0)
            {
                this.ProcessData.Usuario = BPFUsuario.Instance.ObterTodos(new FEUsuario() { Codigo = id.GetValueOrDefault(), CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();

                this.ProcessData.ListUsuarioFilialLoja = BPFUsuarioFilialLoja.Instance.ObterTodos(new FEUsuarioFilialLoja() { CodigoUsuario = this.ProcessData.Usuario.Codigo }).ResultList;
                
            }

            Session["Usu"] = this.ProcessData.Usuario;
            Session["UsuariosFiliais"] = this.ProcessData.ListUsuarioFilialLoja;
            this.CarregaCombos();
            return View(this.ProcessData);
        }
        
        #endregion
        #endregion

        #region Métodos Especificos

        #region GrupoPermissao

        public ActionResult SalvarGrupoPermissao(PDAcesso model)
        {
            try
            {
                model.GrupoPermissao.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.GrupoPermissao = new BPFGrupoPermissao().Salvar(model.GrupoPermissao);
                base.AdicionarMensagemSucesso("Grupo Permissão Salvo Com Sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("CadastroGrupoPermissao", new { id = this.ProcessData.GrupoPermissao.Codigo });
        }

        public JsonResult BuscarGrupoPermissao(FEGrupoPermissao feGrupoPermissao)
        {
            feGrupoPermissao.CodigoLoja = UsuarioLogin.CodigoLoja;
            Session["FEGrupoPermissao"] = feGrupoPermissao;
            return Json(null);
        }


        public JsonResult Buscar(SearchModel searchModel)
        {
            if (Session["FEGrupoPermissao"] == null) Session["FEGrupoPermissao"] = new FEGrupoPermissao() { CodigoLoja = UsuarioLogin.CodigoLoja };

            this.ProcessData.listGrupoPermissao = BPFGrupoPermissao.Instance.ObterTodos(Session["FEGrupoPermissao"] as FEGrupoPermissao).ResultList;
            Session["listItem"] = new List<BEGrupoPermissao>();


            ((List<BEGrupoPermissao>)Session["listItem"]).AddRange(this.ProcessData.listGrupoPermissao);

            IQueryable<BEGrupoPermissao> model = ((List<BEGrupoPermissao>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridGrupoPermissaoModel.BEGrupoPermissaoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region View Controle

        public ActionResult SalvarGrupoView(PDAcesso model)
        {
            try
            {
                this.ProcessData.GrupoView = new BPFGrupoPermissaoViewControle().SalvarGrupoPermissaoViewControle(model.GrupoView);
                base.AdicionarMensagemSucesso("Grupo Associado a Tela com sucesso.");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, "Erro ao associar o Grupo a Tela");
            }


            return RedirectToAction("CadastroGrupoView", new { idGrupoPermissao = this.ProcessData.GrupoView.GrupoPermisao.Codigo });
        }

        [HttpGet]
        public JsonResult ObterGrupoView(int? codigoView, SearchModel searchModel)
        {
            IQueryable<BEGrupoPermissaoViewControle> model = BPFGrupoPermissaoViewControle.Instance.ObterTodos(new FEGrupoPermissaoViewControle() { CodigoGrupoPermissao = codigoView, LoadType = LoadType.Medium }).ResultList.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridGrupoPermissaoModel.BEGrupoViewColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletarGrupoView(BEGrupoPermissaoViewControle beGrupoView)
        {
            List<object> gridResponse = new List<object>();
            try
            {
                BEGrupoPermissaoViewControle beOld = BPFGrupoPermissaoViewControle.Instance.ObterTodos(new FEGrupoPermissaoViewControle() { Codigo = beGrupoView.Codigo }).ResultList.FirstOrDefault();
                if (beOld != null)
                {
                    BPFGrupoPermissaoViewControle.Instance.Excluir(beOld);
                    gridResponse = GridResponse.CreateSuccess();
                }
                else
                {
                    gridResponse = GridResponse.CreateFailure("Associação não foi encontrada para ser Removida.");
                }
            }
            catch (Exception)
            {
                gridResponse = GridResponse.CreateFailure("Associação não pode ser Removido !");
            }

            return Json(gridResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Usuários
        [HttpGet]
        public JsonResult ObterUsuarioGrupo(int? codigoUsuario, SearchModel searchModel)
        {
            IQueryable<BEUsuarioGrupoPermissao> model;
            if (codigoUsuario.HasValue)
            {
                model = ((List<BEUsuarioGrupoPermissao>)Session["UsuariosGrupos"]).Where(g => g.CodigoUsuario == codigoUsuario.GetValueOrDefault()).AsQueryable();
            }
            else
            {
                model = ((List<BEUsuarioGrupoPermissao>)Session["UsuariosGrupos"]).AsQueryable();
            }


            GridData gridData = model.ToGridData(searchModel, GLJ.Models.Acesso.GridUsuarioModel.BEUsuarioGrupoColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletarUsuarioGrupoPermissao(BEUsuarioGrupoPermissao beUsuarioGrupoPermissao)
        {
            List<object> gridResponse = new List<object>();
            try
            {
                BEUsuarioGrupoPermissao beOld = BPFUsuarioGrupoPermissao.Instance.ObterTodos(new FEUsuarioGrupoPermissao() { Codigo = beUsuarioGrupoPermissao.Codigo }).ResultList.FirstOrDefault();
                if (beOld != null)
                {
                    BPFUsuarioGrupoPermissao.Instance.Excluir(beOld);
                    gridResponse = GridResponse.CreateSuccess();
                }
                else
                {
                    gridResponse = GridResponse.CreateFailure("Grupo não encontrado para ser excluído.");
                }
            }
            catch (Exception)
            {
                gridResponse = GridResponse.CreateFailure("Grupo não pode ser removido.");
            }

            return Json(gridResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarGrupo(PDAcesso model)
        {
            try
            {

                BVUsuario bv = new BVUsuario();
                bv.Salvar(model.Usuario);

                this.ProcessData.Usuario = model.Usuario;
                Session["Usuario"] = this.ProcessData.Usuario;

                model.UsuarioGrupo.CodigoUsuario = this.ProcessData.Usuario.Codigo;
                model.UsuarioGrupo.Usuario = this.ProcessData.Usuario;


                model.UsuarioGrupo.GrupoPermisao =
                    BPFGrupoPermissao.Instance.ObterTodos(new FEGrupoPermissao() { Codigo = model.UsuarioGrupo.CodigoGrupoPermissao, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();


                ((List<BEUsuarioGrupoPermissao>)Session["UsuariosGrupos"]).Add(model.UsuarioGrupo);

                return Json(new JsonReturn(true));
            }
            catch (ValidationException ve)
            {
                return Json(new JsonReturn(ve.ValidationResults));
            }


        }

        public ActionResult SalvarUsuario(PDAcesso model)
        {
            LimparMensagens();
            try
            {
                if (UsuarioLogin.CodigoLoja == null)
                    model.Usuario.CodigoLoja = BPFLoja.Instance.ObterTodos(new FELoja()).ResultList.FirstOrDefault().Codigo;

                model.Usuario.CodigoLoja = UsuarioLogin.CodigoLoja;

                this.ProcessData.Usuario = new BPFUsuario().Salvar(model.Usuario);

                model.UsuariosGrupos = ((List<BEUsuarioGrupoPermissao>)Session["UsuariosGrupos"]);

                foreach (BEUsuarioGrupoPermissao item in model.UsuariosGrupos)
                {
                    item.CodigoUsuario = this.ProcessData.Usuario.Codigo;
                    this.ProcessData.UsuarioGrupo = BPFUsuarioGrupoPermissao.Instance.Salvar(item);
                    this.ProcessData.UsuariosGrupos.Add(this.ProcessData.UsuarioGrupo);
                }


                base.AdicionarMensagemSucesso("Usuário Salvo com Sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("CadastroUsuario", new { id = this.ProcessData.Usuario.Codigo });
        }


        public JsonResult BuscarUsuario(FEUsuario feUsuario)
        {

            feUsuario.CodigoLoja = UsuarioLogin.CodigoLoja;

            Session["FEUsuario"] = feUsuario;
            return Json(null);
        }

        public JsonResult BuscarUsuarioGrid(SearchModel searchModel)
        {
  
            if (Session["FEUsuario"] == null)
            {
                Session["FEUsuario"] = new FEUsuario() { CodigoLoja = UsuarioLogin.CodigoLoja };
            }

            this.ProcessData.listUsuario = BPFUsuario.Instance.ObterTodos(Session["FEUsuario"] as FEUsuario).ResultList;
            Session["listItem"] = new List<BEUsuario>();


            ((List<BEUsuario>)Session["listItem"]).AddRange(this.ProcessData.listUsuario);

            IQueryable<BEUsuario> model = ((List<BEUsuario>)Session["listItem"]).AsQueryable();

            GridData gridData = model.ToGridData(searchModel, GridUsuarioModel.BEUsuarioColumns);

            return Json(gridData, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Usuario X Filial Loja 

        [HttpGet]
        public JsonResult ObterUsuarioFilialLoja(Int32 Id, SearchModel searchModel)
        {
            IQueryable<BEUsuarioFilialLoja> model = BPFUsuarioFilialLoja.Instance.ObterTodos(new FEUsuarioFilialLoja() { CodigoUsuario = Id }).ResultList.AsQueryable();
            GridData gridData = model.ToGridData(searchModel, GridUsuarioFilialLojaModel.BEUsuarioFilialLojaColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarFilial(PDAcesso model)
        {
            try
            {
                this.ProcessData.Usuario = model.Usuario;
                Session["Usu"] = this.ProcessData.Usuario;

                model.UsuarioFilialLoja.CodigoUsuario = this.ProcessData.Usuario.Codigo;
                model.UsuarioFilialLoja.Usuario = this.ProcessData.Usuario;

                model.UsuarioFilialLoja.FilialLoja =
                    BPFFilialLoja.Instance.ObterTodos(new FEFilialLoja() { Codigo = model.UsuarioFilialLoja.CodigoFilialLoja, CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList.FirstOrDefault();


                ((List<BEUsuarioFilialLoja>)Session["UsuariosFiliais"]).Add(model.UsuarioFilialLoja);

                return Json(new JsonReturn(true));
            }
            catch (ValidationException ve)
            {
                return Json(new JsonReturn(ve.ValidationResults));
            }


        }

        [HttpGet]
        public JsonResult ObterUsuarioFilial(int? codigoUsuario, SearchModel searchModel)
        {
            IQueryable<BEUsuarioFilialLoja> model;
            if (codigoUsuario.HasValue)
            {
                model = ((List<BEUsuarioFilialLoja>)Session["UsuariosFiliais"]).Where(g => g.CodigoUsuario == codigoUsuario.GetValueOrDefault()).AsQueryable();
            }
            else
            {
                model = ((List<BEUsuarioFilialLoja>)Session["UsuariosFiliais"]).AsQueryable();
            }


            GridData gridData = model.ToGridData(searchModel, GridUsuarioFilialLojaModel.BEUsuarioFilialLojaColumns);
            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletarUsuarioFilial(BEUsuarioFilialLoja beUsuarioFilialLoja)
        {
            List<object> gridResponse = new List<object>();
            try
            {
                BEUsuarioFilialLoja beOld = BPFUsuarioFilialLoja.Instance.ObterTodos(new FEUsuarioFilialLoja() { Codigo = beUsuarioFilialLoja.Codigo }).ResultList.FirstOrDefault();
                if (beOld != null)
                {
                    BPFUsuarioFilialLoja.Instance.Excluir(beOld);
                    gridResponse = GridResponse.CreateSuccess();
                }
                else
                {
                    gridResponse = GridResponse.CreateFailure("Associação não encontrada para ser excluída.");
                }
            }
            catch (Exception)
            {
                gridResponse = GridResponse.CreateFailure("Associação não pode ser removida.");
            }

            return Json(gridResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvarUsuarioFilial(PDAcesso model)
        {
            LimparMensagens();
            try
            {

                model.ListUsuarioFilialLoja = ((List<BEUsuarioFilialLoja>)Session["UsuariosFiliais"]);

                foreach (BEUsuarioFilialLoja item in model.ListUsuarioFilialLoja)
                {
                    //item.CodigoUsuario = this.ProcessData.Usuario.Codigo;
                    this.ProcessData.UsuarioFilialLoja = BPFUsuarioFilialLoja.Instance.Salvar(item);
                    this.ProcessData.ListUsuarioFilialLoja.Add(this.ProcessData.UsuarioFilialLoja);
                }


                base.AdicionarMensagemSucesso("Usuário Salvo com Sucesso!");
            }
            catch (Exception ex)
            {
                base.AdicionarMensagemErro(ex, ex.Message);
            }


            return RedirectToAction("AssociaUsuarioFilialLoja", new { id = this.ProcessData.UsuarioFilialLoja.CodigoUsuario });
        }


        #endregion

        private void CarregaCombos()
        {
            var grupos = from f in BPFGrupoPermissao.Instance.ObterTodos(new FEGrupoPermissao() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                         select new { CodigoGrupo = f.Codigo, f.Descricao };

            TempData["Grupos"] = new SelectList(grupos, "CodigoGrupo", "Descricao");

            var filialLoja = from f in BPFFilialLoja.Instance.ObterTodos(new FEFilialLoja() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                         select new { CodigoFilial = f.Codigo, f.Nome };

            TempData["FilialLoja"] = new SelectList(filialLoja, "CodigoFilial", "Nome");

            var usuario = from f in BPFUsuario.Instance.ObterTodos(new FEUsuario() { CodigoLoja = UsuarioLogin.CodigoLoja }).ResultList
                             select new { CodigoUsuario = f.Codigo, f.NomeUsuario };

            TempData["Usuario"] = new SelectList(usuario, "CodigoUsuario", "NomeUsuario");
        }



        #endregion
    }
}
