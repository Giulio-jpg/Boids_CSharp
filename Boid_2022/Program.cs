using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boid_2022
{
    class Program
    {
        public static Window Window;
        public static float DeltaTime { get { return Window.DeltaTime; } }
               
        public static List<Boid> Boids;
              
        public static bool IsMousePressed;
        
        static void Main(string[] args)
        {
            Window = new Window(1280, 720, "Boids");
            Window.SetVSync(false);
            Window.Position = new Vector2(40, 40);

            IsMousePressed = false;

            Boids = new List<Boid>();



            while (Window.IsOpened)
            {
                if (Window.MouseLeft)
                {
                    if (!IsMousePressed)
                    {
                        IsMousePressed = true;
                        Boids.Add(new Boid(Window.MousePosition)); 
                    }
                }
                else if (IsMousePressed)
                {
                    IsMousePressed = false;
                }


                for (int i = 0; i < Boids.Count; i++)
                {
                    Boids[i].Update();
                }

                for (int i = 0; i < Boids.Count; i++)
                {
                    Boids[i].Draw();
                }


                Window.Update();
            }


        }

    }
}

