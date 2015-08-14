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
    private Text _nameText;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _previewImage;
    [SerializeField]
    private Text _costText;

    #endregion

    #region Public Properties

    private Tower _prefab;
    public Tower Prefab
    {
        get
        {
            return _prefab;
        }
        set
        {
            if (value != _prefab)
            {
                _prefab = value;
                this.Initialize();
            }
        }
    }

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

    private void Initialize()
    {
        if (_prefab != null)
        {
            _nameText.text = _prefab.Name;
            _previewImage.sprite = _prefab.PreviewImage;
            _costText.text = _prefab.Cost.ToString();
        }
    }

    private void HandleResourcesPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        
    }

    #endregion
}
