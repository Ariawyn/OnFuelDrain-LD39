using UnityEngine;

public class CameraController : MonoBehaviour {

	// Target of camera, in this case it would be the players transform
	// Public as it would be set through inspector
	public Transform target;

	// Public floats values to designate the bounds of the object / target since a polygon collider doesnt have collider.bounds,
	// so we also set this through inspector
	public float boundsHeight;
	public float boundsWidth;

	// Actual instance of target bounds struct
	private Bounds targetBounds;

	// Create focus area according to target bounds
	private FocusArea focusArea;
	public Vector2 focusAreaSize;

	// Distance offset used for look ahead of player
	public float verticalLookAheadDistance;
	public float horizontalLookAheadDistance;

	// Directions of vertical and horizontal look ahead movement
	private float verticalLookAheadDirection;
	private float horizontalLookAheadDirection;

	// Current and target look ahead for vertical and horizontal axis
	private float currentVerticalLookAhead;
	private float currentHorizontalLookAhead;
	private float targetVerticalLookAhead;
	private float targetHorizontalLookAhead;

	// Smooth times for look ahead
	public float verticalLookAheadSmoothTime;
	public float horizontalLookAheadSmoothTime;

	// Smoothing velocity
	private float smoothLookAheadVerticalVelocity;
	private float smoothLookAheadHorizontalVelocity;


	// Use this for initialization
	void Start () {
		// Calculate and set target bounds
		this.targetBounds = new Bounds(target.position, new Vector3(boundsWidth, boundsHeight, 0));

		// Create focusArea;
		this.focusArea = new FocusArea(this.targetBounds, focusAreaSize);
	}
	
	void FixedUpdate() {
		// Update bounds position
		this.targetBounds.center = this.target.position;

		// Update focusArea
		this.focusArea.Update(this.targetBounds);

		// TODO: Check if the player stops while we are trying to change the look ahead values
		// In that case we do not want to continue moving the lookahead focus position forward
		// The if statements below would be useful for that

		// Check for focus area movement
		if(this.focusArea.velocity.x != 0) {
			// We have focus area movement along the x axis
			this.horizontalLookAheadDirection = Mathf.Sign(this.focusArea.velocity.x);
		}
		if(this.focusArea.velocity.y != 0) {
			// We have focus area movement along the y axis
			this.verticalLookAheadDirection = Mathf.Sign(this.focusArea.velocity.y);
		}

		// Create focus area position for camera to follow
		Vector2 focusPosition = this.focusArea.center;

		// Set target and current horizontal look ahead
		this.targetHorizontalLookAhead = this.horizontalLookAheadDirection * this.horizontalLookAheadDistance;
		this.currentHorizontalLookAhead = Mathf.SmoothDamp(this.currentHorizontalLookAhead, this.targetHorizontalLookAhead, 
			ref this.smoothLookAheadHorizontalVelocity, this.horizontalLookAheadSmoothTime);

		// Add horizontal look ahead values to focus position
		focusPosition += Vector2.right * this.currentHorizontalLookAhead;

		// Set target and current vertical look ahead
		this.targetVerticalLookAhead = this.verticalLookAheadDirection * this.verticalLookAheadDistance;
		this.currentVerticalLookAhead = Mathf.SmoothDamp(this.currentVerticalLookAhead, this.targetVerticalLookAhead, 
			ref this.smoothLookAheadVerticalVelocity, this.verticalLookAheadSmoothTime);

		// Add vertical look ahead values to focus position
		focusPosition += Vector2.up * this.currentVerticalLookAhead;

		// Set the camera position as the focus position, except with correct z axis so we actually capture the things on the camera lol
		this.transform.position = (Vector3)focusPosition + Vector3.forward * -10;

	}

	// GIZMOS!!!!
	void OnDrawGizmos() {

		// Draw bounds
		Gizmos.color = new Color(0,0,0, .5f);
		Gizmos.DrawCube(this.targetBounds.center, this.targetBounds.size);

		// Draw focus area
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);
	}

	struct FocusArea {
		// Center point of focus area
		public Vector2 center;

		// Basically the amount of movement the focus area has done since the last frame
		public Vector2 velocity;

		// Floats designating the sides of the area
		float left, right;
		float top, bottom;

		// Takes bounds of the target, and focus area size
		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			center = new Vector2((left + right)/2, (top + bottom)/2);
		}

		// Update location of focus area
		public void Update(Bounds targetBounds) {
			// The amount the focus area needs to shift x axis to still focus on target
			float shiftX = 0;
			// Compare the amount the target has moved to focus area sides
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			// The amount the focus area needs to shift y axis to still focus on target
			float shiftY = 0;
			// Compare the amount the target has moved to focus area sides
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;

			// Update center
			center = new Vector2((left + right)/2, (top + bottom)/2);

			// Update velocity
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}
