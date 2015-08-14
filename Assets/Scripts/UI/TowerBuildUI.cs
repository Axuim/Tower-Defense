using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;

public class TowerBuildUI : MonoBehaviour
{
    #region Private Properties

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _previewImage;
    [SerializeField]
    private Text _costText;

    #endregion

    #region Public Properties

    public Tower Prefab { get; set; }

    #endregion

    #region Events

    public event EventHandler Clicked;

    #endregion

    #region MonoBehaviour

    void Start()
    {

    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    #endregion

    #region Public Methods

    public void Click()
    {
        if (this.Clicked != null)
        {
            this.Clicked(this, null);
        }
    }

    #endregion

    #region Private Methods

    private void HandleResourcesPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        
    }

    #endregion
}
