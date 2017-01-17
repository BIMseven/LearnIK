using UnityEngine;
using System.Collections;
using System;

public class WaveformGenerator
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "WaveformGenerator";
	private const bool VERBOSE = true;

	private const double DEFAULT_FREQUENCY  	= 60;
	private const double DEFAULT_GAIN 			= 1.0;
	private const double DEFAULT_SAMPLING_FREQ 	= 2000;

//---------------------------------------------------------------------------FIELDS:

	private double samplingFrequency;
	private double frequency;
	private double gain;
	private double increment;
	private double phase;

//---------------------------------------------------------------------CONSTRUCTORS:

	public WaveformGenerator()
	{
		frequency = DEFAULT_FREQUENCY;
		samplingFrequency = DEFAULT_SAMPLING_FREQ;
		gain = DEFAULT_GAIN;
		phase = 0;
		updateIncrement();
	}

//--------------------------------------------------------------------------METHODS:

	public float[] generateCompleteWaveform( int length )
	{
		double phase = 0;
		float[] waveform = new float[length];
		for( int i = 0; i < length; i++ ) 
		{
			phase += increment;
			waveform[i] = (float)( gain * Math.Sin( phase ) );
		}
		return waveform;
	}

	public float nextOutput()
	{
		phase += increment;
		return (float)( gain * Math.Sin( phase ) );
	}

	public void resetWave()
	{
		phase = 0;
	}

//--------------------------------------------------------------------------HELPERS:

	private void updateIncrement()
	{
		increment = frequency * 2.0 * Math.PI / samplingFrequency;
	}

//--------------------------------------------------------------GETTERS AND SETTERS:

	public void setFrequency( double newFrequency )
	{
		frequency = newFrequency;
		updateIncrement();
	}

	public void setGain( double newGain )
	{
		gain = newGain;
	}

	public void setSamplingFrequency( double newSamplingFrequency )
	{
		samplingFrequency = newSamplingFrequency;
		updateIncrement();
	}
}