using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ServiceType : IEntity
    {
        public ServiceType()
        {
            this.ServiceTypeDetails = new List<ServiceTypeDetail>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int KmForService { get; set; }

        public virtual IList<ServiceTypeDetail> ServiceTypeDetails { get; set; }
        #region ServiceTypeDetail
        public virtual ServiceTypeDetail GetServiceTypeDetail(int Id)
        {

            foreach (ServiceTypeDetail STD in ServiceTypeDetails)
            {
                if (STD.Id == Id)
                    return STD;

            }
            return null;
        }
        public virtual IList<ServiceTypeDetail> GetServiceTypeDetailByServicTypeId(int ServId)
        {
            IList<ServiceTypeDetail> STD = new List<ServiceTypeDetail>();
            foreach (ServiceTypeDetail ST in ServiceTypeDetails)
            {
                if (ST.ServiceType.Id == ServId)
                    STD.Add(ST);

            }
            return STD;
        }
     
        public virtual void RemoveServiceTypeDetail(int Id)
        {

            foreach (ServiceTypeDetail STD in ServiceTypeDetails)
            {
                if (STD.Id == Id)
                    ServiceTypeDetails.Remove(STD);
                break;
            }

        }
        #endregion

    }
}
