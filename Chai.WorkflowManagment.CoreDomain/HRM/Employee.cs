using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public class Employee :IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get;  set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string  Country { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string PersonalEmail { get; set; }
        public string ChaiEMail { get; set; }
        public string Photo { get; set; }
        public Boolean Status { get; set; }   
    }
}
