using PhoneBook.Models;
using PhoneBook.Repository;
using System;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using PhoneBook.Forms.Context;

namespace PhoneBook
{
    public partial class frmAddOrEdit : Form
    {
        public int ContactID = 0;

        private readonly UnitOfWork _uow = new UnitOfWork();


        public frmAddOrEdit()
        {
            InitializeComponent();
        }

        private void frmAddOrEdit_Load(object sender, EventArgs e)
        {

            if (ContactID == 0)
            {
                this.Text = "افزودن مخاطب";
            }
            else
            {
                this.Text = "ویرایش مخاطب";
                Contact contact = _uow.contactRepository.SelectRow(ContactID);
                txtName.Text = contact.Name;
                txtFamily.Text = contact.Family;
                txtMobile.Text = contact.Mobile;
                txtAddress.Text = contact.Address;

                if (contact.Avatar != null)
                {
                    var stream = new MemoryStream(contact.Avatar);
                    pcContact.Image = Image.FromStream(stream);
                }

                if (contact.BirthDate.HasValue)
                {
                    txtDate.Value = contact.BirthDate;
                }

            }
        }

        bool ValidateInputs()
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("لطفا نام را وارد کنید", "اخطار!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtFamily.Text == "")
            {
                MessageBox.Show("لطفا نام خانوادگی را وارد کنید", "اخطار!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtMobile.Text == "")
            {
                MessageBox.Show("لطفا شماره را وارد کنید", "اخطار!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if (!IsValidMobileNumber(txtMobile.Text))
            {
                MessageBox.Show("لطفا شماره معتبر را وارد کنید", "اخطار!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public bool IsValidMobileNumber(string input)
        {
            const string pattern = @"^09[0|1|2|3][0-9]{8}$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(input);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            if (ValidateInputs())
            {
                string phoneNumber = txtMobile.Text;


                if (ContactID == 0)
                {
                    if (_uow.contactRepository.PhoneNumberExists(phoneNumber))
                    {
                        MessageBox.Show("شماره تکراری میباشد.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }


                if (ContactID == 0)
                {
                    Contact contact = new Contact();
                    contact.Name = txtName.Text;
                    contact.Family = txtFamily.Text;
                    contact.Mobile = txtMobile.Text;
                    contact.Address = txtAddress.Text;

                    if (pcContact.Image != null)
                    {
                        contact.Avatar = ImageToByte(pcContact.Image);
                    }

                    if (txtDate.Value != null)
                    {
                        contact.BirthDate = txtDate.Value;
                    }
                    _uow.contactRepository.Insert(contact);
                }
                else
                {
                    Contact contact = _uow.contactRepository.SelectRow(ContactID);
                    contact.Name = txtName.Text;
                    contact.Family = txtFamily.Text;
                    contact.Mobile = txtMobile.Text;
                    contact.Address = txtAddress.Text;

                    if (pcContact.Image != null)
                    {
                        contact.Avatar = ImageToByte(pcContact.Image);
                    }

                    if (txtDate.Value != null)
                    {
                        contact.BirthDate = txtDate.Value;
                    }

                    _uow.contactRepository.Update(contact);
                }
                _uow.Save();
                MessageBox.Show("عملیات با موفقیت انجام شد!", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;

            }
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "jpg Files |*.jpg|PNG FIles |*.png";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pcContact.ImageLocation = openFile.FileName;
            }
        }

        private void txtMobile_TextChanged(object sender, EventArgs e)
        {
            Contact contact = new Contact();
            txtMobile.MaxLength = 11;

            if (System.Text.RegularExpressions.Regex.IsMatch(txtMobile.Text, "09[^0-9]{9}$"))
            {
                MessageBox.Show(".لطفا فقط عدد وارد کنید");
                txtMobile.Text = txtMobile.Text.Remove(txtMobile.Text.Length - 1);
            }
        }

    }
}
