using BusinessEntities;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppEFTest.Filters;
using WebAppEFTest.Models;

namespace WebAppEFTest.Controllers
{
    public class BulkUploadController : AsyncController
    {
        // GET: BulkUpload
        [HeaderFilter]
        [AdminFilter]
        public ActionResult Index()
        {
            return View(new FileUpLoadViewModel());
        }

        public async Task<ActionResult> Upload(FileUpLoadViewModel model)
        {
            int t1 = Thread.CurrentThread.ManagedThreadId;
            List<Employee> employees = await Task.Factory.StartNew<List<Employee>>(() => GetEmployees(model));
            int t2 = Thread.CurrentThread.ManagedThreadId;

            EmployeeBusinesslayer bal = new EmployeeBusinesslayer();
            bal.UploadEmployees(employees);
            return RedirectToAction("Index", "Employee");
        }

        private List<Employee> GetEmployees(FileUpLoadViewModel model)
        {
            List<Employee> employees = new List<Employee>();
            using (StreamReader csvReader = new StreamReader(model.fileUpload.InputStream))
            {
                csvReader.ReadLine();
                while (!csvReader.EndOfStream)
                {
                    var line = csvReader.ReadLine();
                    var values = line.Split(',');
                    Employee e = new Employee();
                    e.FirstName = values[0];
                    e.LastName = values[1];
                    e.Salary = int.Parse(values[2]);
                    employees.Add(e);
                }
            }

            return employees;
        }
    }
}