using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public interface ISelectable
{
    Camera mainCamera { get; set;}
    OutlineEffect outEffect { get; set; }
    EventSystem eventSystem { get; set; }
    Button button { get; set; }
    List<Outline> outlines { get; set; }
    bool mouseOver { get; set; }
    bool blinking { get; set; }
    bool unselecting { get; set; }
    void SelectObject(GameObject selectedObject);
    void PlaceOutlineOnMesh(Transform meshRenderer);
    void SnapRopeButtonsPanel();
    IEnumerator BlinkColor(OutlineEffect outEffect, List<Outline> outlines, int color, int repeating);
    Color GetColor(OutlineEffect outEffect, int color);
    void ChangeColor(OutlineEffect outEffect, int color, Color newColor);
    IEnumerator Unselect();
    bool DetectButtonOver();
}
