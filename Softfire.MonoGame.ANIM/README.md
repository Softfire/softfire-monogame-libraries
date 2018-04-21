![Logo of the project]()

# Softfire Animation 2D Library
> Filename: Softfire.MonoGame.ANIM.dll  
> Requires: .Net 4.7+, MonoGame 3.6+  
> Sources: [.Net](https://www.microsoft.com/en-us/download/details.aspx?id=55170), [MonoGame](http://www.monogame.net)

This library contains C# classes that can be used to animate 2D spritesheets.  
A spritesheet is a single image composed of multiple images arranged in a row/column grid pattern.

## Table of Contents

- [Controls](CONTROLS.md)

## Installing / Getting started

To create an Animation.

1. Create a new Animation variable at the top of your class.

```C#
public Animation Animation { get; set; }
```

2. Instantiate your new Animation variable by calling it's constructor and passing in it's requirements.

```C#
Animation = new Animation(textureFilePath, framesX, framesY, frameSpeed);
```

Requirements:

- texturePath: Intakes a string used to define where the texture should be loaded from. Relative path used in Content.
- framesX: Intakes an int to define the number of frames along the X axis to be used in each animation action, if more than one.
- framesY: Intakes an int to define the number of frames along the Y axis to be used in each animation action, if more than one.
- frameSpeed: Intakes a float, in seconds, to define how fast the animation should switch between frames.

>Usage:

```C#
Animation = new Animation("Sprites/Hero", 4, 4, 0.5f);
```

>The file path given is relative to the folder structure in your MonoGame Content folder.

Optionals:

- depth: Intakes a float to define the draw depth of the animation. Default is 1.0f.
- transparency: Intakes a float to define the animation's transparency level. Default is 1.0f.
- scaleX: Intakes a float to define the width draw scale of the animation's Texture. Default is 1.0f.
- scaleY: Intakes a float to define the height draw scale of the animation's Texture. Default is 1.0f.
- isOriginAtCenter: Intakes a bool to define if the Origin of the Animation will be used to offset the Animation's Rectangle starting X and Y coordinates.
- loopStyle: Intakes a LoopStyle to define how the animation will loop.
- loopLength: Intakes an int to define how many loops the animation will perform before stopping. Default is (int)LoopsLengths.Infinite which is -1.

> LoopStyles:  
> 1. Forward - Draws the animation's frames from left to right along the X axis.
> 2. Reverse - Draws the animation's frames from right to left along the X axis.
> 3. Alternating - Alternates between Forward and Reverse.

Usage w/optionals:

```C#
Animation = new Animation("Sprites/Hero", 4, 4, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, true, Animation.LoopStyles.Forward, (int)Animation.LoopLengths.Infinite);
```

3. Update the Animation.
> In your Update(GameTime gameTime) method you'll need to setup your logic for moving the Animation.  
> Animation.Position can be used to move the Animation or see the Controls Section for more ways to make the Animation move.

```C#
Animation.Update(gameTime);
```
> Pass in MonoGame's instantiated [GameTime](http://www.monogame.net/documentation/?page=T_Microsoft_Xna_Framework_GameTime) class.

4. Draw the Animation!

```C#
Animation.Draw(spriteBatch);
```
> Pass in MonoGame's instantiated [SpriteBatch](http://www.monogame.net/documentation/?page=T_Microsoft_Xna_Framework_Graphics_SpriteBatch) class.

That's it! Your Animation should now be animating!

## Features

The Softfire Animation Library allows you to easily animate a 2D spritesheet.  
There are numerous ways to manipulate the Animation to your needs.  
To name a few:  

- Basic Vector2 movement.
- Advanced movement using velocity, acceleration and drag.
- Options for Animation loop lengths; None, Any Number and Infinite.
- Animate frames from left to right, right to left and alternating between the two.
- Rotate it!
  - Around...
  - Towards...
  - Away...
    - ...from another object...
    - or itself!
- Column and row selection for starting at specific frames in the spritesheet.
- Transparency! Make it vanish!
- Scale it out. 2x, 3x, 55x, 999x!
  - Got a screen big enough?

## Document Informaton

Version: `1.0`  
Author: `Softfire`  
Updated: `Sept. 5, 2017` 