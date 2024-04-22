using PhoneBook.Models;
using PhoneBook.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using PhoneBook.Utility;

namespace PhoneBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void BindGrid()
        {
                dgContacts.AutoGenerateColumns = false;
                dgContacts.Columns[0].Visible = false;
                IEnumerable<Contact> list;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    list = uow.contactRepository.SelectAll();
                }

                List<ContactVM> newList = new List<ContactVM>();
                PersianCalendar pc = new PersianCalendar();

                foreach(var item in list)
                {
                    var obj = new ContactVM()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Avatar = item.Avatar,
                        Family = item.Family,
                        Mobile = item.Mobile,
                        Address = item.Address
                    };

                    if(item.BirthDate != null)
                    {
                        obj.BirthDate = string.Format("{0:D4}/{1:D2}/{2:D2}", pc.GetYear(item.BirthDate.Value), pc.GetMonth(item.BirthDate.Value), pc.GetDayOfMonth(item.BirthDate.Value));

                    }

                    newList.Add(obj);
                }

                dgContacts.DataSource = newList;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgContacts.CurrentRow != null)
            {
                int ContactID = int.Parse(dgContacts.CurrentRow.Cells[0].Value.ToString());
                frmAddOrEdit frm = new frmAddOrEdit();
                frm.ContactID = ContactID;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgContacts.CurrentRow != null)
            {
                if (MessageBox.Show(" آیا از حذف مخاطب مطمئن هستید؟ ", "اخطار", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    int ID = int.Parse(dgContacts.CurrentRow.Cells[0].Value.ToString());
                    using(UnitOfWork uow = new UnitOfWork())
                        
                    {
                        var contact = uow.contactRepository.SelectRow(ID);
                        uow.contactRepository.Delete(contact);
                        uow.Save();
                    }

                }

                BindGrid();
            }
            else
            {
                MessageBox.Show("لطفا یک مخاطب را انتخاب کنید");
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
            
        }
        private void btnNewContact_Click_(object sender, EventArgs e)
        {
            frmAddOrEdit frm = new frmAddOrEdit();
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                BindGrid();
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<Contact> list;

            using (UnitOfWork uow = new UnitOfWork())
            {
                list = uow.contactRepository.SelectAll();
            }

            dgContacts.DataSource = list.Where(c=> c.Name.Contains(txtSearch.Text) || c.Family.Contains(txtSearch.Text)).ToList();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<Contact> result = db.contactRepository.SelectAll().ToList();

                DateTime? startDate;
                DateTime? endDate;

                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.BirthDate >= startDate.Value).ToList();
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.BirthDate <= endDate.Value).ToList();
                }

                dgContacts.AutoGenerateColumns = false;

                //dgContacts.Rows.Clear();
                dgContacts.DataSource = null;

                foreach (var contact in result)
                {
                    dgContacts.Rows.Add(contact.ID, contact.Name, contact.Family, contact.Mobile,contact.Address, contact.BirthDate?.ToShamsi());
                }

            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();

            string name = txtSearch.Text.Trim();
            string mobile = txtSearchN.Text.Trim();
            DateTime? fromDate = null;
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && txtFromDate.Text != "    /  /")
            {
                fromDate = Convert.ToDateTime(txtFromDate.Text);
                fromDate = DateConvertor.ToMiladi(fromDate.Value);
            }

            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(txtToDate.Text) && txtToDate.Text != "    /  /")
            {
                toDate = Convert.ToDateTime(txtToDate.Text);
                toDate = DateConvertor.ToMiladi(toDate.Value);
            }

            IEnumerable<Contact> filteredContacts;
            using (UnitOfWork uow = new UnitOfWork())
            {
                filteredContacts = uow.contactRepository.SearchContacts(name, mobile, fromDate, toDate);
            }

            List<ContactVM> filteredContactVMs = filteredContacts.Select(c => new ContactVM
            {
                ID = c.ID,
                Name = c.Name,
                Family = c.Family,
                Mobile = c.Mobile,
                BirthDate = c.BirthDate?.ToShamsi() 
            }).ToList();

            dgContacts.DataSource = filteredContactVMs;
        }

        private void txtSearchN_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<Contact> list;

            using (UnitOfWork uow = new UnitOfWork())
            {
                list = uow.contactRepository.SelectAll();
            }

            dgContacts.DataSource = list.Where(c => c.Mobile.Contains(txtSearchN.Text)).ToList();

            txtSearchN.MaxLength = 11;

            if (System.Text.RegularExpressions.Regex.IsMatch(txtSearchN.Text, "[^0-9]"))
            {
                MessageBox.Show(".لطفا فقط عدد وارد کنید");
                txtSearchN.Text = txtSearchN.Text.Remove(txtSearchN.Text.Length - 1);
            }
        }

    }
}