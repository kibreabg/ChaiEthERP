using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class PaymentReimbursementRequestDetail : IEntity
    {
        public PaymentReimbursementRequestDetail()
        {
            this.PRAttachments = new List<PRAttachment>();

        }
 
        public int Id { get; set; }
        public decimal ActualExpendture { get; set; }
        public bool SupportDocAttached { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public string AccountCode { get; set; }
        public virtual IList<PRAttachment> PRAttachments { get; set; }
        #region PRAttachment

        public virtual void RemovePRAttachment(string FilePath)
        {
            foreach (PRAttachment cpa in PRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    PRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
        public virtual PaymentReimbursementRequest PaymentReimbursementRequest { get; set; }
    }
}
