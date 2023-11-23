using UnityEngine;
using Spine.Unity;

public class CharacterCoverScript : MonoBehaviour
{
    // Counter for the number of bushes the character is behind
    private int behindBushCount = 0;

    // Reference to the SkeletonAnimation component
    private SkeletonAnimation skeletonAnimation;

    // Sorting order values
    [SerializeField] private int inFrontSortingOrder = 0;
    [SerializeField] private int behindSortingOrder = -1;

    // Reference to the Shadow object (assuming it's a sprite)
    private SpriteRenderer shadowSpriteRenderer;
    private SpriteRenderer swordSprite;

    private void Start()
    {
        // Get the SkeletonAnimation component from the sibling object named "Model"
        Transform modelTransform = transform.parent.Find("Model");
        if (modelTransform == null)
        {
            Debug.LogError("Sibling object named 'Model' not found.");
            return;
        }

        skeletonAnimation = modelTransform.GetComponent<SkeletonAnimation>();

        if (skeletonAnimation == null)
        {
            Debug.LogError("Character script requires a SkeletonAnimation component on the 'Model' sibling GameObject.");
        }

        // Get the SpriteRenderer component of the "Shadow" object as a child of "Model"
        Transform shadowTransform = modelTransform.Find("Shadow");
        if (shadowTransform == null)
        {
            Debug.LogError("Child object named 'Shadow' not found under 'Model'.");
            return;
        }

        shadowSpriteRenderer = shadowTransform.GetComponent<SpriteRenderer>();
        swordSprite = transform.parent.Find("DefaultPivot/Weapon").GetComponent<SpriteRenderer>();

        // Set initial sorting order
        UpdateSortingOrder();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bush"))
        {
            behindBushCount++;

            // Update sorting order only when entering the first bush
            if (behindBushCount == 1)
            {
                UpdateSortingOrder();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bush"))
        {
            behindBushCount--;

            // Update sorting order only when exiting the last bush
            if (behindBushCount == 0)
            {
                UpdateSortingOrder();
            }
        }
    }

    private void UpdateSortingOrder()
    {
        // Update the sorting order of the entire Spine character
        var modelMeshRenderer = skeletonAnimation.GetComponent<MeshRenderer>();

        if (modelMeshRenderer != null && shadowSpriteRenderer != null)
        {
            modelMeshRenderer.sortingOrder = (behindBushCount > 0) ? behindSortingOrder : inFrontSortingOrder;
            shadowSpriteRenderer.sortingOrder = modelMeshRenderer.sortingOrder - 1; // Adjust the shadow's sorting order
            swordSprite.sortingOrder = modelMeshRenderer.sortingOrder; // Adjust the shadow's sorting order
        }
    }
}
