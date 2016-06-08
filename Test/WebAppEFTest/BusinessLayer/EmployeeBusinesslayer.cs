using BusinessEntities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class EmployeeBusinesslayer
    {
        public List<Employee> GetEmployees()
        {
            EFTestDBDAL dal = new EFTestDBDAL();
            return dal.Employees.ToList();
        }

        public bool IsValudUser(UserDetails u)
        {
            if (u.UserName == "Admin" && u.Password == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserStatus GetUserValidity(UserDetails u)
        {
            if (u.UserName == "Admin" && u.Password == "Admin")
            {
                return UserStatus.AuthenticatedAdmin;
            }
            else if (u.UserName == "Sukesh" && u.Password == "Sukesh")
            {
                return UserStatus.AuthentuatedUser;
            }
            else
            {
                return UserStatus.NonAuthenticatedUser;
            }
        }

        public void UploadEmployees(List<Employee> employees)
        {

        }

        public void SaveEmployee(Employee employee)
        {
            EFTestDBDAL dal = new EFTestDBDAL();
            employee.EmployeeId = Guid.NewGuid().ToString();
            dal.Employees.Add(employee);
            dal.SaveChanges();            
        }
    }
}