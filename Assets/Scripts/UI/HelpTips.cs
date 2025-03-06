using UnityEngine;

public class HelpTips : MonoBehaviour
{
    [SerializeField] private string Path;
    private GameObject tip;
    private Tip tipScr;
    private TMPro.TMP_Text info;
    private TMPro.TMP_Text nameOfInteract;
    private CameraMove cam;

    private void Awake()
    {
        GameObject prefab = Resources.Load<GameObject>(Path);
        tip = Instantiate(prefab);
        tipScr = tip.GetComponent<Tip>();
        info = tip.gameObject.transform.GetChild(0).GetComponentInChildren<TMPro.TMP_Text>();
        nameOfInteract = tip.gameObject.transform.GetChild(1).GetComponentInChildren<TMPro.TMP_Text>();
        tip.SetActive(false);
        cam = Camera.main.GetComponent<CameraMove>();
    }

    private void FixedUpdate()
    {
        ShowTip();   
    }

    private void ShowTip()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.MouseOnWorldScreen(), out hit, 25f))
        {
            IDescription cell = hit.collider.GetComponent<IDescription>();

            if (cell != null)
            {
                tip.SetActive(true);
                Vector3 p = cam.MouseOnViewScreen();
                tipScr.Move(hit.point, hit.collider.gameObject.transform.rotation);
                info.text = cell.Description;
                nameOfInteract.text = cell.NameOfInteract;
            }
            
        }
        else tip.SetActive(false);
    }
}
