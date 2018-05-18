using UnityEngine;

public class CameraController : MonoBehaviour {
    private const int MAP_SIZE = 30;

    public float scrollSpeed = 100f;
    public float panSpeed = 15f;
    public float minY = 20f;
    public float maxY = 120f;
    public float panBoarderThickness = 10f;

    public Vector2 panLimit;
	// Basic camera controls, will need further work
	void Update ()
    {
        Vector3 pos = transform.position;

	    if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoarderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("s") || Input.mousePosition.y <= 0 + panBoarderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoarderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= 0 + panBoarderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, 2, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -2, panLimit.y);

        transform.position = pos;
	}
}
