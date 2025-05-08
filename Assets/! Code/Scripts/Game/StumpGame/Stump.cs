using UnityEngine;


public class Stump : MonoBehaviour
{
    [Header("Highlighting")]
        public bool CanBeInteracted {get; set;} = false;
        public bool IsRune;
        [SerializeField] private Material highlightMaterial;
    
    [Header("Outline Coloring")]
        private Material clrmat;
        public Material ColorMaterial {
            get {
                return clrmat;
            } 
            set {
                clrmat = value;
                if (clrmat == null)
                    this.transform.Find("Outline").gameObject.SetActive(false);
                else {
                    this.transform.Find("Outline").GetComponent<SpriteRenderer>().material = clrmat;
                    this.transform.Find("Outline").gameObject.SetActive(true);
                }
            }
        }
    
    private void Start() {
        if (this.transform.Find("Rune"))
            IsRune = true;
        else
            IsRune = false;
        
        if (IsRune) {
            ColorMaterial = this.transform.Find("Outline").GetComponent<SpriteRenderer>().material;
        }
    }

    private void OnMouseEnter() {
        if (CanBeInteracted) {
            this.transform.Find("Outline").GetComponent<SpriteRenderer>().material = highlightMaterial;
            this.transform.Find("Outline").gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        ColorMaterial = clrmat;
    }

    private void OnMouseDown()
    {
        this.transform.parent.parent.GetComponent<StumpGameManager>().StumpClicked(this.transform.GetSiblingIndex());
    }
}
