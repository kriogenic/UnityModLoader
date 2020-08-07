using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Xml.Schema;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;

namespace UnityModLoader.Window
{
    public class Explorer : MonoBehaviour
    {
        public Rect explorerWindowRect = new Rect(20, 20, 800, 600);
        public Rect inspectorWindowRect = new Rect(820, 20, 800, 600);
        public bool explorerWindow = false;
        public Vector2 explorerScrollBar = new Vector2();
        public Vector2 inspectorScrollBar = new Vector2();

        public List<Transform> TList = new List<Transform>();

        public List<GameObject> RootList = new List<GameObject>();


        // GameObject selectedObject = null;






        private Vector2 scrollViewVector = Vector2.zero;
        public Rect dropDownRect = new Rect(500, 20, 300, 300);
        public static List<string> list = new List<string>();

        int indexNumber;
        bool show = false;






        public void Start()
        {

        }

        public void OnGUI()
        {
            GUI.skin = UnityModLoader.Window.UI.UISkin;
            if (explorerWindow)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                explorerWindowRect = GUI.Window(1, explorerWindowRect, ExplorerWindow, "Object Explorer");
                //   inspectorWindowRect = GUI.Window(2, inspectorWindowRect, InspectorWindow, "Inspector");











            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        //Hopefully make a node tree referenced by indexID
        public void recursiveAddition(GameObject go, string key)
        {
            UnityEngine.Debug.Log("Adding: " + key);
            string newkey = key;
            int childCount = go.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject ngo = go.transform.GetChild(i).gameObject;
                newkey = key + "." + ngo.name;
                recursiveAddition(ngo, newkey);
            }
        }
        public void Refresh(List<GameObject> thelist)
        {

            UnityEngine.Debug.Log("About to Refresh");
            TList = new List<Transform>();
            RootList = thelist;


            Dictionary<string, List<GameObject>> Heirarchy = new Dictionary<string, List<GameObject>>();



            //for(int k = 0; k < RootList.Count; k++)
            // {
            //     recursiveAddition(RootList[k], RootList[k].name);
            //  }


            // selectedObject = null;
            //foreach (Transform t in FindObjectsOfType<Transform>()) {
            //	TList.Add (t);
            //}
        }

        //public void InspectorWindow(int windowID)
        //{
        //    if(selectedObject == null)
        //    {
        //        GUILayout.Label("Select an object from the object explorer");
        //    }
        //    else
        //    {
        //        inspectorScrollBar = GUILayout.BeginScrollView(inspectorScrollBar);
        //        Component[] comps = selectedObject.GetComponents<Component>();
        //        foreach (Component c in comps)
        //        {
        //            GUILayout.BeginHorizontal();
        //            GUILayout.Label("Component Type: " + c.GetType().Name);
        //            GUILayout.EndHorizontal();

        //            GUILayout.BeginHorizontal();
        //            GUILayout.EndHorizontal();

        //            System.Type type = c.GetType();

        //            PropertyInfo[] PIA = type.GetProperties();
        //            foreach (PropertyInfo PI in PIA)
        //            {
        //                GUILayout.BeginHorizontal();
        //                GUILayout.Label("Propery & Value: " + PI.Name + ": " + PI.GetValue(c, null));
        //                GUILayout.EndHorizontal();
        //            }

        //            FieldInfo[] FIA = type.GetFields();
        //            foreach (FieldInfo FI in FIA)
        //            {
        //                GUILayout.BeginHorizontal();
        //                GUILayout.Label("Field & Value: " + FI.Name + FI.GetValue(c));
        //                GUILayout.EndHorizontal();
        //            }


        //            // PropertyInfo[] properties = typeof(Rigidbody).GetProperties();
        //            //Get all the properties of your class.
        //            // foreach (PropertyInfo pI in properties)
        //            //  {
        //            // }
        //        }


        //        GUILayout.EndScrollView();
        //    }
        //}

        public void ExplorerWindow(int windowID)
        {
            GUI.Label(new Rect(5, 20, 500, 20), "Press Ctrl + R to refresh the object list");

            if (RootList.Count == 0)
                return;

            /////////////ROOT OBJECT DROP DOWN
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y, dropDownRect.width, 25), ""))
            {
                if (!show)
                {
                    show = true;
                }
                else
                {
                    show = false;
                }
            }

            if (show)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect((dropDownRect.x - 100), (dropDownRect.y + 25), dropDownRect.width, dropDownRect.height), scrollViewVector, new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (RootList.Count * 25))));

                GUI.Box(new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (RootList.Count * 25))), "");

                for (int index = 0; index < RootList.Count; index++)
                {

                    if (GUI.Button(new Rect(0, (index * 25), dropDownRect.height, 25), ""))
                    {

                        List<GameObject> newgol = new List<GameObject>();
                        foreach (Transform t in RootList[index].transform)
                        {
                           
                                newgol.Add(t.gameObject);
                            
                        }
                        if (newgol.Count == 0)
                            return;

                       
                        Refresh(newgol);
                        show = false;
                        indexNumber = 0;


                        //  LoadChildren();
                    }

                    GUI.Label(new Rect(5, (index * 25), dropDownRect.height, 25), RootList[index].name);

                }

                GUI.EndScrollView();
            }
            else
            {
                GUI.Label(new Rect((dropDownRect.x - 95), dropDownRect.y, 300, 25), RootList[indexNumber].name);
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GameObject selected = RootList[indexNumber];
            if (selected != null)
            {
                float goffset = 0f;


                explorerScrollBar = GUI.BeginScrollView(new Rect(19, 49, 790, 400), explorerScrollBar, new Rect(0, 0, 790, Mathf.Infinity));
                foreach (Transform go in selected.transform)
                {
                    GUI.Label(new Rect(20, 50 + goffset, 800, 20), go.name);
                    goffset += 20;





                    //goffset += 30;
                    //Component[] comps = go.GetComponents<Component>();
                    //foreach (Component c in comps)
                    //{
                    //    GUI.Label(new Rect(20, 50 + goffset, 800, 20), "Component Type: " + c.GetType().Name);
                    //    goffset += 25;
                    //    System.Type type = c.GetType();

                    //    PropertyInfo[] PIA = type.GetProperties();
                    //    foreach (PropertyInfo PI in PIA)
                    //    {
                    //        GUI.Label(new Rect(20, 50 + goffset, 800, 20), "Propery & Value: " + PI.Name + ": " + PI.GetValue(c, null));
                    //        goffset += 20;
                    //    }

                    //}


                }



                GUI.EndScrollView();
            }
            //explorerScrollBar = GUILayout.BeginScrollView(explorerScrollBar);





            //////////////////////
            //float yoffset = 0;

            //foreach (GameObject robject in RootList)
            //{
            //    GUILayout.BeginHorizontal();
            //    if (GUI.Button(new Rect(10, 5 + yoffset, 125, 50), robject.name)){
            //        selectedObject = robject;
            //    }
            //    yoffset += 55;
            //    GUILayout.EndHorizontal();
            //}

            //////////////////////////////////////////////////////
            //foreach (Transform parent in t.gameObject.GetComponentsInParent<Transform>())
            //            {
            //                if (parent != null)
            //                {
            //                    GUILayout.Label(parent.name + " > ");
            //                }
            //            }

            //if (GUILayout.Button(t.name))
            //{
            //    foreach (Component c in t.GetComponents<Component>())
            //    {
            //        UnityEngine.Debug.Log(c.GetType().Name);
            //    }
            //}
            // GUILayout.EndScrollView();

        }

        public void LoadChildren()
        {

        }
        public void Update()
        {

            //if (explorerWindow && Input.GetKeyDown(KeyCode.G))
            //{
            //    Transform strans = selectedObject.transform;
            //    Vector3 pos = strans.position;
            //    pos.x -= 3.0f;
            //    strans.position = pos;

            //}
            //if (explorerWindow && Input.GetMouseButtonDown(0))
            //{



            //    UnityEngine.Debug.Log("MOUSE CLICK");
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
            //    foreach (RaycastHit2D hit in hits)
            //    {
            //        if (hit.collider != null)
            //        {
            //            // raycast hit this gameobject


            //            if (selectedObject != null && selectedObject == hit.transform.root.gameObject)
            //            {
            //                continue;
            //            }

            //            selectedObject = hit.transform.root.gameObject;

            //            UnityEngine.Debug.Log("Root Object: " + selectedObject.name);

            //            Component[] clist = selectedObject.GetComponents<Component>();
            //            foreach (Component comp in clist)
            //            {
            //                UnityEngine.Debug.Log("	Component: " + comp.GetType());
            //            }


            //            foreach (Transform child in selectedObject.transform)
            //            {

            //                UnityEngine.Debug.Log("Child: " + child.transform.name);
            //                Component[] cslist = child.GetComponents<Component>();
            //                foreach (Component comp in cslist)
            //                {
            //                    UnityEngine.Debug.Log("	Component: " + comp.GetType() + "," + comp.GetType().Namespace);

            //                }


            //            }

            //        }
            //    }

            //}

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O) || Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.O))
            {
                explorerWindow = !explorerWindow;
                GetComponent<Debug>().debugWindow = false;
                GetComponent<MainWindow>().consoleWindow = false;
                GetComponent<Online>().onlineWindow = false;
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.R))
            {
                Refresh(SceneManager.GetActiveScene().GetRootGameObjects().ToList());

            }


            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
            {

            }

            if (Input.GetKey(KeyCode.DownArrow) && explorerWindow)
            {
                explorerScrollBar.Set(explorerScrollBar.x, explorerScrollBar.y + 100);
                inspectorScrollBar.Set(inspectorScrollBar.x, inspectorScrollBar.y + 100);
            }

            if (Input.GetKey(KeyCode.UpArrow) && explorerWindow)
            {
                explorerScrollBar.Set(explorerScrollBar.x, explorerScrollBar.y - 100);
                inspectorScrollBar.Set(inspectorScrollBar.x, inspectorScrollBar.y + 100);
            }
        }
    }
}
