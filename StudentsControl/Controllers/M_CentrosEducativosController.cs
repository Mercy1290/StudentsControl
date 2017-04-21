using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentsControl.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace StudentsControl.Controllers
{
    public class M_CentrosEducativosController : ApiController
    {
        //Inicializando Conexion a Base de Datos
        StudentsControlContext _context = new StudentsControlContext();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(_context.CentrosEducativos, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form)
        {
            var values = form.Get("values");

            var newOrder = new CentroEducativo();
            JsonConvert.PopulateObject(values, newOrder);

            Validate(newOrder);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            _context.CentrosEducativos.Add(newOrder);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            var order = _context.CentrosEducativos.First(o => o.Id == key);

            JsonConvert.PopulateObject(values, order);

            Validate(order);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public void Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var order = _context.CentrosEducativos.First(o => o.Id == key);

            _context.CentrosEducativos.Remove(order);
            _context.SaveChanges();
        }
    }
}
