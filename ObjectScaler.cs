using UnityEngine;
using System.Collections;

public class ObjectScaler : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ObjectScaler";
	private const bool VERBOSE = true;

//---------------------------------------------------------------------------FIELDS:

	// Unscaled and unrotated dimensions
	public Vector3 neutralDim;

//---------------------------------------------------------------------MONO METHODS:

	void Awake()
	{
		neutralDim = UnscaledDimensions();
	}

//--------------------------------------------------------------------------METHODS:

	/// <summary>
	/// Scales uniformally to match the depth (z component) of targetDepth
	/// </summary>
	/// <param name="targetWidth">Target width.</param>
	public void ScaleToDepth( float targetDepth )
	{
		float targetScale = targetDepth / neutralDim.z;
		transform.localScale = Vector3.one * targetScale;
	}

	/// <summary>
	/// Scales uniformally to match the height (y component) of targetHeight
	/// </summary>
	/// <param name="targetWidth">Target width.</param>
	public void ScaleToHeight( float targetHeight )
	{
		float targetScale = targetHeight / neutralDim.y;
		transform.localScale = Vector3.one * targetScale;
	}

	/// <summary>
	/// Scales uniformally to match the width (x component) of targetWidth
	/// </summary>
	/// <param name="targetWidth">Target width.</param>
	public void ScaleToWidth( float targetWidth )
	{
		float targetScale = targetWidth / neutralDim.x;
		transform.localScale = Vector3.one * targetScale;
	}

	/// <summary>
	/// Returns the dimensions of the object if unrotatued
	/// </summary>
	/// <returns>The bounds.</returns>
	/// <param name="obj">Object.</param>
	public Vector3 UnrotatedDimensions()
	{
		Quaternion initialRotation = transform.rotation;
		transform.rotation = Quaternion.identity;
		Vector3 size = GetComponent<Renderer>().bounds.size;
		transform.rotation = initialRotation;
		return size;
	}

	/// <summary>
	/// Returns the dimensions of the object if unrotatued
	/// </summary>
	/// <returns>The bounds.</returns>
	/// <param name="obj">Object.</param>
	public Vector3 UnscaledDimensions()
	{
		Vector3 initialScale = transform.localScale;
		transform.localScale = new Vector3( 1, 1, 1 );

		Vector3 size = UnrotatedDimensions();
		transform.localScale = initialScale;
		return size;
	}

//--------------------------------------------------------------------------HELPERS:

}