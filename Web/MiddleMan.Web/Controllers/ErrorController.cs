using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleMan.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/error/404")]
        public IActionResult NotFound()
        {
            return this.View();
        }
    }
}
