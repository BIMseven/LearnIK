using UnityEngine;
using System.Collections;

public class ButterworthFilter 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ChangeMe";
	public bool VERBOSE = false;
    
    public enum PassTypes
    {
        Highpass,
        Lowpass,
    }

//---------------------------------------------------------------------------FIELDS:
	
    public float Value
    {
        get { return this.outputHistory[0]; }
    }

    /// <summary>
    /// rez amount, from sqrt(2) to ~ 0.1
    /// </summary>
    public float Resonance { get; private set; }    

    public float Frequency { get; private set; }
    public int SampleRate { get; private set; }
    public PassTypes PassType { get; private set; }

    private  float c, a1, a2, a3, b1, b2;

    /// <summary>
    /// Array of input values, latest are in front
    /// </summary>
    private float[] inputHistory = new float[2];

    /// <summary>
    /// Array of output values, latest are in front
    /// </summary>
    private float[] outputHistory = new float[3];
    
//---------------------------------------------------------------------CONSTRUCTORS:
	
    public ButterworthFilter( float frequency, 
                              int sampleRate, 
                              PassTypes passType, 
                              float resonance )
    {
        SetParameters( frequency, sampleRate, passType, resonance );
    }

//--------------------------------------------------------------------------METHODS:

    public void ClearHistory()
    {
        inputHistory = new float[2];
        outputHistory = new float[3];
    }

    public void SetParameters( float frequency,
                               int sampleRate,
                               PassTypes passType,
                               float resonance )
    {
        this.Resonance = resonance;
        this.Frequency = frequency;
        this.SampleRate = sampleRate;
        this.PassType = passType;

        switch( passType )
        {
            case PassTypes.Lowpass:
                c = 1.0f / (float)Mathf.Tan( Mathf.PI * frequency / sampleRate );
                a1 = 1.0f / ( 1.0f + resonance * c + c * c );
                a2 = 2f * a1;
                a3 = a1;
                b1 = 2.0f * ( 1.0f - c * c ) * a1;
                b2 = ( 1.0f - resonance * c + c * c ) * a1;
                break;
            case PassTypes.Highpass:
                c = (float)Mathf.Tan( Mathf.PI * frequency / sampleRate );
                a1 = 1.0f / ( 1.0f + resonance * c + c * c );
                a2 = -2f * a1;
                a3 = a1;
                b1 = 2.0f * ( c * c - 1.0f ) * a1;
                b2 = ( 1.0f - resonance * c + c * c ) * a1;
                break;
        }
    }

    public void Update( float newInput )
    {
        float newOutput = a1 * newInput + 
                          a2 * this.inputHistory[0] + 
                          a3 * this.inputHistory[1] - 
                          b1 * this.outputHistory[0] - 
                          b2 * this.outputHistory[1];

        inputHistory[1] = this.inputHistory[0];
        inputHistory[0] = newInput;

        outputHistory[2] = this.outputHistory[1];
        outputHistory[1] = this.outputHistory[0];
        outputHistory[0] = newOutput;
    }

}

