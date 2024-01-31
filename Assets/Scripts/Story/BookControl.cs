using UnityEngine;

public class BookControl : MonoBehaviour {


    public bool controlWithKeys;
    public bool controlWithSwipe;
    public bool isVertical;


	private Vector2 startPosition;

	void Update () {
	
		if (controlWithKeys) {
            if (isVertical) {
                if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                    SendMessageUpwards("TurnPage", -1);
                if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
                    SendMessageUpwards("TurnPage", 1);
            } else {
                if (Input.GetKeyDown("z") || Input.GetKeyDown("a") || Input.GetKeyDown("left"))
                    SendMessageUpwards("TurnPage", -1);
                if (Input.GetKeyDown("x") || Input.GetKeyDown("d") || Input.GetKeyDown("right"))
                    SendMessageUpwards("TurnPage", 1);
            }
        }


        if (controlWithSwipe) {
		    if (Input.GetMouseButtonDown(0))
			    startPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		    if (Input.GetMouseButtonUp(0)) {
			    Vector2 currentSwipe = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startPosition).normalized;
                if (isVertical) {
			        if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
				        if (currentSwipe.y < -0.1f)
					        SendMessageUpwards("TurnPage", 1);
				        else if (currentSwipe.y > 0.1f)
					        SendMessageUpwards("TurnPage", -1);
			        }
                } else {
			        if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
				        if (currentSwipe.x < -0.1f)
					        SendMessageUpwards("TurnPage", -1);
				        else if (currentSwipe.x > 0.1f)
					        SendMessageUpwards("TurnPage", 1);
			        }
                }
		    }
        }

	}

}