![Logo of the project]()

# Softfire Animation 2D Library - Controls
> Filename: Softfire.MonoGame.ANIM.dll  
> Requires MonoGame 3.6+  
> Source: [MonoGame](http://www.monogame.net)

This section contains methods integrated into the library that can be used to move Animations.

## Table of Contents

- [README](README.md)

## Basics

#### Animation.Position
Type: `Vector2`  
Default: `X: 0, Y: 0`

Simply add a new Vector2 to the current Position to move the Animation.

Example:
```C#
Animation.Position += new Vector2(X, Y);
```

#### Animation.Acceleration

Acceleration is used to calculate Velocity and will be applied using ApplyVelocity().

#### Animation.Accelerate(double increment)
> Use to increase the rate of acceleration.

Apply a rate of acceleration to the Accelerate method.  
Calculate Velocity.  
Apply Velocity.

Example:
```C#
Animation.Accelerate(1.0);
Animation.CalculateVelocity();
Animation.ApplyVelocity();
```

#### Animation.Accelerate(double increment, double limit)
> Use to increase the rate of acceleration; up to the assigned limit.  
> Limit should be a positive number.

Apply a rate of acceleration to the Accelerate method and apply a limit.  
Calculate Velocity.  
Apply Velocity.

Example:
```C#
Animation.Accelerate(1.0, 10.0);
Animation.CalculateVelocity();
Animation.ApplyVelocity();
```

#### Animation.Decelerate(double decrement)
> Use to decrease the rate of acceleration.

Apply a rate of acceleration to the Decelerate method.  
Calculate Velocity.  
Apply Velocity.

Example:
```C#
Animation.Decelerate(1.0);
Animation.CalculateVelocity();
Animation.ApplyVelocity();
```

#### Animation.Decelerate(double increment, double limit)
> Use to decrease the rate of acceleration; down to the assigned limit.  
> Limit should be a positive number.

Apply a rate of deceleration to the Decelerate method and apply a limit.  
Calculate Velocity.  
Apply Velocity.

Example:
```C#
Animation.Decelerate(1.0, 10.0);
Animation.CalculateVelocity();
Animation.ApplyVelocity();
```

#### Animation.CalculateVelocity()

Use to calculate the velocity of the Animation based on it's current RotationAngle and Acceleration.  
Call ApplyVelocity() to finalize the operation and apply the calculations to Position.

#### Animation.CalculateVelocity(double angle)

Use to calculate the velocity of the Animation based on the supplied angle, in Degrees, and Acceleration.  
Call ApplyVelocity() to finalize the operation and apply the calculations to Position.

## Document Informaton

Version: `1.0`  
Author: `Softfire`  
Updated: `Sept. 5, 2017` 