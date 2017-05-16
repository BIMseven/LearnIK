using UnityEngine;
using System.Collections;
using MyUtility;

/// <summary>
/// Intended to be placed on a Plane.  Will scale and place the plane so it takes 
/// up the full camera screen.
/// </summary>
public class FillScreen : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "FillScreen";
	public bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

    public bool FillOnAwake = false;
	public bool FillOnStart = false;

    private float unscaledWidth, unscaledHeight;

//---------------------------------------------------------------------MONO METHODS:

    void Awake()
    {
        Bounds unscaledBounds = transform.UnscaledAndUnrotatedBounds();
        unscaledWidth = unscaledBounds.extents.x * 2;
        unscaledHeight = unscaledBounds.extents.z * 2;

        if( FillOnAwake )
        {
            FillScreenOfParentCamera();
        }
    }

	void Start()
	{
        if( FillOnStart )
		{
            FillScreenOfParentCamera();
        }
	}

    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            FillScreenOfParentCamera();
        }
    }

//--------------------------------------------------------------------------METHODS:

	public void FillScreenOfCamera( Camera cam )
	{
		Vector3 toCamera = cam.transform.position - transform.position;

		Rect planeDim = cam.FrustumAtDistance( toCamera.magnitude );

        float targetXScale = planeDim.width / unscaledWidth;
        float targetYScale = planeDim.height / unscaledHeight;
        transform.localScale = new Vector3( targetXScale, 1, targetYScale );
        transform.position = cam.transform.position + 
                             cam.transform.forward * toCamera.magnitude;
    }
		
	public void FillScreenOfCameras( Camera cam1, Camera cam2 )
	{
		Vector3 toCamera1 = cam1.gameObject.transform.position - transform.position;
		Vector3 toCamera2 = cam2.gameObject.transform.position - transform.position;

		Vector3 span = GetComponent<Renderer>().bounds.size;

		// Divide by span
		Rect frustum1 = cam1.FrustumAtDistance( toCamera1.magnitude );
		Rect frustum2 = cam2.FrustumAtDistance( toCamera2.magnitude );

		Rect planeDim = Utility.CombineRects( frustum1, frustum2 );
		
		transform.localScale = new Vector3( planeDim.width, 1, planeDim.height );
		Vector3 camPosAverage = Vector3.Lerp( cam1.transform.position, 
											  cam2.transform.position, 
											  0.5f );

		transform.position = camPosAverage + cam1.transform.forward * 10f;
	}
    
    public void FillScreenOfParentCamera()
    {
        Camera camera = GetComponentInParent<Camera>();
        if( camera == null )
        {
            if( VERBOSE )
            {
                LOG_TAG.TPrint( "Unable to find camera in parent, looking in scene" );
            }
            camera = FindObjectOfType<Camera>();
        }
        FillScreenOfCamera( camera );
    }

//--------------------------------------------------------------------------HELPERS:

}