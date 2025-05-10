using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace APIs_Carlo
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private static readonly HttpClient httpClient = new HttpClient();
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "https://api.thecatapi.com/v1/images/search";
                string response = await httpClient.GetStringAsync(url);

                JArray resultado = JArray.Parse(response);
                string imagenUrl = resultado[0]["url"].ToString();

                pictureBoxGato.Load(imagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen del gato: " + ex.Message);
            }
        }

        private void btnPokemon_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string urlImagen = pictureBoxGato.ImageLocation ?? "No disponible";

                string contenido = $"Imagen de gato mostrada:\n{urlImagen}";

                SaveFileDialog guardar = new SaveFileDialog
                {
                    Filter = "Archivo de texto (*.txt)|*.txt|Archivo JSON (*.json)|*.json",
                    Title = "Guardar Información del Gato"
                };

                if (guardar.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(guardar.FileName, contenido);
                    MessageBox.Show("Información del gato guardada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar archivo: " + ex.Message);
            }
        }
    }
}
