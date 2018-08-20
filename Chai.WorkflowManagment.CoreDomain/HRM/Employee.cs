using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class Employee : IEntity
    {
        public Employee()
        {

            this.Contracts = new List<Contract>();
            this.Educations = new List<Education>();
            this.EmergencyContacts = new List<EmergencyContact>();
            this.EmployeeDetails = new List<EmployeeDetail>();
            this.FamilyDetails = new List<FamilyDetail>();
            this.Terminations = new List<Termination>();
            this.WorkExperiences = new List<WorkExperience>();

        }
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string PersonalEmail { get; set; }
        public string ChaiEMail { get; set; }
        public string Photo { get; set; }
        public Boolean Status { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual IList<Contract> Contracts { get; set; }
        public virtual IList<Education> Educations { get; set; }
        public virtual IList<EmergencyContact> EmergencyContacts { get; set; }
        public virtual IList<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual IList<FamilyDetail> FamilyDetails { get; set; }
        public virtual IList<Termination> Terminations { get; set; }
        public virtual IList<WorkExperience> WorkExperiences { get; set; }


        #region EmployeeDetail
        public virtual EmployeeDetail GetEmployeeDetails(int Id)
        {

            foreach (EmployeeDetail ED in EmployeeDetails)
            {
                if (ED.Id == Id)
                    return ED;
            }
            return null;
        }


        public virtual void RemoveEmployeeDetail(int Id)
        {
            foreach (EmployeeDetail ED in EmployeeDetails)
            {
                if (ED.Id == Id)
                {
                    EmployeeDetails.Remove(ED);
                    break;
                }
            }
        }
        #endregion
        #region Contracts
        public virtual Contract GetContracts(int Id)
        {

            foreach (Contract CD in Contracts)
            {
                if (CD.Id == Id)
                    return CD;
            }
            return null;
        }


        public virtual void RemoveContract(int Id)
        {
            foreach (Contract CD in Contracts)
            {
                if (CD.Id == Id)
                {
                    Contracts.Remove(CD);
                    break;
                }
            }
        }
        #endregion
        #region Education
        public virtual Education GetEducations(int Id)
        {

            foreach (Education ED in Educations)
            {
                if (ED.Id == Id)
                    return ED;
            }
            return null;
        }


        public virtual void RemoveEducation(int Id)
        {
            foreach (Education ED in Educations)
            {
                if (ED.Id == Id)
                {
                    Educations.Remove(ED);
                    break;
                }
            }
        }
        #endregion
        #region EmergencyContact
        public virtual EmergencyContact GetEmergencyContacts(int Id)
        {

            foreach (EmergencyContact ED in EmergencyContacts)
            {
                if (ED.Id == Id)
                    return ED;
            }
            return null;
        }


        public virtual void RemoveEmergencyContact(int Id)
        {
            foreach (EmergencyContact ED in EmergencyContacts)
            {
                if (ED.Id == Id)
                {
                    EmergencyContacts.Remove(ED);
                    break;
                }
            }
        }
        #endregion
        #region FamilyDetail
        public virtual FamilyDetail GetFamilyDetails(int Id)
        {

            foreach (FamilyDetail FD in FamilyDetails)
            {
                if (FD.Id == Id)
                    return FD;
            }
            return null;
        }


        public virtual void RemoveFamilyDetail(int Id)
        {
            foreach (FamilyDetail FD in FamilyDetails)
            {
                if (FD.Id == Id)
                {
                    FamilyDetails.Remove(FD);
                    break;
                }
            }
        }
        #endregion
        #region Termination
        public virtual Termination GetTerminations(int Id)
        {

            foreach (Termination TD in Terminations)
            {
                if (TD.Id == Id)
                    return TD;
            }
            return null;
        }


        public virtual void RemoveTermination(int Id)
        {
            foreach (Termination TD in Terminations)
            {
                if (TD.Id == Id)
                {
                    Terminations.Remove(TD);
                    break;
                }
            }
        }
        #endregion
        #region WorkExperience
        public virtual WorkExperience GetWorkExperiences(int Id)
        {

            foreach (WorkExperience WE in WorkExperiences)
            {
                if (WE.Id == Id)
                    return WE;
            }
            return null;
        }


        public virtual void RemoveWorkExperience(int Id)
        {
            foreach (WorkExperience WE in WorkExperiences)
            {
                if (WE.Id == Id)
                {
                    WorkExperiences.Remove(WE);
                    break;
                }
            }
        }
        #endregion

    }
}
