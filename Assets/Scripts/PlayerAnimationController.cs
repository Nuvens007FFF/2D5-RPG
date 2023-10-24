using UnityEngine;
using Spine.Unity;

public class PlayerAnimationController : MonoBehaviour
{
    [SpineAnimation] public string idleFront;
    [SpineAnimation] public string idleBack;
    [SpineAnimation] public string idleSide;

    [SpineAnimation] public string runFront;
    [SpineAnimation] public string runBack;
    [SpineAnimation] public string runSide;

    private SkeletonAnimation skeletonAnimation;
    private bool isFlipped;

    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    public void PlayIdleAnimation(Vector2 direction)
    {
        if (direction.y > 0)
            skeletonAnimation.AnimationState.AddAnimation(0, idleBack, true, 0);
        else if (direction.y < 0)
            skeletonAnimation.AnimationState.AddAnimation(0, idleFront, true, 0);
        else
            skeletonAnimation.AnimationState.AddAnimation(0, idleSide, true, 0);
    }

    public void PlayRunAnimation(Vector2 direction)
    {
        if (direction.y > 0)
            skeletonAnimation.AnimationState.AddAnimation(0, runBack, true, 0);
        else if (direction.y < 0)
            skeletonAnimation.AnimationState.AddAnimation(0, runFront, true, 0);
        else
        {
            skeletonAnimation.AnimationState.AddAnimation(0, runSide, true, 0);

            // Flip the character based on direction
            if (direction.x > 0)
                isFlipped = false;
            else if (direction.x < 0)
                isFlipped = true;

            skeletonAnimation.Skeleton.ScaleX = isFlipped ? -1 : 1;
        }
    }
}
