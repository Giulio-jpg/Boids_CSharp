using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Boid_2022
{
    class Boid
    {
        protected Sprite sprite;
        protected Texture texture;

        protected Vector2 velocity;
        protected float maxSpeed;

        protected float alignHalfAngle = MathHelper.Pi;
        protected float cohesionHalfAngle = MathHelper.Pi;
        protected float separationHalfAngle = MathHelper.DegreesToRadians(150);
        protected float cohesionRadius = 300;
        protected float separationRadius = 70;
        protected float alignRadius = 200;

        private float separationWeight = 5;

        public virtual Vector2 Position { get { return sprite.position; } set { sprite.position = value; } }
        protected float X { get { return sprite.position.X; } set { sprite.position.X = value; } }
        protected float Y { get { return sprite.position.Y; } set { sprite.position.Y = value; } }
        

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
            }
            set
            {
                sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }

        public Boid(Vector2 position)
        {
            texture = new Texture("Assets/boid.png");
            sprite = new Sprite(texture.Width, texture.Height);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.7f);

            Position = position;

            Random rand = new Random();
            int randX;
            int randY;

            do
            {
                randX = rand.Next(-100, 101);
                randY = rand.Next(-100, 101); 
            } while (randX == 0 && randY == 0);

            Vector2 dir = new Vector2(randX, randY).Normalized();

            maxSpeed = 150;

            velocity = dir * maxSpeed;

            Forward = velocity;
        }

        public void Update()
        {
            Vector2 alignment = GetAlignment();

            Vector2 cohesion = GetCohesion();

            Vector2 separation = GetSeparation() * separationWeight;

            Vector2 result = alignment + cohesion + separation;

            if (velocity != result)
            {
                velocity = Vector2.Lerp(velocity, result * maxSpeed, Program.DeltaTime * 0.5f);
                velocity = velocity.Normalized() * maxSpeed;
            }

            if (velocity.Length > 0)
            {
                Forward = velocity;
            }

            Position += velocity * Program.DeltaTime;
            CheckLimits();
        }

        private bool IsVisible(Vector2 point, float radius, float halfAngle, out Vector2 distance)
        {
            distance = point - Position;

            if (distance.Length <= radius)
            {
                float angle = (float)Math.Acos(Vector2.Dot(Forward, distance.Normalized()));
                return angle <= halfAngle;
            }

            return false;
        }

        private void CheckLimits()
        {
            if (X - sprite.pivot.X <= 0)
            {
                X = Program.Window.Width - sprite.pivot.X;
            }
            else if (X + sprite.pivot.X >= Program.Window.Width)
            {
                X = 0 + sprite.pivot.X;
            }


            if (Y - sprite.pivot.Y <= 0)
            {
                Y = Program.Window.Height - sprite.pivot.Y;
            }
            else if (Y + sprite.pivot.Y >= Program.Window.Height)
            {
                Y = 0 + sprite.pivot.Y;
            }

        }



        public Vector2 GetAlignment()
        {
            Vector2 alignment = Vector2.Zero;
            int neighboursCount = 0;
            
            for (int i = 0; i < Program.Boids.Count; i++)
            {
                Boid b = Program.Boids[i];
                Vector2 distance;

                if (b != this && IsVisible(b.Position, alignRadius, alignHalfAngle, out distance))
                {
                    alignment += b.Forward;
                    neighboursCount++;
                }
                
            }

            if (neighboursCount > 0)
            {
                alignment /= neighboursCount;
                if (alignment.Length != 0)
                {
                    alignment.Normalize();
                }
            }

            return alignment;
        }

        public Vector2 GetCohesion()
        {
            Vector2 cohesion = Vector2.Zero;
            int neighboursCount = 0;

            for (int i = 0; i < Program.Boids.Count; i++)
            {
                Boid b = Program.Boids[i];
                Vector2 distance;

                if (b != this && IsVisible(b.Position, cohesionRadius, cohesionHalfAngle, out distance))
                {
                    cohesion += b.Position;
                    neighboursCount++;
                }

            }

            if (neighboursCount > 0)
            {
                cohesion /= neighboursCount;

                cohesion = cohesion - Position;

                if (cohesion.Length != 0)
                {
                    cohesion.Normalize();
                }
            }

            return cohesion;
        }


        public Vector2 GetSeparation()
        {
            Vector2 separation = Vector2.Zero;
            int neighboursCount = 0;

            for (int i = 0; i < Program.Boids.Count; i++)
            {
                Boid b = Program.Boids[i];
                Vector2 distance;

                if (b != this && IsVisible(b.Position, separationRadius, separationHalfAngle, out distance))
                {
                    float weight = 1 - distance.Length / separationRadius;

                    separation += distance * weight;

                    neighboursCount++;
                }

            }

            if (neighboursCount > 0)
            {
                separation /= neighboursCount;

                separation = -separation;

                if (separation.Length != 0)
                {
                    separation.Normalize();
                }
            }

            return separation;
        }

        public void Draw()
        {
            sprite.DrawTexture(texture);
        }

    }
}
