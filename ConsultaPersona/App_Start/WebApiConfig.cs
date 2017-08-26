using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ConsultaPersona.Models;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.Http.OData.Builder;

namespace ConsultaPersona
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Models.Persona>("Personas");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
            //config.EnableSystemDiagnosticsTracing();
        }
        
    }
}
