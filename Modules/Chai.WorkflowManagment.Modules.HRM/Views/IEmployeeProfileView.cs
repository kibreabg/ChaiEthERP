using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.HRM.Views
{
    public interface IEmployeeProfileView
    {
        int GetEmployeeId { get; }
        string GetFirstName { get; }
        string GetLastName { get; }
        string GetGender { get; }
        DateTime GetDateOfBirth { get; }
        string GetMaritalStatus { get; }
        string GetNationality { get; }
        string GetAddress { get; }
        string GetCity { get; }
        string GetCountry { get; }
        string GetPhone { get; }
        string GetCellPhone { get; }
        string GetPersonalEmail { get; }
        string GetChaiEmail { get; }
        string GetPhoto { get; }
    }
}




