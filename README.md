# About
 
This project was done during A.I.V. first year to learn how to implement an artificial intelligence program.

[Boids](https://en.wikipedia.org/wiki/Boids) is an artificial life program, developed by Craig Reynolds in 1986, which simulates the flocking behaviour of birds.

In the following years I also found this algorithm useful (if properly modified) for managing the movement of groups of enemies and preventing them from getting stuck together.

Once launched you can left-click the screen, which will spawn a triangle (a boid) that will start moving in a random direction, infleunced by the other boids that will be spawned:

https://github.com/user-attachments/assets/4a9376ce-922f-48dd-a28e-52cd29ff9953

The movement of boids is defined by three components:

* Separation: steer to avoid crowding neighboring boids

* Alignment: steer towards the average heading of neighboring boids (the average forward of neighboring boids)

* Cohesion: steer to move towards the average position (or center) of neighboring boids

The range within which we want to consider neighboring boids can also be variable for each of these three components (as I did in this project).

Depending on the movement we want, we can give a different weight to each of the components (in this case I gave more weight to the separation).

After calculating the three vectors I will then have to sum them, do other operations if I necessary (in my case I used a lerp on the velocity to have a little smoother motion), and finally normalize the vector:

``` c   
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
```

The graphics part was handled using the library [aiv.fast2d](https://github.com/aiv01/aiv-fast2d).















































