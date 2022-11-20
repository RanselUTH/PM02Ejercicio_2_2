using System;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PM02Eje2_2.Models;


namespace PM02Ejercicio_2_2
{
    public partial class MainPage : ContentPage
    {
        byte[] ImageBytes;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void btnagregar_Clicked(object sender, EventArgs e)
        {
            Stream Firma = await firmapadview.GetImageStreamAsync(SignatureImageFormat.Png);
            try
            {
                //obtenemos la firma
                var firma = await firmapadview.GetImageStreamAsync(SignatureImageFormat.Png);

   
                //Pasamos la firma a imagen a base 64
                var fSream = (MemoryStream)firma;
                byte[] data = fSream.ToArray();
                string base64Value = Convert.ToBase64String(data);
                ImageBytes = Convert.FromBase64String(base64Value);


            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Firma vacia, Porfavor Agregar su firma", "Ok");
            }

            //seteamos los valores
            var firmas = new Firmas
            {
                Nombre = nombre_input.Text,
                Descripcion = descripcion_input.Text,
                Imagen = ImageBytes
            };


            //Aviso de los campos vacios.............
            if (String.IsNullOrEmpty(nombre_input.Text) || String.IsNullOrEmpty(descripcion_input.Text))
            {
                await DisplayAlert("Aviso", "¡¡Todos los campos deben ser llenados!!", "Entendido");
            }
           
           else
            {
                await DisplayAlert("Aviso", "Firma Agregada Con Exito 👌!!!", "Listo");

                //Guardar firma en la base de datos
                await App.BaseDatos.GuardarFirmas(firmas);

                //Guardamos localmente en el dispositivo
                SaveToDevice(Firma);

                //limpiar datos
                firmapadview.Clear();
                nombre_input.Text = "";
                descripcion_input.Text = "";

            }

          
        }


        //Ir la pagina de lista de firmas: Navegar a la pagina de lista de firmas
        private async void btnlistview_firmas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Vistas.ListViewFirma());
            
        }


        //funcíon de guardar firma en el dispositivo
        private void SaveToDevice(Stream img)
        {
            try
            {
                //var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString(), "Firmas");
                var filename = "/storage/emulated/0/Android/data/com.companyname.pm02ejercicio_2_2/files/Pictures/MisFotos/";

                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }

                string nameFile = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                filename = Path.Combine(filename, nameFile);

                var mStream = (MemoryStream)img;
                Byte[] bytes = mStream.ToArray();
                File.WriteAllBytes(filename, bytes);

                DisplayAlert("Notificación Ruta Guardada", "Firma guardada en la ruta: " + filename, "Ok 👌");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }

        }


    }
}
