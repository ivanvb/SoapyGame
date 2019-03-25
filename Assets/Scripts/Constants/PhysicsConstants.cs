/**
 * The purpose of this class is to store constants related
 * to the physical behaviour of the GameObjects (e.g Object's 
 * movement speed).
 */

public static class PhysicsConstants : object {

	public const float CARD_FLIP_VELOCITY = 2.5f;
	public const float CARD_MOVEMENT_VELOCITY = 0.25f;
	public const int FRONT_FACING_CARD_Y_AXIS = 0;
	public const int BACK_FACING_CARD_Y_AXIS = 180;

	public const int X_DELETION_POS = 10;
	public const int Y_DELETION_POS = 10;

	public const float CARD_DELAY_FLIP = 0.15f;

	public const float CARD_APPEARING_X = -0;
	public const float CARD_APPEARING_Y = -1f;
	public const float CARD_APPEARING_Z = 0f;

	public const float SMALL_CARD_X_POSITION_OFFSET = 2.65f;
	public const float SMALL_CARD_HORIZONTAL_SPACE_BETWEEN_CARDS = 0.75f;
	public const float SMALL_CARD_Y_POSITION_OFFSET = 5.25f;
	public const float SMALL_CARD_VERTICAL_SPACE_BETWEEN_CARDS = 1.1f;

	public const float MED_CARD_X_POSITION_OFFSET = 2.72f;
	public const float MED_CARD_HORIZONTAL_SPACE_BETWEEN_CARDS = 0.9f;
	public const float MED_CARD_Y_POSITION_OFFSET = 5.25f;
	public const float MED_CARD_VERTICAL_SPACE_BETWEEN_CARDS = 1.34f;

	public const float BIG_CARD_X_POSITION_OFFSET = 2.88f;
	public const float BIG_CARD_HORIZONTAL_SPACE_BETWEEN_CARDS = 1.15f;
	public const float BIG_CARD_Y_POSITION_OFFSET = 5.25f;
	public const float BIG_CARD_VERTICAL_SPACE_BETWEEN_CARDS = 1.65f;


	public const float ONE_CANVAS_SCALE = 1 / 192f;
	public const float CANVAS_OFFSET = 320f;
}
