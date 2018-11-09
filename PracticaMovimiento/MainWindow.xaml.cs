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
        enum EstadoJuego { Gameplay, Gameover};
        EstadoJuego estadoActual = EstadoJuego.Gameplay;
        public MainWindow()
        {
            InitializeComponent();
            miCanvas.Focus();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;
            //1.-Establecer instrucciones
            ThreadStart threadStart = new ThreadStart(actualizar);
            //2.- Inicializar Thread
            Thread threadMoverEnemigos = new Thread(threadStart);
            //2.-Ejecutar el Thread
            threadMoverEnemigos.Start();
        }
        void actualizar()
        {
            while (true)
            {

                Dispatcher.Invoke(
                    () =>
                    {
                        var tiempoActual = stopwatch.Elapsed;
                        var deltaTime = tiempoActual - tiempoAnterior;
                        if (estadoActual == EstadoJuego.Gameplay)
                        {
                                double leftEnemigoActual = Canvas.GetLeft(imgEnemigo);
                                Canvas.SetLeft(imgEnemigo, leftEnemigoActual - (150 * deltaTime.TotalSeconds) );
                                if (Canvas.GetLeft(imgEnemigo) <=-100)
                                {
                                    Canvas.SetLeft(imgEnemigo, 800);
                                }
                                //Interseccion en X
                                double xEnemigo = Canvas.GetLeft(imgEnemigo);
                                double xMega = Canvas.GetLeft(imgMega);
                                if (xMega+imgMega.Width>=xEnemigo && xMega<=xEnemigo+imgEnemigo.Width)
                                {
                                    lblInterseccionX.Text = "Si hay interseccion en X!!!11!!uno";
                                }
                                else
                                {
                                    lblInterseccionX.Text = "No hay interseccion en X";
                                }
                                double yEnemigo = Canvas.GetTop(imgEnemigo);
                                double yMega = Canvas.GetTop(imgMega);
                                if (yMega+imgMega.Height>=yEnemigo && yMega<=yEnemigo+imgEnemigo.Height)
                                {
                                    lblInterseccionY.Text = "Si hay interseccion en Y!!!11!!uno";
                                }
                                else
                                {
                                    lblInterseccionY.Text = "No hay interseccion en Y";
                                }
                                if (xMega + imgMega.Width >= xEnemigo && xMega <= xEnemigo + imgEnemigo.Width &&
                                yMega + imgMega.Height >= yEnemigo && yMega <= yEnemigo + imgEnemigo.Height)
                                {
                                    lblColicion.Text = "Hay colision";
                                    estadoActual = EstadoJuego.Gameover;
                                miCanvas.Visibility = Visibility.Collapsed;
                                canvasGameOver.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    lblColicion.Text = "No hay colision";
                                }
                        }
                        else if (estadoActual == EstadoJuego.Gameover)
                        {

                        }

                        
                        tiempoAnterior = tiempoActual;
                    }
                    );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (estadoActual == EstadoJuego.Gameplay) { 
                if (e.Key == Key.Up)
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
}
