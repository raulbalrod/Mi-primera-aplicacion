using System.Management;
using System.Net;
using System.Reflection;

namespace Mi_primera_aplicación
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salida.Text = string.Empty;

            ObtenerUsuarioEquipo();
            
            ObtenerUnidades();
            
            ObtenerIPs();

            ComprobarPCEnCarga();

            ObtenerGestionMemoriaRAM();
        }

        private void ObtenerUsuarioEquipo() 
        {
            string usuario = SystemInformation.UserName;
            string dominio = SystemInformation.UserDomainName;

            salida.Text = "Usuario: " + usuario + Environment.NewLine + "Dominio o equipo: " + dominio;
        }

        private void ObtenerUnidades() 
        {
            DriveInfo[] drives = DriveInfo
                .GetDrives()
                .Where(a => a.DriveType == DriveType.Fixed)
                .ToArray();

            foreach (DriveInfo drive in drives) 
            {
                double espacioLibre = drive.TotalFreeSpace;
                double espacioTotal = drive.TotalSize;

                double espacioLibrePorcentaje = (espacioLibre/ espacioTotal) * 100;
                salida.Text += Environment.NewLine + drive.Name + ": " + espacioLibrePorcentaje + "%" + Environment.NewLine;  
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ObtenerIPs()
        {
            IPAddress[] direcciones = 
                Dns.GetHostAddresses(Dns.GetHostName())
                    .Where(a => !a.IsIPv6LinkLocal).ToArray();

            foreach(IPAddress direccion in direcciones)
            {
                salida.Text += Environment.NewLine + "Ip: " + direccion.Address.ToString();
            }
        }

        private void ComprobarPCEnCarga()
        {
            Type pw = typeof(PowerStatus);

            PropertyInfo[] propiedades = pw.GetProperties();

            object? valor = propiedades[0].GetValue(SystemInformation.PowerStatus, null);

            salida.Text += Environment.NewLine + valor.ToString();
        }

        private void ObtenerGestionMemoriaRAM()
        {
            ObjectQuery objectQuery = new("SELECT * FROM Win32_OperatingSystem");

            ManagementObjectSearcher managementObject = new(objectQuery);

            ManagementObjectCollection collection = managementObject.Get();

            foreach(ManagementObject elemento in collection)
            {
                decimal memoriaTotal =
                    Math.Round(Convert.ToDecimal(elemento["TotalVisibleMemorySize"]) / (1024 * 1024), 2); 

                decimal memoriaLibre =
                     Math.Round(Convert.ToDecimal(elemento["FreePhysicalMemory"]) / (1024 * 1024), 2);

                salida.Text += Environment.NewLine + "Memoria Total: " + memoriaTotal + "GB";

                salida.Text += Environment.NewLine + "Memoria Libre: " + memoriaLibre + "GB";
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}