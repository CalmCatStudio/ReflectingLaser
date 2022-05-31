# ReflectingLaser
Creates a Line Renderer that can bounce off 2D or 3D colliders.

Includes two demo scenes that show how the 2D, and 3D versions work.

## Demo Scenes

### 2D scene
- Move with WASD
- Rotate with JL, or Arrow keys.

### 3D scene
- Rotate with IJKL, or Arrow keys.

## Setup
Setup is easy.
1. Create an object to fire the laser from.
2. Add ShootReflectingLaser.cs to the object.
3. Be sure to set the Origin, AimController, and Line Renderer on the ShootReflectingLaser component.
4. Make sure Layers to Bounce Off is setup properly.

## ShootReflectingLaser Properties
- Use2DPhysics: Whether the laser should bounce of 2D or 3D colliders.
- Origin: The starting position of the laser.
- Aim Controller: The direction the laser fires is based on this object. It will shoot "up" relative to this objects rotation.
- Line: The line renderer that represents the laser.
- Max Range: The maximum distance a laser can travel before ending.
- Max Bounces: The maximum number of bounces before a laser will be forced to stop.
- Layers To Bounce Off: Which layers should cause the laser to bounce.

## License
See the [LICENSE](License.txt) File for license right, and limitations(MIT).