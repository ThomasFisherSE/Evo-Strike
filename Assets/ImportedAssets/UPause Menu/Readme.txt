Thanks for buyinf UPause Menu.


Get Started:-----------------------------------------------------

-Import UPause Menu into your project.
-Drap "PauseMenu" prefabs in your scene.
-Play scene and press "Escape".
- Note: for delect the example text in UI go to the PauseMenu prefab in scene in
PauseMenu -> PauseMenuUI -> Text(Delete).

Pro Tips:--------------------------------------------------------

-for access to sensitivity of pause menu use:

float yoursensitivity = bl_PauseOptions.Sensitivity;

**********************************************************
Use (bool)bl_PauseMenu.m_Pause reference pause the game, example:

void Update(){
//put this in top of function
if(bl_PauseMenu.m_Pause == true){//if pause 
   return; //no update the script
   }
}

or use as condition, example:

if(!bl_PauseMenu.m_Pause){//if not pause
//do something they only work when not paused
}


Support Email:brinerjhonson.lc@gmail.com
Support Forum:http://lovattostudio.com/Forum/index.php