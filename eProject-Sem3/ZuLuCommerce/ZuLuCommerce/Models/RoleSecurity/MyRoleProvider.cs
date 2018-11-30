using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ZuLuCommerce.Models
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }//chinh sua

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)//chinh sua
        {
            using (var db = new eCommerceEntities())
            {
                var emp = db.Employees.Find(int.Parse(username));
                if (emp == null)
                    return new string[] { };
                return emp.EmployeeLevels.Select(x => x.Level.LevelName).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)//chinh sua
        {
            using (var db = new eCommerceEntities())
            {
                var emp = db.Employees.Find(int.Parse(username));
                return emp.EmployeeLevels.Select(x => x.Level.LevelName).Contains(roleName);
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}