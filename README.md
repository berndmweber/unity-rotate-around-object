# Example Code for Unity - Rotate Camera around an object

The purpose of this project is to show how to move a camera around a 3D object in Unity. The idea is that this could be a player controller. Be aware that the code does not align the camera on a specific plane with the object it orbits around. Movement is aligned with the local up/forward vectors of the camera. So, for example, this is useful for orbiting a planet but likely not for orbiting a character in an isometric view setup. There would be extra work to make it useful for such a setup.

The code implements an acceleration/deceleration scheme when moving the camera. Zooming can be limited in how close or how far the camera moves into or away from the object it orbits.

The implemented controls are the WASD keyboard keys for up/down/left/right as well as the mouse wheel to zoom in/out.

## Use

In your own project:
1. Copy the [PlayerController.cs](Assets/Scripts/PlayerController.cs) file to your project's script directory.
2. Add the script as a component to the object you want the camera to orbit around.
3. Drag the Main Camera from your scene into the "Main Camera" slot in the "Player Controller (Script)" component.
4. Hit the "Play button" up top and use the WASD keys and the mouse wheel to move the camera around the object.

## Requirements

This code was implemented and tested on Unity version 2020.3.15f2 as well as Visual Studio 2019.

## Licensing

The C# code in this project is BSD-3 licensed. For specifics on the Unity related  files please check with Unity Technologies.
