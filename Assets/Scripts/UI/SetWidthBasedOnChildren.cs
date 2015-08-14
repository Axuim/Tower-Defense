using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(RectTransform))]
public class SetWidthBasedOnChildren : MonoBehaviour
{
    #region Private Properties

    private RectTransform _rectTransform;

    [SerializeField]
    private float _widthPerChild = 100.0f;

    #endregion

    #region MonoBehaviour
    
    void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    #endregion

    #region Public Methods

    public void UpdateSize()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.childCount * _widthPerChild, _rectTransform.sizeDelta.y);
    }

    #endregion
}
