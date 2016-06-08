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
using WebAppEFTest.Filters;

namespace WebAppEFTest.Areas.SPA.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class MainController : Controller
    {
        // GET: SPA/Main
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MainViewModel v = new MainViewModel();
            v.UserName = User.Identity.Name;
            v.FooterData = new FooterViewModel();
            v.FooterData.CompanyName = "Microsoft";
            v.FooterData.Year = DateTime.Now.Year.ToString();

            return View("Index", v);
        }

        /// <summary>
        /// Employees the list.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the add new link.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Adds the new.
        /// </summary>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult AddNew()
        {
            CreateEmployeeViewModel v = new CreateEmployeeViewModel();
            return PartialView("CreateEmployee", v);
        }

        /// <summary>
        /// Saves the employee.
        /// </summary>
        /// <param name="emp">The emp.</param>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult SaveEmployee(Employee emp)
        {
            EmployeeBusinesslayer empBal = new EmployeeBusinesslayer();
            empBal.SaveEmployee(emp);

            EmployeeViewModel empViewModel = new EmployeeViewModel();
            empViewModel.EmployeeName = emp.FirstName + " " + emp.LastName;
            empViewModel.Salary = emp.Salary.ToString("C");

            if (emp.Salary > 15000)
            {
                empViewModel.SalaryColor = "Yellow";
            }
            else
            {
                empViewModel.SalaryColor = "Green";
            }

            return Json(empViewModel);
        }
    }
}