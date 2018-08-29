﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.ComponentModel.DataAnnotations;

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
            this.Warnings = new List<Warning>();
           
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
        public Nullable<decimal> SDLeaveBalance { get; set; }
        public DateTime? LeaveSettingDate { get; set; }
        public Nullable<Boolean> Status { get; set; }
        [Required]
        public virtual AppUser AppUser { get; set; }
        public virtual IList<Contract> Contracts { get; set; }
        public virtual IList<Education> Educations { get; set; }
        public virtual IList<EmergencyContact> EmergencyContacts { get; set; }
        public virtual IList<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual IList<FamilyDetail> FamilyDetails { get; set; }
        public virtual IList<Termination> Terminations { get; set; }
        public virtual IList<WorkExperience> WorkExperiences { get; set; }

        public virtual IList<Warning> Warnings { get; set; }


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
        public virtual Contract GetContract(int Id)
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
        public virtual Education GetEducation(int Id)
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
        public virtual EmergencyContact GetEmergencyContact(int Id)
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
        public virtual FamilyDetail GetFamilyDetail(int Id)
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
        public virtual WorkExperience GetWorkExperience(int Id)
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
        #region Employee detail
        public virtual string GetEmployeeDutyStation()
        {
            if (EmployeeDetails.Count != 0)

                return EmployeeDetails.Last().DutyStation;
            else
                return "";
        }
        public virtual string GetEmployeeProgram()
        {
            if (EmployeeDetails.Count != 0)

                return EmployeeDetails.Last().Program.ProgramName;
            else
                return "";
        }
        public virtual string GetEmployeePosition()
        {
            if (EmployeeDetails.Count != 0)

                return EmployeeDetails.Last().Position.PositionName;
            else
                return "";
        }
        #endregion

        #region Leave calculation Methods
        public virtual DateTime GetEmployeeHiredDate()
        {
            if (Contracts.Count != 0)
            {
                foreach (Contract cn in Contracts)
                {
                    if (cn.Reason == "ReHired")
                        return cn.ContractStartDate;
                }
                return Contracts[0].ContractStartDate;
            }
            else
                return new DateTime();
            
        }

        public virtual decimal Leavefromhiredtonow()
        {
            DateTime DateHired = GetEmployeeHiredDate();
           
            decimal leaveEnti = 0;
            decimal Sumleave = 0;
            TimeSpan workingdays = DateTime.Now - DateHired;
            decimal wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd < 365)
                {

                   // leaveEnti = leaveEnti + Math.Round(wd / 365);
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
                }
            }
            else { //leaveEnti = leaveEnti + Math.Round(wd / 365);
                   Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
            }

            return Sumleave;
        }
        public virtual decimal LeavefromhiredtoYE()
        {
            DateTime YE = new  DateTime(DateTime.Now.Year, 12, 31);

            decimal leaveEnti = 0;
            decimal Sumleave = 0;
            TimeSpan workingdays = YE - GetEmployeeHiredDate();
            decimal wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd < 365)
                {
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
                }
            }
            else { Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365)); }

            return Sumleave;
        }
        public virtual decimal LeavefromhiredtoCED(DateTime CED)
        {
            DateTime ced = CED;

            decimal leaveEnti = 0;
            decimal Sumleave = 0;
            TimeSpan workingdays =  ced - GetEmployeeHiredDate();
            decimal wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd < 365)
                {
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
                }
            }
            else { Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365)); }

            return Sumleave;
        }

        public virtual decimal LeavefromhiredtoSettingDate()
        {
            decimal leaveEnti = 0;
            decimal Sumleave = 0;
            TimeSpan workingdays = LeaveSettingDate.Value - GetEmployeeHiredDate();
            decimal wd = workingdays.Days;
            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd < 365)
                {
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
                }
            }
            else { Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365)); }

            return Sumleave;
        }

        public virtual decimal EmployeeLeaveBalance()
        {
            return (Leavefromhiredtonow() - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value;
        }
        public virtual decimal EmployeeLeaveBalanceCED(DateTime CED)
        {
            return (LeavefromhiredtoCED(CED) - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value;
        }
        public virtual decimal EmployeeLeaveBalanceYE()
        {
            return (LeavefromhiredtoYE() - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value;
        }
        #endregion

    }
}
