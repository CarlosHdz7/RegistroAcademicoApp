using Microsoft.AspNetCore.Mvc;
using RegistroAcademicoApp.Server.Models;
using RegistroAcademicoApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistroAcademicoApp.Server.Controllers
{
    public class FacultadController : Controller
    {
        [HttpGet]
        [Route("api/Facultad/obtenerFacultad/{idFacultad}")]
        public FacultadCls obtenerFacultad(int idFacultad)
        {
            FacultadCls fclCls = new FacultadCls();
            using (RegistroAcademicoContext db = new RegistroAcademicoContext())
            {
                fclCls = (from facultad in db.Facultad
                           where facultad.FacultadId == idFacultad
                           select new FacultadCls
                           {
                               FacultdadId = facultad.FacultadId,
                               FacultadNombre = facultad.Nombre,

                           }).First();

            }
            return fclCls;
        }

        [HttpPost]
        [Route("api/Facultad/Guardar")]
        public async Task<ActionResult<int>> Guardar(FacultadCls facultadCls)
        {

            int rpta = 0;
            try
            {

                using (RegistroAcademicoContext db = new RegistroAcademicoContext())
                {
                    Facultad oFacultad = new Facultad();
                    if (facultadCls.FacultdadId == 0)
                    {
                        oFacultad.FacultadId = facultadCls.FacultdadId;
                        oFacultad.Nombre = facultadCls.FacultadNombre;
                        db.Facultad.Add(oFacultad);
                    }
                    else
                    {
                        Facultad f = db.Facultad.Where(f => f.FacultadId == facultadCls.FacultdadId).FirstOrDefault();
                        f.FacultadId = facultadCls.FacultdadId;
                        f.Nombre = facultadCls.FacultadNombre;
                    }
                    await db.SaveChangesAsync();
                    rpta = 1;
                }


            }
            catch (Exception)
            {
                rpta = 0;
            }
            return rpta;
        }

        [HttpGet]
        [Route("api/Facultad/EliminarFacultad/{data?}")]
        public int eliminarFacultad(string data)
        {
            int rpta = 0;
            try
            {
                using (RegistroAcademicoContext db = new RegistroAcademicoContext())
                {
                    int idFacultad = int.Parse(data);
                    Facultad facultad = db.Facultad.Where(g => g.FacultadId == idFacultad).First();
                    db.Attach(facultad);
                    db.Remove(facultad);
                    db.SaveChanges();
                    rpta = 1;
                }
            }
            catch (Exception)
            {

                rpta = 0;
            }
            return rpta;
        }

        [HttpGet]
        [Route("api/Facultad/Filtrar/{data?}")]
        public List<Facultad> Filtrar(string data)
        {
            List<Facultad> lista = new List<Facultad>();
            using (RegistroAcademicoContext db = new RegistroAcademicoContext())
            {
                if (data == null)
                {
                    lista = (from f in db.Facultad
                             select new Facultad
                             {
                                 FacultadId = f.FacultadId,
                                 Nombre = f.Nombre
                             }).ToList();
                }

                else
                {
                    lista = (from f in db.Facultad
                             where f.FacultadId.ToString().Contains(data) || f.Nombre.Contains(data)
                             select new Facultad
                             {
                                 FacultadId = f.FacultadId,
                                 Nombre = f.Nombre
                             }).ToList();
                }
            }
            return lista;

        }

        [HttpGet]
        [Route("api/Facultad/Get")]
        public List<FacultadCls> Get()
        {
            List<FacultadCls> lista = new List<FacultadCls>();
            using (RegistroAcademicoContext db = new RegistroAcademicoContext())
            {
                lista = (from g in db.Facultad
                         select new FacultadCls
                         {
                             FacultdadId = g.FacultadId,
                             FacultadNombre = g.Nombre,
                         }).ToList();
            }
            return lista;

        }
    }
}
