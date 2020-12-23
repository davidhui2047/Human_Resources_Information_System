using HRIS.Control;
using HRIS.Teaching;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HRIS.Database
{
    abstract class SchoolDBAdapter
    {
        //Connection strings
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        private static MySqlConnection conn = null;
        //A method that opens a connection to the database
        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                string connectionString = String.Format("Database={0};Data Source={1};User ID={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        //Fetch a list of staff from the database, including their basic details
        public static List<Staff> FetchBasicStaffDetails()
        {
            List<Staff> staff = new List<Staff>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, title, campus, category from staff", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    staff.Add(new Staff { id = rdr.GetInt32(0), GivenName = rdr.GetString(1), FamilyName = rdr.GetString(2), Title = rdr.GetString(3), campus = ParseEnum<Campus>(rdr.GetString(4)), Category = ParseEnum<Category>(rdr.GetString(5)) });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to databse: " + e);
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (conn != null)
                    conn.Close();
            }
            return staff;
        }

        //Fetch the complete details of a particular staff memeber from the database
        public static Staff FetchCompleteStaffDetails(Staff s)
        {
            Staff staff = new Staff();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                //select the following data for the staff member passed into the method
                MySqlCommand cmd = new MySqlCommand("select given_name, family_name, title, campus, phone, room, email, category, photo from staff where id=?id", conn);
                cmd.Parameters.AddWithValue("id", s.id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    s.GivenName = rdr.GetString(0);
                    s.FamilyName = rdr.GetString(1);
                    s.Title = rdr.GetString(2);
                    s.campus = ParseEnum<Campus>(rdr.GetString(3));
                    s.Phone = rdr.GetString(4);
                    s.Room = rdr.GetString(5);
                    s.Email = rdr.GetString(6);
                    s.Category = ParseEnum<Category>(rdr.GetString(7));
                    s.Photo = rdr.GetString(8);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to databse: " + e);
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (conn != null)
                    conn.Close();
            }
            return staff;
        }

        //Fetch a list of units from the database
        public static List<Unit> FetchUnits()
        {
            List<Unit> units = new List<Unit>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                //select the following data for the staff member passed into the method
                MySqlCommand cmd = new MySqlCommand("select code, title, coordinator from unit", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    units.Add(new Unit { Code = rdr.GetString(0), Title = rdr.GetString(1), CoordinatorID = rdr.GetInt32(2) });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to databse: " + e);
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (conn != null)
                    conn.Close();
            }
            return units;
        }

        //Fetch a list of classes for a particular unit from the database
        public static void FetchClasses()
        {
            List<UnitClass> classes = new List<UnitClass>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                //select the following data for the staff member passed into the method
                MySqlCommand cmd = new MySqlCommand("select unit_code, campus, day, start, end, type, room, staff from class", conn);
                 rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UnitClass uc = new UnitClass();
                    uc.UnitCode = rdr.GetString(0);
                    uc.Campus = ParseEnum<Campus>(rdr.GetString(1));
                    uc.Day = ParseEnum<DayOfWeek>(rdr.GetString(2));
                    uc.Start = rdr.GetTimeSpan(3);
                    uc.End = rdr.GetTimeSpan(4);
                    uc.Type = ParseEnum<ClassType>(rdr.GetString(5));
                    uc.Room = rdr.GetString(6);
                    uc.staffID = rdr.GetInt32(7);
                    classes.Add(uc);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to databse: " + e);
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (conn != null)
                    conn.Close();
            }
            UnitController.classes = classes;
        }

        public static List<Consultation> FetchConsultations()
        {
            List<Consultation> consultations = new List<Consultation>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                //select the following data for the staff member passed into the method
                MySqlCommand cmd = new MySqlCommand("select staff_id, day, start, end from consultation", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    consultations.Add(new Consultation { StaffID = rdr.GetInt32(0), Day = ParseEnum<DayOfWeek>(rdr.GetString(1)), Start = rdr.GetTimeSpan(2), End = rdr.GetTimeSpan(3) });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to databse: " + e);
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (conn != null)
                    conn.Close();
            }
            return consultations;
        }
    }
}
