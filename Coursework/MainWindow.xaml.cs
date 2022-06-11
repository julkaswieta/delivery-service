/* 
 * Application name: ParcelTrack
 * Author: Julia Swietochowska, 40495101
 * Last modified: 27 / 11 / 2021
 */
using System;
using System.Collections.Generic;
using System.Windows;

namespace Coursework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // generate the list of all area objects
        private List<Area> areas = ParcelTrack.GenerateAreas();
        public MainWindow()
        {
            InitializeComponent();
            // add the areas to the areas combobox
            foreach (Area a in areas)
            {
                cmbAreas.Items.Add(a.Code);
            }
        }

        /* 
         * Handles adding a new courier to the system 
         */ 
        private void btnAddCourier_Click(object sender, RoutedEventArgs e)
        {
            // Validate the inputs 
            try
            {
                // check if the name,  type  and areas are specified
                if (String.IsNullOrEmpty(txtCourierName.Text))
                {
                    throw new ArgumentException("Name cannot be blank");
                }
                if (String.IsNullOrEmpty(cmbCourierType.Text))
                {
                    throw new ArgumentException("Type myst be specified");
                }
                if (listAreas.Items.Count < 1)
                {
                    throw new ArgumentException("At least one area must be selected");
                }

                // map the codes entered in the WPF to Area objects created 
                // get the suitable Area objects and put them in an individual area list for the courier
                List<Area> courierAreas = new List<Area>();
                foreach (String item in listAreas.Items)
                {
                    foreach (Area a in ParcelTrack.Areas)
                    {
                        if (a.Code.Equals(item))
                        {
                            courierAreas.Add(a);
                            break;
                        }
                    }
                }

                // add a new courier to the system
                try
                {
                    Courier c = ParcelTrack.AddCourier(txtCourierName.Text, cmbCourierType.Text, courierAreas);
                    MessageBox.Show("Courier successfully created. Allocated id: " + c.Id);

                    // Add the courier ID to the combobox fore transfering the parcel
                    // and to the courier schedule display combobox
                    cmbCourierID.Items.Add(c.Id + " " + c.Name);
                    cmbCouriers.Items.Add(c.Id + " " + c.Name);
                    // Clear the fields for another potential courier
                    listAreas.Items.Clear();
                    cmbCourierType.Text = "";
                    txtCourierName.Text = "";
                }
                catch
                {
                    MessageBox.Show("Something went wrong. Please try again");
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*
         * Handles adding an area code to the area list (ListBox)
         */
        private void btnAddArea_Click(object sender, RoutedEventArgs e)
        {
            // Validate the area selection
            try
            {
                // First, make the user choose the type to ensure all checks are carried out
                if(cmbCourierType.Text == "")
                {
                    throw new Exception("Please select courier type first");
                }

                // Make sure an area was selected
                if (cmbAreas.SelectedItem == null)
                {
                    throw new Exception("Please select an area first");
                }
                // Make sure only one are is added for walking and cycling couriers
                if (cmbCourierType.Text.Equals("Walking") || cmbCourierType.Text.Equals("Cycle"))
                {
                    if (listAreas.Items.Count >= 1)
                    {
                        throw new Exception(cmbCourierType.Text + " courier is limited to 1 area.");
                    }
                }
                // check if the area for a walking courier is between EH1 and EH4
                if (cmbCourierType.Text.Equals("Walking"))
                {
                    if (!cmbAreas.Text.Equals("EH1") && !cmbAreas.Text.Equals("EH2") && !cmbAreas.Text.Equals("EH3") && !cmbAreas.Text.Equals("EH4"))
                    {
                        throw new Exception("Walking courier may operate in areas EH1-EH4 only.");
                    }
                }
                // don't let duplicate areas be added
                foreach (string area in listAreas.Items)
                {
                    if (area.Equals(cmbAreas.Text))
                    {
                        throw new Exception("Duplicate areas selected. Please select another area");
                    }
                }
                // if no errors found, add the area to the list of areas
                listAreas.Items.Add(cmbAreas.Text);
                // clear the choice
                cmbAreas.Text = "";
            }
            catch (Exception ex)
            {
                // if an error was found, display an error message 
                MessageBox.Show(ex.Message);
                cmbAreas.Text = "";
            }
        }

        /*
         * Handles adding a new parcel to the system
         */
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            // Validate the inputs 
            try
            {
                // 1. check if all three fields were specified
                if (String.IsNullOrEmpty(txtPostcode.Text))
                {
                    throw new ArgumentException("Postcode cannot be blank");
                }
                if (String.IsNullOrEmpty(txtAddressee.Text))
                {
                    throw new ArgumentException("Addressee cannot be blank");
                }
                if (String.IsNullOrEmpty(txtAddress.Text))
                {
                    throw new ArgumentException("Address cannot be blank");
                }
                bool success = true;
                string deliveryID = "";

                // 2. check if the postcode is in the correct format
                try
                {
                    deliveryID = txtPostcode.Text.Split(" ")[1];
                }
                catch
                {
                    success = false;
                    MessageBox.Show("Postcode must be in the correct format: \nEHx xxx");
                }
                if (deliveryID == "")
                {
                    throw new ArgumentException("Delivery ID cannot be blank");
                }

                // 3. check if the area is correct
                bool correct = false;
                foreach (Area area in areas)
                {
                    if (txtPostcode.Text.Split(" ")[0].Equals(area.Code))
                    {
                        correct = true;
                        break;
                    }
                }
                if (!correct)
                {
                    throw new ArgumentException("Area could not be identified. Accepted areas EH1-EH22");
                }

                // 4. check if the delivery ID is unique
                foreach (Parcel p in ParcelTrack.Parcels)
                {
                    string id1 = p.Postcode.Split(" ")[1];
                    if (deliveryID.Equals(id1))
                    {
                        throw new ArgumentException("The delivery ID must be unique. Please try again");
                    }
                }

                string addressee = txtAddressee.Text + ", " + txtAddress.Text;

                // if all inputs seem to be correct, create a new parcel object and allocate it
                // if it can't be allocated, return an error message
                if (success)
                {
                    MessageBox.Show(ParcelTrack.AddParcel(addressee, txtPostcode.Text));
                    // Add the parcel's delivery ID to the combobox for transfering a parcel
                    cmbPostcode.Items.Add(txtPostcode.Text);
                }

                // Clean the fields to prepare for entering a new parcel
                txtPostcode.Text = "";
                txtAddressee.Text = "";
                txtAddress.Text = "";
            }
            catch (ArgumentException ex)
            {
                // if any errors were found, display an error message
                MessageBox.Show(ex.Message);
            }
        }

        /*
         * Handles transfering a parcel to another courier
         * Also works for unassigned parcels
         */
        private void btnTransferParcel_Click(object sender, RoutedEventArgs e)
        {
            // Validate the inputs
            // Both are selected from a list so they are correct
            try
            {
                // Check if both were selected 
                if (String.IsNullOrEmpty(cmbPostcode.Text))
                {
                    throw new ArgumentException("Delivery ID must be selected");
                }
                if (String.IsNullOrEmpty(cmbCourierID.Text))
                {
                    throw new ArgumentException("Courier ID must be selected");
                }

                // Try to transfer the parcel 
                // Return an error message if it cannot be allocated
                MessageBox.Show(ParcelTrack.TransferParcel(cmbPostcode.Text, Int32.Parse(cmbCourierID.Text.Split(" ")[0])));

                // Clean the fields 
                cmbPostcode.Text = "";
                cmbCourierID.Text = "";

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*
         * Handles printing the area summary 
         */
        private void btnDisplaySummary_Click(object sender, RoutedEventArgs e)
        {
            // clear the list before displaying anything
            listSummary.Items.Clear();
            // Check if there is anything to display
            try
            {
                List<string> areaSummary = ParcelTrack.GetAreaSummary();
                if (areaSummary.Count == 0)
                {
                    throw new Exception("No data to display");
                }
                foreach (string line in areaSummary)
                {
                    listSummary.Items.Add(line);
                }
            }
            // Display a message if there are no couriers to display
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*
         * Handles displaying delivery schedule for each courier
         */
        private void btnDisplayDeliveries_Click(object sender, RoutedEventArgs e)
        {
            // Check if a courier was selected
            try
            {
                listCourierDeliveries.Items.Clear();
                if (String.IsNullOrEmpty(cmbCouriers.Text))
                {
                    throw new ArgumentException("Please select a courier first");
                }
                // if no errors, display the schedule
                // check if it's not empty list
                int id = Int32.Parse(cmbCouriers.Text.Split(" ")[0]);
                List<string> schedule = ParcelTrack.GetCourierSchedule(id);
                if (schedule.Count == 0)
                {
                    throw new ArgumentException("No deliveries to display");
                }
                else
                {
                    foreach (string line in schedule)
                    {
                        listCourierDeliveries.Items.Add(line);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool read = false;
        /*
         * Handles the reading to the .csv file
         */
        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            string message;
            if (!read)
            {
                // note if the data has already been read
                message = ParcelTrack.ReadCSV();
                read = true;
            }
            else
            {
                message = "Data already read";
            }
            
            // add each courier's IDs and names to the comboboxes
            foreach (Courier c in ParcelTrack.Couriers)
            {
                if(!cmbCouriers.Items.Contains(c.Id + " " + c.Name))
                {
                    cmbCouriers.Items.Add(c.Id + " " + c.Name);
                }
                if (!cmbCourierID.Items.Contains(c.Id + " " + c.Name))
                {
                    cmbCourierID.Items.Add(c.Id + " " + c.Name);
                }
            }

            // add each parcels postcodes to the combobox
            foreach (Parcel p in ParcelTrack.Parcels)
            {
                if(!cmbPostcode.Items.Contains(p.Postcode))
                {
                    cmbPostcode.Items.Add(p.Postcode);
                }
            }

            MessageBox.Show(message);
        }

        /*
         * Handles writing to the .csv file
         */
        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ParcelTrack.WriteToCSV());
        }
    }
}
