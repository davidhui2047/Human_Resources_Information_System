using HRIS.Control;
using HRIS.Teaching;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HRIS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StaffController staffController = (StaffController)Application.Current.FindResource("staffController");
        UnitController unitController = (UnitController)Application.Current.FindResource("unitController");
        public MainWindow()
        {
            staffController.LoadStaff();
            unitController.LoadUnits();
            staffController.LoadConsultations();
            unitController.LoadClasses();

            foreach (Staff s in staffController.ViewableStaffMembers)
                staffController.LoadCompleteStaffDetails(s);
            InitializeComponent();
            UnitClassListView.ItemsSource = unitController.ViewableClasses;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StaffListBox.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("FamilyName", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("GivenName", ListSortDirection.Ascending));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnitCodeLabel.Content = "Unit Code: " + ((Unit)UnitListBox.SelectedItem).Code;
            UnitCoordinatorLabel.Content = "Unit Coordinator: " + ((Unit)UnitListBox.SelectedItem).CoordinatorID;
            unitController.FindClass((Unit)UnitListBox.SelectedItem);
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if(item != null)
            {
                StaffListBox.SelectedItem = staffController.FindClassTeacher(((UnitClass)UnitClassListView.SelectedItem));
                ListTabs.SelectedIndex = 0;
            }
        }

        void UpdateStaffDetails(Staff s)
        {

            NameLabel.Content = "Name: " + s.GivenName + " " + s.FamilyName;
            CampusLabel.Content = "Campus: " + s.campus;
            PhoneLabel.Content = "Phone: " + s.Phone;
            RoomLabel.Content = "Room: " + s.Room;
            EmailLabel.Content = "Email: " + s.Email;
            CategoryLabel.Content = "Category: " + s.Category;
            StaffImage.Source = new BitmapImage(new Uri(s.Photo));

          
            UnitsListView.ItemsSource = unitController.FindUnits(s);
            ConsultationsListView.ItemsSource = staffController.FindConsultations(s);
        }

        private void StaffListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStaffDetails((Staff)StaffListBox.SelectedItem);
        }

        private void CampusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CampusFilterComboBox.SelectedItem)
            {
                case Campus.Hobart:
                    unitController.FilterByCampus(Campus.Hobart, (Unit)UnitListBox.SelectedItem);
                    break;
                case Campus.Launceston:
                    unitController.FilterByCampus(Campus.Launceston, (Unit)UnitListBox.SelectedItem);
                    break;
                default:
                    unitController.RefreshViewableClasses((Unit)UnitListBox.SelectedItem);
                    break;
            }
        }

        private void CategoryFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CategoryFilterBox.SelectedItem)
            {
                case Category.Academic:
                    staffController.FilterBy(Category.Academic);
                    break;
                case Category.Admin:
                    staffController.FilterBy(Category.Admin);
                    break;
                case Category.Casual:
                    staffController.FilterBy(Category.Casual);
                    break;
                case Category.Technical:
                    staffController.FilterBy(Category.Technical);
                    break;
                default:
                    staffController.RefreshViewableStaff();
                    break;
            }

        }

        private void StaffFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            staffController.FilterByName(StaffFilterTextBox.Text);
        }

        private void ActivityGridButton_Click(object sender, RoutedEventArgs e)
        { 
            ActivityDataGrid.ItemsSource = null;
            //ActivityDataGrid.ItemsSource = 
            if (ActivityDataGrid.Visibility == Visibility.Hidden)
                ActivityDataGrid.Visibility = Visibility.Visible;
            else
                ActivityDataGrid.Visibility = Visibility.Hidden;
        }
    }
}
