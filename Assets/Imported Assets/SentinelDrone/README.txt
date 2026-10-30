SENTINEL DRONE by Rainwire  (v1.0)
Free cyberpunk enemy - 4 animations + normal map

A small hovering security drone for side-view games, with hover, charge, shoot and death animations.

CONTENTS
  spritesheet/  the whole sheet (sentinel_drone.png), its normal map (sentinel_drone_n.png) and
                sentinel_drone.json (frame size, origin, and each animation's row, frame count, fps, loop)
  strips/       one PNG per animation, frames left to right (strips/normal/ has the normal maps)
  godot/        a ready-made Godot 4 folder: SpriteFrames, a normal-mapped material and a demo scene

FRAMES
  Frame size 32x32 px, one animation per row, origin pixel (16, 16).
  The origin is the drone's center.

  animation        row  frames   fps  loop
  hover              0       4    12  yes
  charge             1       3     8  no
  shoot              2       2    12  no
  death              3       5    10  no

GODOT 4 QUICK START
  1. Copy the godot/sentinel_drone folder anywhere into your project.
  2. Open sentinel_drone_demo.tscn and run it (F6): Left/Right cycles animations, F flips.
  3. In your own scene: add an AnimatedSprite2D, set Sprite Frames to sentinel_drone_frames.tres,
     Material to sentinel_drone_lit.tres, Texture > Filter to Nearest, and turn off Centered with
     Offset = (-16, -16) so the node's position is the origin pixel.
  Normal maps shade the sprite under any Light2D / PointLight2D. Without lights it looks like the plain art.

OTHER ENGINES
  Use the strips, or slice the sheet into 32x32 cells using the table above.
  Normal maps are OpenGL-style (green = up), which Unity and Godot expect. For DirectX-style engines
  such as Unreal, invert the green channel.

HOW IT WAS MADE
  The frames come from Rainwire's own procedural generator: code draws each frame pixel by pixel
  (characters are posed on a skeleton first), and the normal maps are computed from each frame's
  shape. No image-generation AI or training data was used for the art. The generator and the Godot
  scripts in the download were written with help from an AI coding assistant.

LICENSE
  Use it in your own commercial or free projects, no credit needed. Don't resell or redistribute the
  assets themselves. Full terms in LICENSE.txt.
