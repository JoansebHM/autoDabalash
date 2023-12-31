using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace autoDabalash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("Nombre", "Nombre");
            dataGridView1.Columns.Add("Cedula", "Cedula");
            dataGridView1.Columns.Add("Celular", "Celular");
            dataGridView1.Columns.Add("Direccion", "Direccion");
            dataGridView1.Columns.Add("Barrio", "Barrio");
            dataGridView1.Columns.Add("Ciudad", "Ciudad");
            dataGridView1.Columns.Add("Correo", "Correo");

        }

        static bool ContieneSoloNumeros(string cadena)
        {
            cadena = cadena.Trim();
            return cadena.All(char.IsDigit);
        }

        static string NormalizarCiudad(string ciudad)
        {
            ciudad = ciudad.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < ciudad.Length; i++)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ciudad[i]);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(ciudad[i]);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }


        static bool buscarCiudad(string ciudadInput, List<string> ciudades)
        {
            ciudadInput = ciudadInput.Trim();
            foreach (string ciudad in ciudades)
            {
                if (NormalizarCiudad(ciudadInput) == NormalizarCiudad(ciudad))
                {
                    return true;
                }
            }

            return false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> ciudades = new List<string>
            {
                "Bogotá",
                "jamundi",
                "Medellín",
                "Cali",
                "Barranquilla",
                "Cartagena",
                "Cúcuta",
                "Soledad",
                "Ibagué",
                "Bucaramanga",
                "Soacha",
                "Santa Marta",
                "Villavicencio",
                "Valledupar",
                "Montería",
                "Pasto",
                "Manizales",
                "Neiva",
                "Pereira",
                "Quibdó",
                "Buenaventura",
                "Riohacha",
                "Popayán",
                "Armenia",
                "Sincelejo",
                "Palmira",
                "Florencia",
                "Yopal",
                "Tunja",
                "Maicao",
                "Girardot",
                "Sogamoso",
                "Mocoa",
                "San Andrés",
                "Turbo",
                "Apartadó",
                "Fusagasugá",
                "Ciénaga",
                "Duitama",
                "Tuluá",
                "Magangué",
                "Girón",
                "Chía",
                "Zipaquirá",
                "La Ceja",
                "Bello",
                "Itagüí"
            };

            List<string> departamentos = new List<string>
            {
                "Amazonas",
                "Antioquia",
                "Arauca",
                "Atlántico",
                "Bolívar",
                "Boyacá",
                "Caldas",
                "Caquetá",
                "Casanare",
                "Cauca",
                "Cesar",
                "Chocó",
                "Córdoba",
                "Cundinamarca",
                "Guainía",
                "Guaviare",
                "Huila",
                "La Guajira",
                "Magdalena",
                "Meta",
                "Nariño",
                "Norte de Santander",
                "Putumayo",
                "Quindío",
                "Risaralda",
                "San Andrés y Providencia",
                "Santander",
                "Sucre",
                "Tolima",
                "Valle del Cauca",
                "Vaupés",
                "Vichada"
            };
            string[] lineas = richTextBox1.Lines;
            string nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lineas[0].ToLower()), correo = "", cedula = "", celular = "", ciudad = "", direccion = "", barrio = "";
            if (lineas.Length > 0)
            {
                string[] nuevasLineas = new string[lineas.Length - 1];
                Array.Copy(lineas, 1, nuevasLineas, 0, lineas.Length - 1);
                lineas = nuevasLineas;
            }

            foreach (string line in lineas)
            {
                line.Trim().ToLower();
                Console.Write(line);

                if ((line.Length == 10 && line[0] == '1') || (ContieneSoloNumeros(line) && line.Length != 10))
                {
                    cedula = line;
                }
                else if (line.Contains("@"))
                {
                    correo = line;
                }
                else if (line.Length == 10 && line[0] == '3')
                {
                    celular = line;
                }
                else if (buscarCiudad(line, ciudades))
                {
                    ciudad = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(line.ToLower()); ;
                }
                else if (line.Contains("#") || line.Contains("cl") ||
                    line.Contains("no") || line.Contains("cra") ||
                    line.Contains("carrera") || line.Contains("calle") ||
                    line.Contains("cr") || line.Contains("no.") || line.Contains("condominio") ||
                    line.Contains("apto") || line.Contains("casa") || line.Contains("torre") ||
                    line.Contains("avenida") || line.Contains("trasversal") || line.Contains("diagonal"))
                {
                    direccion += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(line.ToLower()); ;
                }
                else
                {
                    if (!buscarCiudad(line, departamentos))
                    {
                        barrio = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(line.ToLower());
                    }
                }
            }

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1);
            row.Cells[0].Value = nombre;
            row.Cells[1].Value = cedula;
            row.Cells[2].Value = celular;
            row.Cells[3].Value = direccion;
            row.Cells[4].Value = barrio;
            row.Cells[5].Value = ciudad;
            row.Cells[6].Value = correo;

            dataGridView1.Rows.Add(row);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        public string EliminarTildes(string textoConTildes)
        {
            string conTildes = "áéíóúÁÉÍÓÚ";
            string sinTildes = "aeiouAEIOU";

            StringBuilder sb = new StringBuilder();
            foreach (char c in textoConTildes)
            {
                int pos = conTildes.IndexOf(c);
                if (pos > -1)
                    sb.Append(sinTildes[pos]);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string textoSinTildes = EliminarTildes(richTextBox1.Text).Trim().ToLower();
            richTextBox1.Text = textoSinTildes;

            string[] lineas = richTextBox1.Lines;
            string[] palabrasClave = { "barrio", "completo", "completa", "cedula", "celular", "ciudad", "electronico", "telefono", "direccion", "correo", "tel",
            "cc", "c.c", "cel", "dir"};

            char[] caracteresAEliminar = { ':', ',', '.', ';' };

            for (int i = 0; i < lineas.Length; i++)
            {
                foreach (string palabraClave in palabrasClave)
                {
                    int posicion = lineas[i].IndexOf(palabraClave);
                    if (posicion != -1)
                    {
                        lineas[i] = lineas[i].Substring(posicion + palabraClave.Length).TrimStart(caracteresAEliminar).Trim();
                        break;
                    }
                }
                foreach (char caracter in caracteresAEliminar)
                {
                    lineas[i] = lineas[i].Replace(caracter.ToString(), string.Empty);
                }
            }

            richTextBox1.Lines = lineas;

        }
    }
}