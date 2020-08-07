using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LevelEditor
{
    class modComponent : MonoBehaviour
    {

        public static SaveData saveData = new SaveData();

        GameObject tilemap8 = null;
        GameObject tilemap16 = null;

        PlayerManager PM;
        LevelManager LM;
        List<string> lookupList = new List<string>();
        List<GameObject> objectList = new List<GameObject>();

        GameObject clickedObject = null;
        GameObject copiedObject = null;
        public void Start()
        {




            lookupList.Add("SunkenTemple_Lantern");
            lookupList.Add("SunkenShrine_Stomper");
            lookupList.Add("HowlingGrotto_MovingWall");
            lookupList.Add("SunkenShrine_MovingWall");
            lookupList.Add("Timeshard");
            lookupList.Add("HP_5");
            lookupList.Add("SunkenTemple_SpikeTrap");
            lookupList.Add("SunkenTemple_FallingPlatform");
            lookupList.Add("SunkenShrine_Saw");
            lookupList.Add("MegaTimeShard");
            lookupList.Add("WaypointPlatform");
            lookupList.Add("PowerSeal");
            lookupList.Add("SunkenShrine_MovingPf");
            lookupList.Add("V_MovingSpikes_04");

        }

        public void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "I'm a little teapot");
        }


        private void copyToClipboard()
        {

        }

        private void pasteFromClipboard()
        {

        }
        private GameObject getClickedGPI()
        {
            Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] col = Physics2D.OverlapPointAll(v);
            foreach (Collider2D c in col)
            {
                GameObject now = c.gameObject;
                Transform a = c.transform.parent;
                while (a != null)
                {
                    if (a.name.ToLower().Contains("chunk"))
                    {
                        Debug.Log("we found a chunk parent on " + now.name);

                        ///Now we need to check if it has SpriteRenderers.
                        List<SpriteRenderer> SRL = now.GetComponentsInChildren<SpriteRenderer>().ToList();
                        SpriteRenderer SR = now.GetComponent<SpriteRenderer>();
                        if (SR != null)
                            SRL.Add(SR);


                        if (SRL.Count > 0)
                            return now;

                    }
                    now = a.gameObject;
                    a = a.parent;
                }
            }
            return null;
        }

        public void Update()
        {

            //Canvas can = FindObjectOfType<Canvas>();
            //CanvasScaler cans = can.gameObject.GetComponent<CanvasScaler>();
            //if (can == null)
            //{
            //    Debug.Log("we aint got not canvas");
            //    return;
            //}
            //Debug.Log("Render Mode: " + can.renderMode);
            //Debug.Log("Pixel Perfect: " + can.pixelPerfect);
            //Debug.Log("Render Camera: " + can.worldCamera.name);
            //Debug.Log("Sort Order: " + can.sortingOrder);
            //Debug.Log("Target Display: " + can.targetDisplay);
            //Debug.Log("Additional shader: " + can.additionalShaderChannels);

            //Debug.Log("Scaler Scale Mode: " + cans.uiScaleMode);
            //Debug.Log("Scaler Reference Resolution: " + cans.referenceResolution);
            //Debug.Log("Scalar Match Mode: " + cans.screenMatchMode);
            //Debug.Log("Scalar MatchWidthHeight: " + cans.matchWidthOrHeight);
            //Debug.Log("Scaler Scale Factor: " + cans.scaleFactor);
            //Debug.Log("Scaler reference pixels per unit: " + cans.referencePixelsPerUnit);
            //EventSystem eve = FindObjectOfType<EventSystem>();
            //if (eve == null)
            //    Debug.Log("there is NO eventmanager");
            Cursor.visible = true;

            if (PM == null || LM == null)
            {
                PM = Manager<PlayerManager>.Instance;
                LM = Manager<LevelManager>.Instance;
                return;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                PauseScreen PS = GameObject.FindObjectOfType<PauseScreen>();
                if (PS != null)
                    PS.cheatButton.SetActive(true);
            }



            if (Input.GetMouseButtonDown(0))
            {
                clickedObject = getClickedGPI();
                if (clickedObject == null)
                    return;
                OutlineObject(clickedObject);

                List<Component> car = clickedObject.GetComponentsInChildren<Component>().ToList();
                List<Component> carr = clickedObject.GetComponents<Component>().ToList();
                car.AddRange(carr);
                foreach (Component c in car)
                {
                    Debug.Log("Component of Type: " + c.GetType() + " on " + c.gameObject.name + " - " + c.gameObject.transform.childCount);
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
            {
                if (tilemap8 == null || tilemap16 == null)
                {
                    GameObject[] list = GameObject.FindObjectsOfType<GameObject>();
                    foreach (GameObject go in list)
                    {
                        if (go.name.Equals("Tilemap_8"))
                            tilemap8 = go;
                        if (go.name.Equals("Tilemap_16"))
                            tilemap16 = go;
                    }
                }
                if (tilemap8 != null)
                    tilemap8.SetActive(!tilemap8.activeInHierarchy);
                if (tilemap16 != null)
                    tilemap16.SetActive(!tilemap16.activeInHierarchy);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
            {
                if (clickedObject == null)
                    return;
                copiedObject = clickedObject;
                Destroy(clickedObject.GetComponent<LineRenderer>());
                copiedObject.SetActive(false);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
                if (copiedObject == null)
                    return;
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                copiedObject.transform.position = mouse;
                copiedObject.SetActive(true);
            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    for (int i = lookupList.Count - 1; i >= 0; i--)
                    {
                        string name = lookupList[i];
                        if (go.name.Equals(name) && go.transform.childCount > 0)
                        {
                            Debug.Log("We did find something with the name: " + name);
                            objectList.Add(go);
                            lookupList.RemoveAt(i);
                            break;
                        }
                        else
                        {
                            Debug.Log("WHAT WE DIDNT FIND WAS: " + name);
                        }
                    }
                }
                Debug.Log("-------------");
                foreach (GameObject go in objectList)
                {
                    Debug.Log("Again: " + go.name);
                }


            }
        }

        public void OutlineObject(GameObject clickedObjected)
        {
            List<SpriteRenderer> renderList = new List<SpriteRenderer>();

            SpriteRenderer SR = clickedObjected.GetComponent<SpriteRenderer>();
            if (SR != null)
                renderList.Add(SR);

            SpriteRenderer[] SRA = clickedObjected.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer tSR in SRA)
            {
                renderList.Add(tSR);
            }

            Vector2 minXY = new Vector2(99999f, 99999f);
            Vector2 maxXY = new Vector2(-99999f, -99999f);
            int sortOrder = -999;
            int layerID = 0;
            string layerName = null;
            //////////////////////////////////////////
            ///Make the bounding points by finding the min x\y and max x\y of all SpriteRenderers
            ///
            foreach (SpriteRenderer render in renderList)
            {
                Bounds b = render.bounds;
                Vector3 center = b.center;
                Vector3 topLeft = new Vector3(center.x - b.extents.x, center.y - b.extents.y, center.z);
                Vector3 bottomRight = new Vector3(center.x + b.extents.x, center.y + b.extents.y, center.z);
                int sOrder = render.sortingOrder;
                if (sOrder > sortOrder)
                    sortOrder = sOrder;
                layerID = render.sortingLayerID;
                layerName = render.sortingLayerName;


                Debug.Log("Our Layer sOrder: " + sOrder + ", sLayer: " + layerName + ", sID: " + layerID);
                if (topLeft.x < minXY.x)
                    minXY.x = topLeft.x;
                if (topLeft.y < minXY.y)
                    minXY.y = topLeft.y;
                if (bottomRight.x > maxXY.x)
                    maxXY.x = bottomRight.x;
                if (bottomRight.y > maxXY.y)
                    maxXY.y = bottomRight.y;
            }

            LineRenderer[] lra = GameObject.FindObjectsOfType<LineRenderer>();
            foreach (LineRenderer line in lra)
            {
                DestroyImmediate(line);
            }


            LineRenderer l = clickedObjected.GetComponent<LineRenderer>();
            if (l == null)
            {
                l = clickedObjected.AddComponent<LineRenderer>();
            }

            l.useWorldSpace = true;
            l.widthCurve = AnimationCurve.Linear(0, 1, 1, 1);
            l.widthMultiplier = 0.05f;
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;
            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.yellow;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            l.colorGradient = gradient;

            l.positionCount = 5;
            l.SetPosition(0, new Vector3(minXY.x, minXY.y, -1f));      //Top Left Start
            l.SetPosition(1, new Vector3(maxXY.x, minXY.y, -1f));      //Draw to Top Right
            l.SetPosition(2, new Vector3(maxXY.x, maxXY.y, -1f));      //Draw to Bottom Right
            l.SetPosition(3, new Vector3(minXY.x, maxXY.y, -1f));      //Draw to Bottom Left
            l.SetPosition(4, new Vector3(minXY.x, minXY.y, -1f));    //Draw back to Top Left Start

            SpriteRenderer playerSR = PM.Player.GetComponentInChildren<SpriteRenderer>();
            l.sortingOrder = playerSR.sortingOrder;
            l.sortingLayerName = playerSR.sortingLayerName;
            l.sortingLayerID = playerSR.sortingLayerID;
            l.material = new Material(Shader.Find("Sprites/Default"));
            l.material.color = Color.yellow;

        }

    }


}
