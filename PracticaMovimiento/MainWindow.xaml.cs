using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//Librerias para multiprocesamiento
using System.Threading;
using System.Diagnostics;

namespace PracticaMovimiento
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;
        public MainWindow()
        {
            InitializeComponent();
            miCanvas.Focus();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;
            //1.-Establecer instrucciones
            ThreadStart threadStart = new ThreadStart(moverEnemigos);
            //2.- Inicializar Thread
            Thread threadMoverEnemigos = new Thread(threadStart);
            //2.-Ejecutar el Thread
            threadMoverEnemigos.Start();
        }
        void moverEnemigos()
        {
            while (true)
            {
                Dispatcher.Invoke(
                    () =>
                    {
                        var tiempoActual = stopwatch.Elapsed;
                        var deltaTime = tiempoActual - tiempoAnterior;
                        double leftEnemigoActual = Canvas.GetLeft(imgEnemigo);
                        Canvas.SetLeft(imgEnemigo, leftEnemigoActual - (100 * deltaTime.TotalSeconds) );
                        if (Canvas.GetLeft(imgEnemigo) <=-100)
                        {
                            Canvas.SetLeft(imgEnemigo, 800);
                        }
                        tiempoAnterior = tiempoActual;
                    }
                    );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Up)
            {
                double topMegaActual = Canvas.GetTop(imgMega);
                Canvas.SetTop(imgMega, topMegaActual - 15);
            }
            if (e.Key == Key.Down)
            {
                double topMegaActual = Canvas.GetTop(imgMega);
                Canvas.SetTop(imgMega, topMegaActual + 15);
            }
            if (e.Key == Key.Left)
            {
                double topMegaActual = Canvas.GetLeft(imgMega);
                Canvas.SetLeft(imgMega, topMegaActual - 15);
            }
            if (e.Key == Key.Right)
            {
                double topMegaActual = Canvas.GetLeft(imgMega);
                Canvas.SetLeft(imgMega, topMegaActual + 15);
            }
        }
    }
}
