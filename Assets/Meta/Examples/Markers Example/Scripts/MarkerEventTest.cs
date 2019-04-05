using Meta;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// MarkerEventTest is a demonstration of the MarkerTarget API's handler methods.
/// </summary>
/// <remarks>
/// MarkerEventTest uses the state information available in Meta.MarkerTarget.marker to make an animated marker-tracking vertical panel that appears and dissapears after changes in marker visibility after time threshholds are reached.
/// 
/// When the panel appears, it does so after a sci-fi "spinning up" animation plays and a QuadExpander prefab rectangle expands to the dimensions of the panel that is then shown to replace the effect.
/// </remarks>
public class MarkerEventTest : MonoBehaviour
{
    /// <summary>
    /// The spinner is the "loading indicator" that indicates something is happening while the marker spins up.
    /// </summary>
    public GameObject spinner;
    /// <summary>
    /// The panel is the content shown after the spin up animation completes.
    /// </summary>
    public GameObject panel;
    /// <summary>
    /// The time the spin up timer began.
    /// </summary>
    public float spinUpTimer = 0f;
    /// <summary>
    /// If the marker is spinning up, the animation will be playing.
    /// </summary>
    public bool spinningUp = false;
    /// <summary>
    /// Once the marker finishes spinning up, the animation is hidden and the panel is shown.
    /// </summary>
    public bool spunUp = false;
    /// <summary>
    /// The elapsed time the marker has been visible.
    /// </summary>
    public float timeVisible = 0f;
    /// <summary>
    /// The elapsed time the marker has been playing the spinning up animation.
    /// </summary>
    public float timeSpinning = 0f;
    /// <summary>
    /// The text shows how much time has elapsed till the spin up or spin down timer is finished.
    /// </summary>
    public Text countText;
    /// <summary>
    /// The material of the cube to be color-changed.
    /// </summary>
    public Material material;

    public void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    /// <summary>
    /// SpinUp starts after the marker is visible long enough, shows an animation and starts a timer.
    /// </summary>
    public void SpinUp()
    {
            spinningUp = true;
            spinner.SetActive(true);
            spinUpTimer = Time.time;
    }

    /// <summary>
    /// SpunUp completes the spinning up animation.
    /// </summary>
    public void SpunUp()
    {
        panel.SetActive(true);
        spinner.SetActive(false);
        spunUp = true;
        spinningUp = false;
    }

    /// <summary>
    /// SpinDown happens after the marker is not visible for long enough, hiding the panel and resetting.
    /// </summary>
    public void SpinDown()
    {
        panel.SetActive(false);
        spinner.SetActive(false);
        spinningUp = false;
        spunUp = false;
    }

    /// <summary>
    /// OnMarkerVisible happens every frame that the marker is visible. MarkerTarget.marker.status can be used to trigger behaviour instead of using the other handler methods such as OnMarkerVisibleStart, OnMarkerVisibleFirst, etc.
    /// </summary>
    /// <param name="mt">The MarkerTarget.marker gives you the state info you need to decide what happens each frame.</param>
    public void OnMarkerVisible(MarkerTarget mt)
    {
        material.color = Color.grey;
        timeVisible = Time.time - mt.marker.visibleStartTime;
        if (!spinningUp && !spunUp && timeVisible > 2f)
        {
            material.color = Color.white;
            SpinUp();
        }
        else
        {
            timeSpinning = Time.time - spinUpTimer;
            if (spinningUp && timeSpinning > 2f)
            {
                SpunUp();
            }
        }
        
    }

    /// <summary>
    /// OnMarkerVisibleHold happens only on frames after the first frame the MarkerTarget is detected.
    /// </summary>
    public void OnMarkerVisibleHold(MarkerTarget mt)
    {
        if (!spunUp)
        {
            countText.text = "" + (Time.time - mt.marker.visibleStartTime);
        }
        else
        {
            countText.text = "";
        }
    }

    /// <summary>
    /// OnMarkerVisibleStart happens the first frame a MarkerTarget is detected.
    /// </summary>
    public void OnMarkerVisibleStart(MarkerTarget mt)
    {
        Debug.Log("OnMarkerVisibleStart");
        material.color = Color.cyan;
    }

    /// <summary>
    /// OnMarkerVisbleFirst happens only the first time a MarkerTarget appears.
    /// </summary>
    /// <param name="mt"></param>
    public void OnMarkerVisibleFirst(MarkerTarget mt)
    {
        Debug.Log("OnMarkerVisibleFirst");
        //Debug.Log("OnMarkerVisible");
        
        material.color = Color.green;
        
    }

    /// <summary>
    /// OnMarkerNotVisibleStart happens on the first frame that MarkerTarget objects become not visible.
    /// </summary>
    public void OnMarkerNotVisibleStart(MarkerTarget mt)
    {
        Debug.Log("OnMarkerNotVisibleStart");
        material.color = Color.magenta;
    }

    /// <summary>
    /// OnMarkerNotVisible happens every frame that a MarkerTarget is not visible.
    /// </summary>
    public void OnMarkerNotVisible(MarkerTarget mt)
    {
        //Debug.Log("OnMarkerNotDetected");
        GetComponent<Renderer>().material.color = Color.yellow;
        if (Time.time - mt.marker.notVisibleStartTime > 2f)
        {
            material.color = Color.red;
            SpinDown();
        }
        if (spunUp)
        {
            countText.text = "" + (Time.time - mt.marker.notVisibleStartTime);
        }
        else
        {
            countText.text = "";
        }
    }
}
