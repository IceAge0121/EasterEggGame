using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    [SerializeField] private Transform _highlight;
    [SerializeField] private Transform _selection;
    [SerializeField] private float _outlineWidth = 7.0f;
    [SerializeField] private float _maxSelectionDistance = 9.0f;
    private RaycastHit raycastHit;

    private void Update()
    {
        // Every frame, turn of the outline of the hit game object and lose our 
        // reference because we have to check it constantly.
        if (_highlight != null)
        {
            _highlight.gameObject.GetComponent<Outline>().enabled = false;
            _highlight = null;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        //if (!EventSystem.current.IsPointerOverGameObject() &&
        //    Physics.Raycast(ray, out raycastHit))


        // Check if our raycast hits an object.
        if (Physics.Raycast(ray, out raycastHit) && raycastHit.distance < _maxSelectionDistance)
        {
            _highlight = raycastHit.transform;
            //Debug.DrawLine(Camera.main.transform.position, raycastHit.point);
            if (_highlight.CompareTag("Selectable") && _highlight != _selection)
            {
                //Debug.Log($"Passed tag comparison, hit {_highlight}.");
                // If the hit object has an outline component set it to turn on.
                if (_highlight.gameObject.GetComponent<Outline>() != null)
                {
                    _highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                // If it doesn't, we add the outline component and enable it. 
                else
                {
                    Outline outline = _highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    _highlight.gameObject.GetComponent<Outline>().OutlineWidth = _outlineWidth;
                }
            }
            else
            {
                _highlight = null;
            }
        }

        // If in the current frame we pushed on the left click.
        if (Input.GetMouseButtonDown(0))
        {
            // If we have something we can highlight as we hover over.
            if (_highlight)
            {
                // If we don't have anything selected at the moment, be sure to clear 
                // our selection first.
                if (_selection != null)
                {
                    _selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                // Then we reassign our selection to our highlighted object.
                _selection = _highlight;
                _selection.gameObject.GetComponent<Outline>().enabled = true;
                _highlight = null;
            }
            else
            {
                if (_selection)
                {
                    _selection.gameObject.GetComponent<Outline>().enabled = false;
                    _selection = null;
                }
            }
        }
    }
}
