# UnityProjectForMike

Download Unity from its website https://unity.com/

Once unity hub is installed go to Installs and Install the version Unity 2022.3.10f1

Once the version is installed click New Project, 2D (Built In), Then name the project name whatever you want and then select where you want ur build files to go.
(And idk if u need to create a Unity cloud org but if you do look it up its pretty ez)

I also put some sprites i made incase u want use them(its in the Extras folder in git)!

All Scripts / any files u wanna use always go in the Assets folder in ur unity build

Once the project is built you should have a Main Camera!!!! GREAT SIGN DUDE!!! ur literally about to make a game!!!

Ok what you want to do now is right click in the hiearchy (Thats where you see all ur gameObjects) and click Create Empty and name that "Player" cause ur about to make the player!

So ur about to make the GFX(Graphics) for the player! So right click on the Player object you just made and click Create Empty and name it "GFX" u just made a child!!!

Now click On that child object and look in ur inspector(U see all ur components there VERY usefull stuff) Ur gonna wanna click Add Component and add Sprite Renderer and click on where it says Sprite under the Sprite Renderer and it should open a menu!
click the little eye ball with a slash through it and pick any sprite u want THATS THE PLAYER GFX!!!!

Now click on the Player gameobject and look in ur inspector again! Ur gonna wanna click Add Component and add a Rigidbody2D, BoxCollider2D And the player Script (that should be in the git files)
Look at ur RigidBody2D component u just made and click constraints and check the Freeze Rotation (should say Z right by the check box!)

Ok!!! now that you have all of those if u look in ur player script in the inspector ur gonna see all the vars! set them as u plz i try to make things modular(or whatever the word is for making things ez to change / reuse code!)

But there is a var called feet that needs to be specific! So create a new child under the player and name that something like "groundCheck" or "Feet" Once u create the child ur gonna wanna move it to the very bottom of ur player! 
(once u drag it in there should be a red circle make sure thats peeking out of ur collisions a little bit)

Ok!! half way done! We are about to make the ground now!!! So go to the hiearchy and right click and click 2D object/Sprites/Sqaure. and name that "Ground" You should see a white box now look at the inspector and look at ur transform component on the box and make the X scale like 20 
"it doesnt matter just big enough to walk around"

Now! ur gonna wanna add a box collider2D on ur ground object (if u dont remember go to the inspector and click add component then search "Box Collider 2D") Once that is setup you should see a button in the inspector called Layer (make sure to have the Ground object selected) and click Add layer and make a layer called Ground (or whatever u want!)

Now Go back to the player object and look at ur inspector and look at the player script var you added earlier and click "What is Ground" and set it to the new layer u just made!!!

That is the whole basic player controller setup!

(Be sure to drag the objects around and stuff. like the ground should be below the player OBV)


// Sorry if this is confusing i litearlly have no idea how to explain this stuff i kinda know how to do everything just not how to explain it GOOD LUCK!!! 

