using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Issue : IEntity
    {
        public Issue()
        {
            IssueDetails = new List<IssueDetail>();
        }
        public int Id { get; set; }        
        public string IssueNo { get; set; }
        public string Purpose { get; set; }
        public int IssuedTo { get; set; }      
        public Nullable<DateTime> IssueDate { get; set; }
        public int HandedOverBy { get; set; }
        public IList<IssueDetail> IssueDetails { get; set; }
        public virtual IssueDetail GetIssueDetail(int Id)
        {
            foreach (IssueDetail issueDet in IssueDetails)
            {
                if (issueDet.Id == Id)
                    return issueDet;
            }
            return null;
        }
        public virtual void RemoveIssueDetail(int Id)
        {
            foreach (IssueDetail issueDet in IssueDetails)
            {
                if (issueDet.Id == Id)
                    IssueDetails.Remove(issueDet);
                break;
            }
        }
    }
}
