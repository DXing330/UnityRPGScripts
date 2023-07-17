using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentChecker : MonoBehaviour
{
    // Everyday, checks if the deadline and quota are met.
    protected StoryDataManager story_data;

    private void Start()
    {
        story_data = GameManager.instance.story;
    }
}
