using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppEFTest.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public string Index(int id1, int? id2)
        {
            return "id1=" + id1 + ";  id2=" + id2;
        }
    }
}