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

        enum Direccion { Arriba, Abajo, Izquierda, Derecha, Ninguna};
        Direccion direccionJugador = Direccion.Ninguna;

        double velocidadMega = 100;
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
        void moverJugador(TimeSpan deltaTime)
        {
            double topMegaActual = Canvas.GetTop(imgMega);
            double leftMegaActual = Canvas.GetLeft(imgMega);
            switch (direccionJugador)
            {
                case Direccion.Arriba:

                        Canvas.SetTop(imgMega, topMegaActual - (velocidadMega * deltaTime.TotalSeconds));
                    break;
                case Direccion.Abajo:
                        Canvas.SetTop(imgMega, topMegaActual + (velocidadMega * deltaTime.TotalSeconds));
                    break;
                case Direccion.Izquierda:
                    if (leftMegaActual - (velocidadMega * deltaTime.TotalSeconds) >= 0)
                    {
                        Canvas.SetLeft(imgMega, leftMegaActual - (velocidadMega * deltaTime.TotalSeconds));
                    }

                    break;
                case Direccion.Derecha:
                    double nuevaPosicion = leftMegaActual + (velocidadMega * deltaTime.TotalSeconds);
                    if (nuevaPosicion + imgMega.Width <= 800)
                    {
                        Canvas.SetLeft(imgMega, leftMegaActual + (velocidadMega * deltaTime.TotalSeconds));
                    }
                        
                    break;
                case Direccion.Ninguna:
                    break;
                default:
                    break;
            }

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
                        //velocidadMega += 10 * deltaTime.TotalSeconds;
                        if (estadoActual == EstadoJuego.Gameplay)
                        {
                            moverJugador(deltaTime);
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
                    direccionJugador = Direccion.Arriba;
                }
                if (e.Key == Key.Down)
                {
                    direccionJugador = Direccion.Abajo;
                }
                if (e.Key == Key.Left)
                {
                    direccionJugador = Direccion.Izquierda;
                }
                if (e.Key == Key.Right)
                {
                    direccionJugador = Direccion.Derecha;
                }
            }
        }

        private void miCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Up && direccionJugador == Direccion.Arriba)
            {
                direccionJugador = Direccion.Ninguna;
            }
            if (e.Key==Key.Down && direccionJugador == Direccion.Abajo)
            {
                direccionJugador = Direccion.Ninguna;
            }
            if (e.Key==Key.Left && direccionJugador == Direccion.Izquierda)
            {
                direccionJugador = Direccion.Ninguna;
            }
            if (e.Key==Key.Right && direccionJugador == Direccion.Derecha)
            {
                direccionJugador = Direccion.Ninguna;
            }
        }
    }
}