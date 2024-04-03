#pragma strict

 var hits : RaycastHit[];
 var ground : Collider;
 var height : float;
 var height2 : float;
 //var height2 : string;
 var stringToEdit : String ;
 

//public Text DegreesObject;

function Update(){

hits=Physics.RaycastAll (transform.position, Vector3.down , Mathf.Infinity);

 {
     for( var hit : RaycastHit in hits ) 
     {
         if( hit.collider.name == "Terrain" )
         {
             //height = transform.position.y - hit.point.y;//seems to work aswell
             height = hit.distance;

             height2 = 65f - height;
             
			 
			 stringToEdit = height2.ToString();
         }
     }
 }    

    
 }

 function OnGUI () {
        // Make a text field that modifies stringToEdit.
         //stringToEdit = GUI.TextField (Rect (50, 130, 200, 20), stringToEdit, 60);
		 GUI.TextField (new Rect (190, 130, 100, 20), "Alt: " + stringToEdit, 60);
		 //GUI.Label(new Rect(20,130,120,30), "Depth:");
    } 



