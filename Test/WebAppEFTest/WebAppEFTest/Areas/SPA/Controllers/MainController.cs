using BusinessEntities;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;
using ViewModel.SPA;
using WebAppEFTest.Areas.SPA.Model;

namespace WebAppEFTest.Areas.SPA.Controllers
{
    public class MainController : Controller
    {
        // GET: SPA/Main
        public ActionResult Index()
        {
            MainViewModel v = new MainViewModel();
            v.UserName = User.Identity.Name;
            v.FooterData = new FooterViewModel();
            v.FooterData.CompanyName = "Microsoft";
            v.FooterData.Year = DateTime.Now.Year.ToString();

            return View("Index", v);
        }

        public ActionResult EmployeeList()
        {
            EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel();
            EmployeeBusinesslayer empBal = new EmployeeBusinesslayer();
            List<Employee> employees = empBal.GetEmployees();

            List<EmployeeViewModel> empViewModels = new List<EmployeeViewModel>();

            foreach (var emp in employees)
            {
                EmployeeViewModel empViewModel = new EmployeeViewModel();
                empViewModel.EmployeeName = emp.FirstName + "." + emp.LastName;
                empViewModel.Salary = emp.Salary.ToString("C");

                if (emp.Salary > 15000)
                {
                    empViewModel.SalaryColor = "yellow";
                }
                else
                {
                    empViewModel.SalaryColor = "green";
                }

                empViewModels.Add(empViewModel);
            }

            employeeListViewModel.employees = empViewModels;
            return PartialView("EmployeeList", employeeListViewModel);
        }

        public ActionResult GetAddNewLink()
        {
            if (Convert.ToBoolean(Session["IsAdmin"]))
            {
                return PartialView("AddNewLink");
            }
            else
            {
                return new EmptyResult();
            }
        }

    }
}