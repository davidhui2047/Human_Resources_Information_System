using HRIS.Database;
using HRIS.Teaching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HRIS.Control
{
    class StaffController
    {
        //list of all staff members
        private static List<Staff> staffMembers;
        //list of viewable staff members - It is good to separate these lists in cases where filtering will need to be applied.
        public ObservableCollection<Staff> ViewableStaffMembers;

        public ObservableCollection<Consultation> Consultations = new ObservableCollection<Consultation>();

        public void LoadStaff()
        {
            staffMembers = new List<Staff>(SchoolDBAdapter.FetchBasicStaffDetails());
            ViewableStaffMembers = new ObservableCollection<Staff>(staffMembers);
        }

        public void LoadCompleteStaffDetails(Staff s)
        {
            SchoolDBAdapter.FetchCompleteStaffDetails(s);
        }

        public ObservableCollection<Staff> GetViewableStaffMembers()
        {
            ViewableStaffMembers = new ObservableCollection<Staff>(staffMembers);
            return ViewableStaffMembers;
        }

        public void FilterBy(Category category)
        {
            var filtered = from Staff s in staffMembers
                           where s.Category == category
                           select s;
            ViewableStaffMembers.Clear();
            foreach (Staff s in filtered)
                ViewableStaffMembers.Add(s);
        }

        public void FilterByName(String name)
        {
            var filtered = from Staff s in staffMembers
                           where ((string)s.GivenName.ToLower() + " " + s.FamilyName.ToLower()).Contains(name.ToLower())
                           select s;
            ViewableStaffMembers.Clear();
            foreach (Staff s in filtered)
                ViewableStaffMembers.Add(s);
        }

        public Staff FindStaffMember(int id)
        {
            Staff staff = (from Staff s in staffMembers
                           where s.id == id
                           select s).First();
            return staff;
        }

       

        public void RefreshViewableStaff()
        {
            ViewableStaffMembers.Clear();
            foreach (Staff s in staffMembers)
            {
                ViewableStaffMembers.Add(s);
            }
        }

        public void LoadConsultations()
        {
            Consultations = new ObservableCollection<Consultation>(SchoolDBAdapter.FetchConsultations());
        }

        public List<Consultation> FindConsultations(Staff s)
        {
            var filtered = from Consultation c in Consultations
                           where c.StaffID == s.id
                           select c;
            return new List<Consultation>(filtered);
        }

        public Staff FindClassTeacher(UnitClass uc)
        {
            var teacher = (from Staff s in staffMembers
                          where s.id == uc.staffID
                          select s).First();
            return teacher;
        }
    }
}
