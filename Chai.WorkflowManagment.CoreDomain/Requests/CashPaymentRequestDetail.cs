using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class CashPaymentRequestDetail : IEntity
    {
        public CashPaymentRequestDetail()
        {
            this.CPRAttachments = new List<CPRAttachment>();
        }
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal ActualExpendture { get; set; }
        public bool SupportDocAttached { get; set; }
        public virtual CashPaymentRequest CashPaymentRequest { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public string AccountCode { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual IList<CPRAttachment> CPRAttachments { get; set; }
        #region CPAttachment
        public virtual void RemoveCPAttachment(string FilePath)
        {
            foreach (CPRAttachment cpa in CPRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    CPRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
