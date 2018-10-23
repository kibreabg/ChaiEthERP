using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class Contract : IEntity
    {
        public Contract()
        {
            this.EmployeeDetails = new List<EmployeeDetail>();


        }
        public int Id { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public virtual HRM.Employee Employee { get; set; }

        public virtual IList<EmployeeDetail> EmployeeDetails { get; set; }
        #region Employee Detail/History
        public virtual EmployeeDetail GetEmployeeDetails(int Id)
        {

            foreach (EmployeeDetail TR in EmployeeDetails)
            {
                if (TR.Id == Id)
                    return TR;
            }
            return null;
        }


        public virtual void RemoveEmployeeDetail(int Id)
        {
            foreach (EmployeeDetail TR in EmployeeDetails)
            {
                if (TR.Id == Id)
                {
                    EmployeeDetails.Remove(TR);
                    break;
                }
            }
        }
        #endregion


    }
}
