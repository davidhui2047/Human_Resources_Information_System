using HRIS.Database;
using HRIS.Teaching;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HRIS.Control
{
    class UnitController
    {
        private static List<Unit> units = new List<Unit>();
        public ObservableCollection<Unit> ViewableUnits;

        public static List<UnitClass> classes = new List<UnitClass>();
        public ObservableCollection<UnitClass> ViewableClasses;

        public List<UnitClass> GetClasses ()
        {
            return classes;
        }

        public ObservableCollection<Unit> GetViewableUnits()
        {
            ViewableUnits = new ObservableCollection<Unit>(units);
            return ViewableUnits;
        }

        public void RefreshViewableClasses(Unit u)
        {
            ViewableClasses.Clear();
            foreach (UnitClass uc in classes)
                if(u != null && uc.UnitCode == u.Code)
                    ViewableClasses.Add(uc);
        }

        public void LoadUnits()
        {
            units = new List<Unit>(SchoolDBAdapter.FetchUnits());
        }

        public void LoadClasses()
        {
            SchoolDBAdapter.FetchClasses();
            ViewableClasses = new ObservableCollection<UnitClass>(classes);
        }

        public List<Unit> FindUnits(Staff s)
        {
            var filtered = from Unit u in units
                           where u.CoordinatorID == s.id
                           select u;
            return new List<Unit>(filtered);
        }
        /*public ObservableCollection<UnitClass> FindClass(Unit u)
        {
            var filtered = from UnitClass uc in classes
                           where u.Code == uc.UnitCode
                           select uc;
            return new ObservableCollection<UnitClass>(filtered);
        }*/

        public void FindClass(Unit u)
        {
            var filtered = from UnitClass uc in classes
                           where u.Code == uc.UnitCode
                           select uc;
            ViewableClasses.Clear();
            foreach (UnitClass uc in filtered)
                ViewableClasses.Add(uc);
        }

        public void FilterByCampus(Campus campus, Unit u)
        {
            var filtered = from UnitClass uc in classes
                           where uc.Campus == campus && uc.UnitCode == u.Code
                           select uc;
            ViewableClasses.Clear();
            foreach (UnitClass uc in filtered)
                ViewableClasses.Add(uc);
        }
    }
}
