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
    public partial class Form1 : Form
    {
        private readonly HttpClient httpClient = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }
        private async void btnBuscar_Click_1(object sender, EventArgs e)
        {
            pictureBoxPokemon.BackgroundImage = null;   //que desaparezca el fondo
            string nombrePokemon = txtNombrePokemon.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(nombrePokemon))
            {
                MessageBox.Show("Por favor, ingresa un nombre de Pokémon.");
                return;
            }
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon/{nombrePokemon}";
                string response = await httpClient.GetStringAsync(url);

                JObject pokemon = JObject.Parse(response);

                string nombre = pokemon["name"]?.ToString();
                string imagenUrl = pokemon["sprites"]["front_default"]?.ToString();
                var tipos = pokemon["types"];
                var stats = pokemon["stats"];

                lblNombre.Text = $"Nombre: {nombre.ToUpper()}";

                // Tipos
                string tiposTexto = "Tipos: ";
                foreach (var tipo in tipos)
                {
                    tiposTexto += tipo["type"]["name"] + " ";
                }
                lblTipos.Text = tiposTexto;

                // Estadísticas
                string statsTexto = "Estadísticas:\n";
                foreach (var stat in stats)
                {
                    statsTexto += $"{stat["stat"]["name"]}: {stat["base_stat"]}\n";
                }
                lblStats.Text = statsTexto;

                pictureBoxPokemon.Load(imagenUrl);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar con la API. Verifica tu conexión.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            txtNombrePokemon.Clear();
            lblNombre.Text = "";
            lblTipos.Text = "";
            lblStats.Text = "";
            pictureBoxPokemon.Image = null;
        }

        private void txtNombrePokemon_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClima_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string contenido = $"Pokémon:\nNombre: {lblNombre.Text}\nTipos: {lblTipos.Text}\nEstadísticas:\n{lblStats.Text}";

                SaveFileDialog guardar = new SaveFileDialog
                {
                    Filter = "Archivo de texto (*.txt)|*.txt|Archivo JSON (*.json)|*.json",
                    Title = "Guardar Información del Pokémon"
                };

                if (guardar.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(guardar.FileName, contenido);
                    MessageBox.Show("Información del Pokémon guardada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar archivo: " + ex.Message);
            }
        }
    }
}
