using BusinessEntities;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;
using WebAppEFTest.Filters;
using WebAppEFTest.Models;

namespace WebAppEFTest.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Test
        [HeaderFilter]
        public ActionResult Index()
        {
            EmployeeBusinesslayer helper = new EmployeeBusinesslayer();
            //List<Employee> employees = helper.GetEmployees();

            //EmployeeListViewModel employeeModel = new EmployeeListViewModel();
            //employeeModel.UserName = "WWJ";
            //employeeModel.Employees.Add(new Employee() { EmployeeId = "1", FirstName = "2", LastName = "3", Salary = 100 });

            //employeeModel.FooterData = new FooterViewModel();
            //employeeModel.FooterData.CompanyName = "Microsoft";
            //employeeModel.FooterData.Year = DateTime.Now.Year.ToString();

            //return View("Index", employeeModel);
            return View();
        }

        //[HeaderFilter]
        //[AdminFilter]
        //public ActionResult AddNew()
        //{
        //    Employee employee = new Employee();
        //    employee.UserName = "Admin";
        //    employee.FooterData = new FooterViewModel();
        //    employee.FooterData.CompanyName = "Microsoft";
        //    employee.FooterData.Year = DateTime.Now.Year.ToString();

        //    return View("AddNewEmployee", employee);
        //}
        
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

        //[HeaderFilter]
        //[AdminFilter]
        //public ActionResult SaveEmployee(Employee e)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        Employee employee = new Employee();
        //        employee.UserName = "Admin";
        //        employee.FooterData = new FooterViewModel();
        //        employee.FooterData.CompanyName = "Microsoft";
        //        employee.FooterData.Year = DateTime.Now.Year.ToString();

        //        return View("AddNewEmployee", employee);
        //    }
        //}
    }
}