/**
 * This class manages stores definitions for the
 * different states of the GameObjects.
 */

public static class StateConstants{

	/// Card States
	public const int STATE_FRONT_FACING_IDLE = 0;
	public const int STATE_BACK_FACING_IDLE = 1;
	public const int STATE_BACK_FLIPPING = 2;
	public const int STATE_FRONT_FLIPPING = 3;
	public const int STATE_MOVING = 4;
	public const int STATE_READY_TO_BE_DESTROYED = 5;

	/// Special Card States
	public const int DEFAULT_CARD_STATE = STATE_BACK_FACING_IDLE;

	public const float NO_FLIPPING_DELAY = -1f;
}
