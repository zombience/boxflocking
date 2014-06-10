/*	Globals should be kept to a minimum
*	use them in to indicate things such as enums of global gamestates
*	and lists of broadcast messages */

// Global broadcast signatures
public enum SIG
{
	REGISTERLINEMAN,
	REGISTERCUBEZONE,
	CUBEZONETRIGGER,
	ENTERINGPORTAL,
	CANCELPORTAL,
	LOADINGLEVEL,
	BETWEENWORLDS
}

public enum LEAPFX
{
	GATHER,
	FORCE,
	NONE
}
