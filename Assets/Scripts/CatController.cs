using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour
{
    public Resource resource = Resource.Wood;
    public GameObject cat;
    public float wiggleAngle = 12f;
    public float wiggleDuration = 0.3f;
    public float wiggleSpeed = 40f;

    private Coroutine wiggleRoutine;

    void OnEnable()
    {
        AutoClicker.OnAutoClickerStarted += HandleStarted;
        AutoClicker.OnAutoClick += HandleClick;
    }

    void OnDisable()
    {
        AutoClicker.OnAutoClickerStarted -= HandleStarted;
        AutoClicker.OnAutoClick -= HandleClick;
    }

    void Start()
    {
        cat.SetActive(false);
    }

    private void HandleStarted(Resource r)
    {
        if (r != resource) return;

        cat.SetActive(true);
        if (wiggleRoutine != null) StopCoroutine(wiggleRoutine);
        wiggleRoutine = StartCoroutine(Wiggle());
    }

    private void HandleClick(Resource r)
    {
        if (r != resource) return;
        cat.SetActive(true);
        if (wiggleRoutine != null) StopCoroutine(wiggleRoutine);
        wiggleRoutine = StartCoroutine(Wiggle());
    }
    

    private IEnumerator Wiggle()
    {
        float t = 0f;
        while (t < wiggleDuration)
        {
            t += Time.deltaTime;
            float fade = 1f - (t / wiggleDuration);
            float angle = Mathf.Sin(t * wiggleSpeed) * wiggleAngle * fade;
            cat.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
        cat.transform.localRotation = Quaternion.identity;
        wiggleRoutine = null;
    }
}