using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class MultiPrefabApplier : EditorWindow
{
//------------------------------------------------------------------------CONSTANTS:

    private const string LOG_TAG = "MultiPrefabApplier";
    public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:

    public delegate void ApplyOrRevert( GameObject currentGO,
                                        Object prefabParentObj,
                                        ReplacePrefabOptions replaceOpt );

//--------------------------------------------------------------------------METHODS:

    [MenuItem( "Tools/Apply all selected prefabs %#a" )]
    static void ApplyPrefabs()
    {
        searchPrefabConnections( applyToSelectedPrefabs );
    }

    [MenuItem( "Tools/Revert all selected prefabs %#r" )]
    static void ResetPrefabs()
    {
        searchPrefabConnections( revertToSelectedPrefabs );
    }

//--------------------------------------------------------------------------HELPERS:

    // Apply      
    private static void applyToSelectedPrefabs( GameObject _goCurrentGo,
                                                Object _ObjPrefabParent,
                                                ReplacePrefabOptions replaceOpt )
    {
        PrefabUtility.ReplacePrefab( _goCurrentGo,
                                     _ObjPrefabParent,
                                     replaceOpt );
    }

    // Revert
    private static void revertToSelectedPrefabs( GameObject _goCurrentGo,
                                                 Object _ObjPrefabParent,
                                                 ReplacePrefabOptions replaceOpt )
    {
        PrefabUtility.ReconnectToLastPrefab( _goCurrentGo );
        PrefabUtility.RevertPrefabInstance( _goCurrentGo );
    }

    // Look for connections
    static void searchPrefabConnections( ApplyOrRevert prefabModFunction )
    {
        GameObject[] tSelection = Selection.gameObjects;

        if( tSelection.Length > 0 )
        {
            GameObject goPrefabRoot;
            GameObject goCur;
            bool topHierarchyFound;
            int iCount = 0;
            PrefabType prefabType;
            bool canApply;
            // Iterate through all the selected gameobjects
            foreach( GameObject go in tSelection )
            {
                prefabType = PrefabUtility.GetPrefabType( go );
                // Is the selected gameobject a prefab?
                if( prefabType != PrefabType.PrefabInstance &&
                    prefabType != PrefabType.DisconnectedPrefabInstance )
                {
                    continue;
                }

                var goParent = getPrefabParent( go );
                goPrefabRoot = goParent.transform.root.gameObject;

                goCur = go;
                topHierarchyFound = false;
                canApply = true;

                // We go up in the hierarchy to apply the root of the go to the prefab
                while( goCur.transform.parent != null  &&  ! topHierarchyFound )
                {
                    //Are we still in the same prefab?
                    GameObject parentCur = goCur.transform.parent.gameObject;
                    if( PrefabUtility.GetCorrespondingObjectFromSource( parentCur ) != null  &&
                        ( goPrefabRoot == getPrefabParent( parentCur ).transform.root.gameObject ) )
                    {
                        goCur = goCur.transform.parent.gameObject;
                    }
                    else
                    {
                        //The gameobject parent is another prefab, we stop here
                        topHierarchyFound = true;
                        if( goPrefabRoot != getPrefabParent( goCur ) )
                        {
                            //Gameobject is part of another prefab
                            canApply = false;
                        }
                    }
                }
                if( prefabModFunction != null   &&   canApply )
                {
                    iCount++;
                    prefabModFunction( goCur, PrefabUtility.GetCorrespondingObjectFromSource( goCur ),
                                       ReplacePrefabOptions.ConnectToPrefab );
                }

            }
            Debug.Log( iCount + " prefab" + ( iCount > 1 ? "s" : "" ) + " updated" );
        }
    }

    private static GameObject getPrefabParent( GameObject go )
    {
        return (GameObject)PrefabUtility.GetCorrespondingObjectFromSource( go );
    }
}
#endif

