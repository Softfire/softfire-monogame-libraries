![Logo of the project]()

# Softfire State Management Library - States
> Filename: Softfire.MonoGame.SM.dll  
> Requires: .Net 4.7+, MonoGame 3.6+  
> Sources: [.Net](https://www.microsoft.com/en-us/download/details.aspx?id=55170), [MonoGame](http://www.monogame.net)

This section contains methods integrated into the library that can be used to Add, Remove, Update and Draw States.

## Table of Contents

- [README](README.md)

## Basics

#### Class Creation

The State class is an abstract class.  
A derived class must be created to instantiate a State.
> The following is an example of a custom class derived from the State class.

```C#
public class CustomState : State
{
    public CustomState(string name, Vector2 position, int width, int height) : base(name, position, width, height)
    {
        // Your Code Here.
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}
```

#### Instantiation

```C#
State CustomState = new CustomState(string name, Vector2 position, int width, int height);
```

Requirements:

- name: Unique name to identify the State when on the Stack. Intaken as a `string`.
- position: The position of the State on the screen. Position is found at the center of the State. Intaken as a `Vector2`.
- width: The width of the State. Generally this is equal to the width of the screen. Intaken as an `int`.
- height: The height of the State. Generally this is equal to the height of the screen. Intaken as an `int`.

#### State.Sleep()
> This is a `virtual` method and can be overridden with the `override` keyword.

Sleep is used to transition the state into an inactive state.

```C#
if (ActivateSleep)
{
    if (await Sleep())
    {

    }
}
```

#### State.Wake()
> This is a `virtual` method and can be overridden with the `override` keyword.

Sleep is used to transition the state into an active state.

```C#
if (ActivateWake)
{
    if (await Wake())
    {

    }
}
```

#### State.RunTansition()
> This is an `async` method.  
> Returns a `Task<bool>`.

Runs the transition with the provided array index in an `async` call.

```C#
if (ActivateTransitions)
{
    if (await RunTransition())
    {

    }
}
```

Requirements:

- index: An index that will be used to search the Transitions array and run the Transition. Intaken as an `int`.

#### State.RunTansitions()
> This is an `async` method.  
> Returns a `Task<bool>`.

Runs all the transitions currently loaded in an `async` call.  
If even a single transition completes the rusult will be `true`.

```C#
if (ActivateTransitions)
{
    if (await RunTransitions())
    {

    }
}
```

## Document Informaton

Version: `1.0`  
Author: `Softfire`  
Updated: `Sept. 5, 2017`