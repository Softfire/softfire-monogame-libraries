![Logo of the project]()

# Softfire State Management Library
> Filename: Softfire.MonoGame.SM.dll  
> Requires: .Net 4.7+, MonoGame 3.6+  
> Sources: [.Net](https://www.microsoft.com/en-us/download/details.aspx?id=55170), [MonoGame](http://www.monogame.net)

This library contains C# classes that can be used to manage States.  
A State is a reusable screen such as a title screen or options screen. Actions can be performed in each state independantly and are maintained by the State Manager class.  
States are maintained on a Stack and are acccessed by Pushing and Popping States to and from the Stack.
Transitions can also be used to move over to another screen if desired.

## Table of Contents

- [States](STATES.md)
- [Transitions](TRANSITIONS.md)

## Installing / Getting started

1. Create a new State Manager variable at the top of your class.
> Consider making the State Manager a static variable so it can be accessed from elsewhere within your project.

```C#
public StateManager StateManager { get; set; }
```

2. Instantiate your new State Manager variable by calling it's constructor and passing in it's requirements.


```C#
StateManager = new StateManager(graphicsDevice, content);
```

Requirements:

- graphicsDevice: Intakes a GraphicsDevice used to draw the states.
- content: Intakes a ContentManager.

3. Update the State Manager.

```C#
StateManager.Update(gameTime);
```

> Pass in MonoGame's instantiated [GameTime](http://www.monogame.net/documentation/?page=T_Microsoft_Xna_Framework_GameTime) class.  
> The Update method also maintains the Transition class DeltaTime.

4. Draw the active States!

```C#
StateManager.Draw(spriteBatch);
```
> Pass in MonoGame's instantiated [SpriteBatch](http://www.monogame.net/documentation/?page=T_Microsoft_Xna_Framework_Graphics_SpriteBatch) class.  
> If the state's IsVisible bool is true then the state will be drawn.

That's it! Your State Manager should now be ready to load and draw your States!

## Usage

###### AddState()

To use States within the State Manager States must be added first to the list of available states.
To add a newly instantiated or anonymous derived class to the AddState method you would call the AddState method.
> The State class is an abstract class so new instances will need to be made from your own derived class.
> See [States](STATES.md) for details on creating your derived class.

```C#
StateManager.AddState(string identifier, State state);
```

Requirements:

- identifier: A unique key/identifier is provided that will be used to get the State when needed. Intaken as a `string`.
- state: The state that will be added is provided here. Intakes a derived class of `State`.

###### GetState()
> Returns a `State`.

Rerieves the state with the provided key/identifier.

```C#
State state = StateManager.GetState(string identifier);
```

Requirements:

- identifier: A unique key/identifier is provided that will be used to get the State. Intaken as a `string`.

###### RemoveState()

Removes the state with the provided key/identifier.

```C#
StateManager.RemoveState(string identifier);
```

Requirements:

- identifier: A unique key/identifier is provided that will be used to get the State. Intaken as a `string`.


## Features

The Softfire State Management Library allows you to easily add, remove, view, update and draw active States.

## Document Informaton

Version: `1.0`  
Author: `Softfire`  
Updated: `Sept. 5, 2017` 