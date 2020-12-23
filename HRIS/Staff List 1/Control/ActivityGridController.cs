using HRIS.Database;
using HRIS.Teaching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace HRIS.Control
{
    public class ActivityGridController
    {
        UnitController unitController;
        StaffController staffController;

        private ObservableCollection<ActivityRow> _ActivityGrid = new ObservableCollection<ActivityRow>();
        public ObservableCollection<ActivityRow> ActivityGrid
        {
            get { return _ActivityGrid; }
        }

        public void isBusy(ActivityRow ar, DayOfWeek day, int staffID)
        {
            var teaching = from UnitClass uc in unitController.GetClasses()
                           where uc.staffID == staffID
                                 && uc.Start.Hours == ar.Time.Hours
                                 || uc.End.Hours == ar.Time.Hours
                           select uc;
            if (teaching.Count() > 0)
                switch (day)
                {
                    case DayOfWeek.Monday:
                        ar.Monday = 1;
                        break;
                    case DayOfWeek.Tuesday:
                        ar.Tuesday = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        ar.Wednesday = 1;
                        break;
                    case DayOfWeek.Thursday:
                        ar.Thursday = 1;
                        break;
                    case DayOfWeek.Friday:
                        ar.Friday = 1;
                        break;
                    default:
                        break;
                }

            var consulting = from Consultation c in staffController.Consultations
                             where c.StaffID == staffID
                                   && c.Start.Hours == ar.Time.Hours
                                   || c.End.Hours == ar.Time.Hours
                             select c;
            if (teaching.Count() > 0)
                switch (day)
                {
                    case DayOfWeek.Monday:
                        ar.Monday = 2;
                        break;
                    case DayOfWeek.Tuesday:
                        ar.Tuesday = 2;
                        break;
                    case DayOfWeek.Wednesday:
                        ar.Wednesday = 2;
                        break;
                    case DayOfWeek.Thursday:
                        ar.Thursday = 2;
                        break;
                    case DayOfWeek.Friday:
                        ar.Friday = 2;
                        break;
                    default:
                        break;
                }
        }

        public void GenerateActivityRows()
        {
            for (int i = 0; i < 8; i++)
            {
                _ActivityGrid.Add(new ActivityRow { Time = new TimeSpan(i + 9, 0, 0) });

            }
        }
    }
}