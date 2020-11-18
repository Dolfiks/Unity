using UnityEngine;
using System.Collections;

public class GlobalValue : MonoBehaviour {
	public static bool isSound = true;
	public static bool isMusic = true;
	public static bool isRestart = false;

	public static int levelPlaying = 1;	
	public static int worldPlaying = 1; 

	public static bool checkpoint = false;
	public static bool isUsingJetpack = false;

	public static int checkpointstars = 0;
	public static int checkpointbullet = 0;
	public static int checkpointscore = 0;
}
