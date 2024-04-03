using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace rj.ghost.editor
{
public class GhostPlayer : MonoBehaviour
{
    List<Vector3> recorders = new List<Vector3>();
    List<Quaternion> recorderRotation = new List<Quaternion>();
    List<GameObject> foots = new List<GameObject>();
    private GameObject nowF;
    private bool isR = true;
    [Tooltip("Set up Ghost object")]
    public GameObject myGhost;
    [Tooltip("Automatically erase the position that ghost has reproduced")]
    public bool isBurn;
    [Tooltip("Set up Footprint object")]
    public GameObject Footprint;
    [Tooltip("Set Footstep Frequency")]
    public float FootprintRate;
    [Tooltip("Set slow play rates")]
    [Range(0, 0.01f)]
    private bool isGhost =false;
    public float SlowPlay;
    private Vector3 PP;
    private Transform footP;
    private GameObject ghost;

    //private PlayGhost playGhost;
    // Start is called before the first frame update
    void Start()
    {
        footP = new GameObject("foot prints parent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //按R清空数据
        //Press R to Clear data
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Clear data");
            ClearAllData();
        }
        //Press T to print the recorded frames numbers
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("There are " + recorders.Count() + " recorders");
        }
        //Hold right button to rewind the ghost
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            print("Play Ghost");
            StartCoroutine("PlayGhost");
        }
        //Release right button to stop rewind the ghost
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            print("Stop Replay");
            StopCoroutine("PlayGhost");
        }
        //Hold Left button to record
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            print("Play Ghost");
            StartCoroutine("LeaveFoot");
            StartCoroutine("StarRecord");
        }
        //Release Left button to stop record
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            print("Stop Replay");
            StopCoroutine("StarRecord");
            StopCoroutine("LeaveFoot");
        }
    }
    //Clear all current recorded data, position, ghost and footsteps
    private void ClearAllData()
    {
        recorders.Clear();
        recorderRotation.Clear();
        isGhost = false;
        for (int i = foots.Count - 1; i >= 0; i--)
        {
            GameObject FF = foots[i];
            Destroy(FF);
        }
        ghost.SetActive(false);
    }
    //Instantiate the ghost 
    private void CreateGhost()
    {
        if (isGhost != true && recorders[0] != null)
        {
            ghost = Instantiate(myGhost, transform.position, transform.rotation) as GameObject;
            isGhost = true;
        }
        if (recorders[0] != null)
        {
            ghost.SetActive(true);
        }
    }
    //Record current position data
    IEnumerator StarRecord()
    {
        while (isR)
        {
            recorders.Add(transform.position);
            recorderRotation.Add(transform.rotation);
            yield return null;
        }
    }
    //Rewind the ghost, at the same time erase the footsteps that the ghost passed
    IEnumerator PlayGhost()
    {
        CreateGhost();//Only instantiate the ghost when rewind started
        for (int i = 0; i < recorders.Count - 1; i++)
        {
            if (recorders[i] != null)
            {   
                PP = recorders[i];
                ghost.transform.rotation = recorderRotation[i];
                ghost.transform.position = PP;
                recorders.Remove(PP);
                recorderRotation.Remove(recorderRotation[i]);
                i--;
            }
            StartCoroutine("EraserFoot");//Erase footsteps
            yield return new WaitForSeconds(SlowPlay);
        }
    }
    //Leave a footprint after the assigned time
    IEnumerator LeaveFoot()
    {
        while (isR)
        {
            GameObject footPrints;
            footPrints = Instantiate(Footprint, transform.position, transform.rotation) as GameObject;
            nowF = footPrints;
            footPrints.transform.SetParent(footP);
            foots.Add(nowF);
            yield return new WaitForSeconds(FootprintRate);
        }
    }
    //Erase the footprint when the ghost passed the exact same position
    IEnumerator EraserFoot()
    {
        for (int k = foots.Count - 1; k >= 0; k--)
        {
            GameObject FF = foots[k];
            if (FF != null && FF.transform.position == PP && isBurn == true)
            {
                Destroy(FF);
            }
        }
        yield return null;
    }
}

}