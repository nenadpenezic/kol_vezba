--------------------------PRESENTATION LAYER-----------------------------
using BusinessLayerVet;
using DataAcessLayerVet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayerVet
{
    public partial class Form1 : Form
    {
        private readonly BusinessVet businessVet;

        public Form1()
        {
            this.businessVet = new BusinessVet();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        public void RefreshData()
        {
            List<Vet> vets = this.businessVet.GetAllVets();
            listBoxVetList.Items.Clear();

            foreach(Vet v in vets)
            {
                listBoxVetList.Items.Add(v);
            }


        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Vet v = new Vet();
            v.FullName = textBoxFullName.Text;
            v.Specialty = textBoxSpecialty.Text;
            v.YearsExperience = Convert.ToInt32(textBoxYE.Text);

            if (this.businessVet.InsertVet(v))
            {
                RefreshData();
                textBoxFullName.Text = "";
                textBoxSpecialty.Text = "";
                textBoxYE.Text = "";



            }
            else
                MessageBox.Show("Greska!");
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string vitem = listBoxVetList.SelectedItem.ToString();
            var temp = Convert.ToInt32(vitem.Split('.')[0]);

            Vet v = new Vet();
            v.FullName = textBoxFullName.Text;
            v.Specialty = textBoxSpecialty.Text;
            v.YearsExperience = Convert.ToInt32(textBoxYE.Text);

            if (this.businessVet.UpdateVet(v, temp))
            {
                RefreshData();
                textBoxFullName.Text = "";
                textBoxSpecialty.Text = "";
                textBoxYE.Text = "";

                MessageBox.Show("Uspesno je izmenjen veterinar!");
            }
            else
                MessageBox.Show("Greska pri izmeni!");


        }

        private void buttonCompare_Click(object sender, EventArgs e)

        {
            listBoxCompared.Items.Clear();

            int exp = Convert.ToInt32(textBoxYECompare.Text);
            var vetsCmp = this.businessVet.BiggerExpThanGivenValue(exp);
            if (vetsCmp.Count > 0)
            {
                foreach (Vet vts in vetsCmp)
                    listBoxCompared.Items.Add(vts.ToString());
                    

            }
            else
                MessageBox.Show("Nema takvih veterinara!");

            textBoxYECompare.Text = "";
        }

        private void buttonDeleteVet_Click(object sender, EventArgs e)
        {
            string vitem = listBoxVetList.SelectedItem.ToString();
            var temp = Convert.ToInt32(vitem.Split('.')[0]);

            if (this.businessVet.DeleteVet(temp))
            {
                RefreshData();
                MessageBox.Show("Uspesno je izbrisan veterinar!");
            }
            else
                MessageBox.Show("Greska pri izmeni!");


        }
    }
}

--------------------------------------------BUSINESS LAYER--------------------------------------------------
using DataAcessLayerVet;
using DataAcessLayerVet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerVet
{
    public class BusinessVet
    {
        private readonly VetRepositorium vetRepositorium;

        public BusinessVet()
        {
            this.vetRepositorium = new VetRepositorium();
        }

        public List<Vet> GetAllVets()
        {
            return this.vetRepositorium.GetAllVets();
        }

        public bool InsertVet(Vet v)
        {
            if (this.vetRepositorium.InsertVet(v) > 0)
                return true;

            return false;
        }

        public List<Vet> BiggerExpThanGivenValue(int exp)
        {
            List<Vet> listVets = this.vetRepositorium.GetAllVets();
            List<Vet> newList = new List<Vet>();

            foreach(Vet ve in listVets)
            {
                if (ve.YearsExperience > exp)
                    newList.Add(ve);
            }

            return newList;


        }

        public bool UpdateVet(Vet v, int id)
        {
            if (this.vetRepositorium.UpdateVet(v, id) > 0)
                return true;
            return false;
        }

        public bool DeleteVet(int id)
        {
            if (this.vetRepositorium.DeleteVet(id) > 0)
                return true;
            return false;
        }

    }
}
----------------------------------------DATA ACCESS LAYER-------------------------------------------
using DataAcessLayerVet.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayerVet
{
    public class VetRepositorium
    {
       public List<Vet> GetAllVets()
        {
            List<Vet> listOfVet = new List<Vet>();
            using(SqlConnection dataConnection = new SqlConnection(Constants.connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = dataConnection;
                command.CommandText = "SELECT * FROM  Vets";
                dataConnection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while(dataReader.Read())
                {
                    Vet v = new Vet();
                    v.Id = dataReader.GetInt32(0);
                    v.FullName = dataReader.GetString(1);
                    v.Specialty = dataReader.GetString(2);
                    v.YearsExperience = dataReader.GetInt32(3);
                    listOfVet.Add(v);

                }



            }
            return listOfVet;
        }

        public int InsertVet(Vet v)
        {
            using(SqlConnection dataConnection = new SqlConnection(Constants.connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = dataConnection;
                command.CommandText = string.Format("INSERT Into Vets VALUES ('{0}', '{1}', {2})", v.FullName, v.Specialty, v.YearsExperience);
                dataConnection.Open();

                return command.ExecuteNonQuery();
            }
           
        }

        public int UpdateVet(Vet v, int id)
        {
            using (SqlConnection dataConnection = new SqlConnection(Constants.connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = dataConnection;
                command.CommandText = string.Format("UPDATE Vets SET FullName = '{0}', Specialty = '{1}', YearsExperience = {2}" +
                                                    "WHERE Id = {3}", v.FullName, v.Specialty, v.YearsExperience, id); 
                dataConnection.Open();

                return command.ExecuteNonQuery();
            }

        }
        public int DeleteVet(int idVet)
        {
            using (SqlConnection dataConnection = new SqlConnection(Constants.connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = dataConnection;
                command.CommandText = string.Format("DELETE FROM Vets WHERE Id = {0}", idVet);
                dataConnection.Open();

                return command.ExecuteNonQuery();
            }

        }


    }

}
----------------------------DODATNO--------------------------------

public static DateTime now = DateTime.Now;
bit -> bool isSet = reader.GetBoolean(0);
