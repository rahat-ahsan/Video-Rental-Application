﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataLayer.Classes;

namespace VideoRental.UI
{
    public partial class Form1 : Form
    {
       
        private readonly DBManager DbManager;

        /* Name of Group Members

         * Rahat Ahsan Chowdhury
         * Ferdous Alam 
         * Christoffer Simon
                 
         
         */



        public Form1()
        {
            InitializeComponent();
           
            DbManager = new DBManager();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectTab(2);
                var booking = new Booking(DbManager);

                cboGenreGenre.DataSource = booking.GetGenre().ToList();
                cboGenreGenre.DisplayMember = "Name";

                cboVideoGenre.DataSource = booking.GetGenre().ToList();
                cboVideoGenre.DisplayMember = "Name";

                cboVideoVideo.DataSource = booking.GetVideo().ToList();
                cboVideoVideo.DisplayMember = "Name";

                cboCustomersName.DataSource = booking.GetCustomer().ToList();
                cboCustomersName.DisplayMember = "Name";


                cboRentCustomerName.DataSource = booking.GetCustomer().ToList();
                cboRentCustomerName.DisplayMember = "Name";

                FillAvailableVideoList();
                FillBookingList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
            }
            

        }



        #region Genre Events
        
        
        private void btnGenreAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string genre = txtGenreName.Text;
                if (string.IsNullOrWhiteSpace(genre) || genre.Any(Char.IsDigit))
                {
                    MessageBox.Show("Please enter valid Genre name.", "Error Message");
                    txtGenreName.Clear();
                    txtGenreName.Focus();
                }
                else
                {
                    var booking = new Booking(DbManager);
                    var result = booking.AddGenre(new Genre() {Name = txtGenreName.Text});
                    if (result)
                    {
                        cboGenreGenre.DataSource = booking.GetGenre();
                        cboGenreGenre.DisplayMember = "Name";
                        txtGenreName.Clear();

                        cboVideoGenre.DataSource = booking.GetGenre();
                        cboVideoGenre.DisplayMember = "Name";
                        MessageBox.Show(txtGenreName.Text + " genre added successfully.");
                        
                    }
                    else
                    {
                        MessageBox.Show(txtGenreName.Text + " genre already exists.");
                    }
                }


            }
            catch (VideoRentalException ex)
            {
                MessageBox.Show(String.Format("Error occured: {0} is not added succesfully", ex.Item.Name));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
            }


        }


        private void btnGenereDelete_Click(object sender, EventArgs e)
        {
            
            try
            {
                string genre = txtGenreName.Text;
                if (string.IsNullOrWhiteSpace(genre))
                {
                    MessageBox.Show("Please select Genre name from list.", "Error Message");
                    txtGenreName.Clear();
                    txtGenreName.Focus();
                }
                else
                {
                    var booking = new Booking(DbManager);
                    var result = booking.DeleteGenre((Genre) cboGenreGenre.SelectedItem);

                    if (result)
                    {
                        cboGenreGenre.DataSource = booking.GetGenre().ToList();
                        cboGenreGenre.DisplayMember = "Name";
                        txtGenreName.Clear();

                        cboVideoGenre.DataSource = booking.GetGenre().ToList();
                        cboVideoGenre.DisplayMember = "Name";

                        MessageBox.Show("Deletion succesfull.");
                    }
                    else
                    {
                        MessageBox.Show("Deletion not succesfull.");
                    }
                }
            }
            catch (VideoRentalException ex)
            {
                MessageBox.Show(String.Format("Error occured: {0} is not deleted succesfully", ex.Item.Name));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
            }
        }
        

        private void btnGenreUpdate_Click(object sender, EventArgs e)
        {
            var genre = txtGenreName.Text;

            if (string.IsNullOrWhiteSpace(genre) || genre.Any(Char.IsDigit))
            {
                MessageBox.Show("Please enter valid Genre name.", "Error Message");
                txtGenreName.Clear();
                txtGenreName.Focus();
            }
            else
            {
                try
                {
                    var booking = new Booking(DbManager);
                    var result = booking.UpdateGenre(((Genre)cboGenreGenre.SelectedItem).Id, new Genre(){ Name = txtGenreName.Text});
                    if (result)
                    {
                        cboGenreGenre.DataSource = booking.GetGenre().ToList();
                        cboGenreGenre.DisplayMember = "Name";
                        txtGenreName.Clear();

                        cboVideoGenre.DataSource = booking.GetGenre().ToList();
                        cboVideoGenre.DisplayMember = "Name";

                        FillBookingList();
                        FillAvailableVideoList();
                        MessageBox.Show("Succesfully updated ");

                    }
                    else
                    {
                        MessageBox.Show(String.Format("Error occured: {0} is not updated succesfully", ((Genre)cboGenreGenre.SelectedItem).Name));
                    }
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: {0} is not updated succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }

            }


        }

        private void cboGenreGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGenreName.Text = ((Genre)cboGenreGenre.SelectedItem).Name.ToString();

        }

        #endregion


        #region Video Events

        private void btnVideoAdd_Click(object sender, EventArgs e)
        {
            string video = txtVideoName.Text;
            if (string.IsNullOrWhiteSpace(video) || (nmrDaysToBeRent.Value == 0))
            {
                MessageBox.Show(string.Format("Please enter valid Video name or Number of days to be rented.", "Error Message"));
                txtVideoName.Focus();
            }
            else
            {
                try
                {
                    var booking = new Booking(DbManager);

                    var result =
                        booking.AddVideo(new Video()
                        {
                             GenreId = ((Genre)cboVideoGenre.SelectedItem).Id, 
                             Name = txtVideoName.Text, 
                             DaysToBeRent = Convert.ToInt16(nmrDaysToBeRent.Value)
                        });
                    if (result)
                    {
                        cboVideoVideo.DataSource = booking.GetVideo().ToList();
                        cboVideoVideo.DisplayMember = "Name";
                        txtVideoName.Clear();

                        FillBookingList();
                        FillAvailableVideoList();

                        MessageBox.Show("Successfully Added.");
                    }
                    else
                    {
                        MessageBox.Show(txtVideoName.Text + " video already exists.");
                    }
                }

                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: {0} is not added succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }
            }
        }

        private void btnVideoDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string video = txtVideoName.Text;
                if (string.IsNullOrWhiteSpace(video))
                {
                    MessageBox.Show("Please select Video from list.", "Error Message");
                    txtVideoName.Clear();
                    txtVideoName.Focus();
                }
                else
                {
                    var booking = new Booking(DbManager);
                    var result = booking.DeleteVideo((Video) cboVideoVideo.SelectedItem);

                    if (result)
                    {
                        cboVideoVideo.DataSource = booking.GetVideo().ToList();
                        cboVideoVideo.DisplayMember = "Name";
                        txtVideoName.Clear();

                        cboVideoGenre.DataSource = booking.GetGenre().ToList();
                        cboVideoGenre.DisplayMember = "Name";
                        FillAvailableVideoList();

                        MessageBox.Show("Successfully Deleted.");

                    }
                    else
                    {
                        MessageBox.Show("Deletion not succesfull.");
                    }
                }
            }
            catch (VideoRentalException ex)
            {
                MessageBox.Show(String.Format("Error Occured: {0} is not deleted succesfully", ex.Item.Name));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
            }
        }
        

        private void btnVideoUpdate_Click(object sender, EventArgs e)
        {
            var video = txtVideoName.Text;

            if (string.IsNullOrWhiteSpace(video) || (nmrDaysToBeRent.Value == 0))
            {
                MessageBox.Show("Please enter valid Video name.", "Error Message");
                txtVideoName.Clear();
                txtVideoName.Focus();
            }
            else
            {
                try
                {
                    var booking = new Booking(DbManager);
                    var result = booking.UpdateVideo(((Video)cboVideoVideo.SelectedItem).Id,
                                                    new Video()
                                                    {GenreId = ((Genre)cboVideoGenre.SelectedItem).Id,
                                                    Name = txtVideoName.Text,
                                                    DaysToBeRent = Convert.ToInt16(nmrDaysToBeRent.Value)});
                    if (result)
                    {
                        cboVideoVideo.DataSource = booking.GetVideo().ToList();
                        cboVideoVideo.DisplayMember = "Name";
                        txtVideoName.Clear();

                        cboVideoGenre.DataSource = booking.GetGenre().ToList();
                        cboVideoGenre.DisplayMember = "Name";
                        //cboVideoGenre.SelectedText= ((Video)cboVideoVideo.SelectedItem).GenreId

                        FillBookingList();
                        FillAvailableVideoList();
                        MessageBox.Show("Successfully Updated.");

                    }
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: {0} is not updated succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }

            }

        }

        private void cboVideoVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                var booking = new Booking(DbManager);
            var video = (Video) cboVideoVideo.SelectedItem;
            var genre = booking.GetGenre().First(g => g.Id.Equals(video.GenreId));

            cboVideoGenre.SelectedIndex = cboVideoGenre.FindString(genre.Name);

            nmrDaysToBeRent.Value = Convert.ToDecimal(video.DaysToBeRent);
            txtVideoName.Text = video.Name;

            }

            catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: {0} is not updated succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }

        }

        #endregion


        #region Customer Events

        private void btnCustomersAdd_Click(object sender, EventArgs e)
        {
            string fname = txtCustomersFName.Text;
            string lname = txtCustomersLName.Text;

            if (string.IsNullOrWhiteSpace(fname) || (string.IsNullOrWhiteSpace(lname)))
            {
                MessageBox.Show("Please enter full customer name.", "Error Message");
                txtCustomersFName.Focus();
            }
            else
            {
                try
                {
                    var booking = new Booking(DbManager);

                    var result =
                        booking.AddCustomer(new Customer()
                            {
                                FirstName = txtCustomersFName.Text,
                                LastName = txtCustomersLName.Text
                            });

                    if (result)
                    {
                        cboCustomersName.DataSource = booking.GetCustomer().ToList();
                        cboCustomersName.DisplayMember = "Name";
                        txtCustomersFName.Clear();
                        txtCustomersLName.Clear();

                        cboRentCustomerName.DataSource = booking.GetCustomer().ToList();
                        cboRentCustomerName.DisplayMember = "Name";

                        MessageBox.Show("Successfully Added.");
                    }
                    else
                    {
                        MessageBox.Show("Name could not be added.");
                    }
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: {0} is not added succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }
            }
        }

        private void btnCustomersDelete_Click(object sender, EventArgs e)
        {
            string customerName = txtCustomersFName.Text + txtCustomersLName;
            if (string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("Please select Customer name from list.", "Error Message");
                txtCustomersFName.Clear();
                txtCustomersLName.Clear();
                txtCustomersFName.Focus();
            }
            else
            {
                try
                {
                    var booking = new Booking(DbManager);
                    var result = booking.DeleteCustomer((Customer)cboCustomersName.SelectedItem);

                    if (result)
                    {
                        cboCustomersName.DataSource = booking.GetCustomer().ToList();
                        cboCustomersName.DisplayMember = "Name";
                        txtCustomersFName.Clear();
                        txtCustomersLName.Clear();

                        cboRentCustomerName.DataSource = booking.GetCustomer().ToList();
                        cboRentCustomerName.DisplayMember = "Name";

                        MessageBox.Show("Successfully Deleted.");
                    }
                    else
                    {
                        MessageBox.Show("Customer deletion not succesfull.");
                    }
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: {0} is not deleted succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }
            }
        }

        private void btnCustomersUpdate_Click(object sender, EventArgs e)
        {
            string customerName = txtCustomersFName.Text + txtCustomersLName;

            if (string.IsNullOrWhiteSpace(txtCustomersFName.Text) || customerName.Any(Char.IsDigit))
            {
                MessageBox.Show("Please enter valid Customer name.", "Error Message");
                txtCustomersFName.Clear();
                txtCustomersLName.Clear();
                txtCustomersFName.Focus();
            }
            else
            {
                try
                {
                    var booking = new Booking(DbManager);
                    var result = booking.UpdateCustomer(((Customer)cboCustomersName.SelectedItem).Id, 
                        new Customer(){FirstName = txtCustomersFName.Text, LastName = txtCustomersLName.Text});
                    if (result)
                    {
                        cboCustomersName.DataSource = booking.GetCustomer().ToList();
                        cboCustomersName.DisplayMember = "Name";
                        txtCustomersFName.Clear();
                        txtCustomersLName.Clear();

                        cboRentCustomerName.DataSource = booking.GetCustomer().ToList();
                        cboRentCustomerName.DisplayMember = "Name";

                        FillBookingList();
                        MessageBox.Show("Successfully Updated.");


                    }
                    else
                    {
                        MessageBox.Show("Update not successfull: Customer already exists.", "Error Message");
                    }
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: Customar {0} is not updated succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }

            }
        }

        private void cboCustomersName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: You forgot to add exception handling
            txtCustomersFName.Text = ((Customer)cboCustomersName.SelectedItem).FirstName;
            txtCustomersLName.Text = ((Customer)cboCustomersName.SelectedItem).LastName;
        }

        #endregion


        #region Rent/Return Events

        private void btnRent_Click(object sender, EventArgs e)
        {
            if (lvwVideoStatus.SelectedItems.Count.Equals(0))
            {
                MessageBox.Show("Please select a video from list");
            }
            else
            {
                try
                {
                    var videoId = Guid.Parse(lvwVideoStatus.SelectedItems[0].Text);
                    var customerId = ((Customer)cboRentCustomerName.SelectedItem).Id;

                    var booking = new Booking(DbManager);
                    var result = booking.RentVideo(new VideoBooking()
                        {
                            VideoId = videoId, 
                            CustomerId = customerId, 
                            RentDate = DateTime.Now,
                            ReturnDate = DateTime.Now
                        });

                    FillAvailableVideoList();
                    FillBookingList();
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: Rent {0} is not  completed succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (lvwBookings.SelectedItems.Count.Equals(0))
            {
                MessageBox.Show("Please select a Customer from the list");
            }

            else if ((lvwBookings.SelectedItems[0].SubItems[6].Text) != String.Empty)
            {
                MessageBox.Show("Video already returned ");
            }
            else
            {
                try
                {
                    var bookingId = Guid.Parse(lvwBookings.SelectedItems[0].Text);
                    var returnDate = dtpReturnDate.Value;
                    var booking = new Booking(DbManager);
                    var result = booking.ReturnVideo(bookingId, returnDate);
                    if (result)
                    {
                        FillBookingList();
                        FillAvailableVideoList();
                    }
                    else
                    {
                        MessageBox.Show("Video return not successful.", "Error Message");
                    }
                }
                catch (VideoRentalException ex)
                {
                    MessageBox.Show(String.Format("Error occured: Return {0} is not compelted succesfully", ex.Item.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Occoured:  {0}", ex.Message));
                }
            }
        }

       

        private void FillBookingList()
        {
            var booking = new Booking(DbManager);
            var bookingList = booking.GetBookingListView();
            
            lvwBookings.Items.Clear();
            //lvwVideoStatus.Columns.Add("Id", 0);
            //lvwVideoStatus.Columns.Add("Name", 150);
            //lvwVideoStatus.Columns.Add("Genre", 150);
            //lvwVideoStatus.Columns.Add("Rent Date", 100);
            //lvwVideoStatus.Columns.Add("Return Date", 100);
            //lvwVideoStatus.Columns.Add("Cost", 100);
            lvwBookings.Items.AddRange(bookingList.ToArray());
        }

        private void FillAvailableVideoList()
        {

            var booking = new Booking(DbManager);
            var availableVideo =  booking.GetAvailableVideo();
            lvwVideoStatus.Items.Clear();

            //lvwVideoStatus.Columns.Add("Id", 0);
            //lvwVideoStatus.Columns.Add("Name", 150);
            //lvwVideoStatus.Columns.Add("Genre", 150);
            //lvwVideoStatus.Columns.Add("Rent Days", 100);
            
            lvwVideoStatus.Items.AddRange(availableVideo.ToArray());
        }


        #endregion


        private void btnClose_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

       



    }
}
