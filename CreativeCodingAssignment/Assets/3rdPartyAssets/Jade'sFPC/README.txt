===
SETUP

Edit > Project Settings > Input Manager
Add (or replace an existing) axis for "Sprint" and "Jump" (case sensitive)

If you wish to enable sprinting uncomment lines 23, 91, and 96

Camera sensitivity is controlled by the Main Camera object (child of main Jade'sFPC Object).

===
Other Notes

The groundMask is important. I have it set to default, but if your objects that are your ground are set on the "ground" layer, you will have to adjust the groundMask accordingly.
The most common bug that can arise is that you set your ground check too large, and it starts detecting the *player* as ground, allowing you to jump infinitely and slow fall.
To fix this, you will need to make sure that the groundMask is not searching for anything layer that the player is on.
Either set ALL of your terrain to be on a Ground layer, and set the groundMask to that, OR leave the terrain as default and give the Player it's own layer.
If you do all this, I reccomend bumbing up the ground Distance to 0.4 for slightly better feeling jumping.

Step height can be finicky. Be careful.

I tried my best to comment my code well. I hope it makes sense

===
Thank you for using my asset!