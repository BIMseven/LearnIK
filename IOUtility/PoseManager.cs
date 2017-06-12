using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using MyUtility;
//using Pose = System.Collections.Generic.List<Tuple<UnityEngine.Vector3, UnityEngine.Quaternion>>;
    
public class PoseManager : MonoBehaviour
{
    protected class Pose
    {
        //------------------------------------------------------FIELDS:

        List<Tuple<Vector3, Quaternion>> pose;

        public int NumJoints
        {
            get
            {
                return pose.Count;
            }
        }
        
        //------------------------------------------------CONSTRUCTORS:

        public Pose()
        {
            pose = new List<Tuple<Vector3, Quaternion>>();
        }
        
        //-----------------------------------------------------METHODS:

        public void AddJoint( Vector3 localPos, Quaternion localRot )
        {
            pose.Add( new Tuple<Vector3, Quaternion>( localPos, localRot ) );
        }

        public Vector3 GetJointPos( int jointNum )
        {
            return pose[jointNum].First;
        }

        public Quaternion GetJointRot( int jointNum )
        {
            return pose[jointNum].Second;
        }

        public static Pose FromString( string str )
        {
            Pose pose = new Pose();
            string[] components = str.Split( ',' );
            Queue<string> componentQueue = new Queue<string>( components );
            while( componentQueue.Count > 0 )
            {
                float posX = float.Parse( componentQueue.Dequeue(), 
                                          CultureInfo.InvariantCulture );
                float posY = float.Parse( componentQueue.Dequeue(), 
                                          CultureInfo.InvariantCulture );
                float posZ = float.Parse( componentQueue.Dequeue(), 
                                          CultureInfo.InvariantCulture );
                float rotX = float.Parse( componentQueue.Dequeue(),
                                          CultureInfo.InvariantCulture );
                float rotY = float.Parse( componentQueue.Dequeue(),
                                          CultureInfo.InvariantCulture );
                float rotZ = float.Parse( componentQueue.Dequeue(), 
                                          CultureInfo.InvariantCulture );
                float rotW = float.Parse( componentQueue.Dequeue(), 
                                          CultureInfo.InvariantCulture );
                Vector3 localPos = new Vector3( posX, posY, posZ );
                Quaternion localRot = new Quaternion( rotX, rotY, rotZ, rotW );
                pose.AddJoint( localPos, localRot );
            }
            return pose;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 0; i < pose.Count; i++ )
            {
                Vector3 pos = pose[i].First;
                Quaternion rot = pose[i].Second;
                builder.Append( pos.x.ToString( "N3" ) );
                builder.Append( "," );
                builder.Append( pos.y.ToString( "N3" ) );
                builder.Append( "," );
                builder.Append( pos.z.ToString( "N3" ) );
                builder.Append( "," );

                builder.Append( rot.x.ToString( "N3" ) );
                builder.Append( "," );
                builder.Append( rot.y.ToString( "N3" ) );
                builder.Append( "," );
                builder.Append( rot.z.ToString( "N3" ) );
                builder.Append( "," );
                builder.Append( rot.w.ToString( "N3" ) ); 
                if( i < pose.Count - 1 ) builder.Append( "," );
                else builder.Append( "\n" );
            }            
            return builder.ToString();
        }
    }

//------------------------------------------------------------------------CONSTANTS:

    private const string LOG_TAG = "PoseManager";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

    #region InspectorFields
    public bool LoadPosesOnStart;
    public string FileName = "RecordedPoses.csv";
    #endregion

    public int NumPoses
    {
        get
        {
            ensureInitialized();
            if( poses != null )   return poses.Length;
            return 0;
        }
    }
    public bool IsTransitioning { get; private set; }

    #region HelperFields
    private string filepath;
    private Pose[] poses;
    private Pose previousPose, nextPose;
    private float transitionStart, transitionFinish;
    private bool isInitialized;
    #endregion

//---------------------------------------------------------------------MONO METHODS:

    void FixedUpdate()  
    {
        if( ! IsTransitioning )   return;

        float t = ( Time.realtimeSinceStartup - transitionStart ) /
                  ( transitionFinish - transitionStart );

        if( t >= 1 )
        {
            matchPose( nextPose );
            IsTransitioning = false;
            return;
        }

        Pose targetPose = blendPoses( previousPose, nextPose, t );
        matchPose( targetPose );
    }

	void Start() 
	{
        ensureInitialized();
    }
		
	void Update()
	{
        // TODO temp!
        //if( Input.GetKeyDown( KeyCode.Space ) ) SaveCurrentPose();
    }

//--------------------------------------------------------------------------METHODS:

    /// <summary>
    /// Loads poses from file, returns the number loaded (-1 on error)
    /// </summary>
    /// <returns></returns>
    public int LoadPoses()
    {
        if( VERBOSE )    LOG_TAG.TPrint( "Loading poses from " + filepath );
        List<Pose> poseList = new List<Pose>();
        string fileString = IOUtility.ReadEntireFile( filepath );
        string[] poseStrings = fileString.Split( '\n' );
        foreach( string poseString in poseStrings )
        {
            if( poseString.Length == 0 )   continue;
            poseList.Add( Pose.FromString( poseString ) );
        }
        poses = poseList.ToArray();
        return poses.Length;
    }

    public int LoadPoses( string filepath )
    {
        this.filepath = filepath;
        if( filepath == "" )
        {
            Debug.LogError( "No filepath for recorded poses" );
        }
        return LoadPoses();
    }

    /// <summary>
    /// Appends the current pose to csv at filepath
    /// </summary>
    public void SaveCurrentPose()
    {
        if( VERBOSE )    LOG_TAG.TPrint( "Saving current pose to " + filepath );
        IOUtility.AppendToFile( filepath, currentPose().ToString() );
    }

    /// <summary>
    /// Transitions to the given poseNumber over the given transitionTime
    /// </summary>
    /// <param name="poseNumber"></param>
    /// <param name="transitionTime"></param>
    public void TransitionToPose( int poseNumber, float transitionTime )
    {
        transitionToPose( poses[poseNumber], transitionTime );
    }
            
//--------------------------------------------------------------------------HELPERS:
	    
    protected Pose blendPoses( Pose a, Pose b, float t )
    {
        Pose blendedPose = new Pose();

        int numJoints = a.NumJoints;

        for( int i = 0; i < numJoints; i++ )
        {
            Vector3 pos = Vector3.Lerp( a.GetJointPos( i ), 
                                        b.GetJointPos( i ), 
                                        t );

            Quaternion rot = Quaternion.Slerp( a.GetJointRot( i ), 
                                               b.GetJointRot( i ), 
                                               t );
            blendedPose.AddJoint( pos, rot );
        }
        return blendedPose;
    }

    protected Pose currentPose()
    {
        // local states of each joint
        List<Vector3> positions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        
        Queue<Transform> unvisited = new Queue<Transform>();
        unvisited.Enqueue( transform.root );
        while( unvisited.Count > 0 )
        {
            Transform visiting = unvisited.Dequeue();
            foreach( Transform child in visiting )
            {
                unvisited.Enqueue( child );
                positions.Add( child.localPosition );
                rotations.Add( child.localRotation );
            }
        }

        Pose pose = new Pose();
        for( int i = 0; i < positions.Count; i++ )
        {
            pose.AddJoint( positions[i], rotations[i] );
        }
        return pose;
    }

    protected void ensureInitialized()
    {
        if( isInitialized )   return;
        isInitialized = true;

        filepath = Application.dataPath + "/" + FileName;

        if( LoadPosesOnStart ) LoadPoses();

    }

    protected void matchPose( Pose pose )
    {
        int jointNum = 0;
        Queue<Transform> unvisited = new Queue<Transform>();
        unvisited.Enqueue( transform.root );
        while( unvisited.Count > 0 )
        {
            Transform visiting = unvisited.Dequeue();
            foreach( Transform child in visiting )
            {
                unvisited.Enqueue( child );
                child.localPosition = pose.GetJointPos( jointNum );
                child.localRotation = pose.GetJointRot( jointNum );
                jointNum++;
            }
        }
    }

    protected void transitionToPose( Pose pose, float transitionTime )
    {
        previousPose = currentPose();
        nextPose = pose;
        transitionStart = Time.realtimeSinceStartup;
        transitionFinish = transitionStart + transitionTime;
        IsTransitioning = true;
    }
}
