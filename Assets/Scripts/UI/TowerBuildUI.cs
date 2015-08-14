using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

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
        this.UpdateCanBuild();
    }

    void OnEnable()
    {
        ResourceManager.PropertyChanged += this.ResourcesPropertyChangedHandler;
    }

    void OnDisable()
    {
        ResourceManager.PropertyChanged -= this.ResourcesPropertyChangedHandler;
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

    private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
        return (propertyExpression.Body as MemberExpression).Member.Name;
    }

    private void Initialize()
    {
        if (_prefab != null)
        {
            _nameText.text = _prefab.Name;
            _previewImage.sprite = _prefab.PreviewImage;
            _costText.text = _prefab.Cost.ToString();
        }
    }
    
    private void UpdateCanBuild()
    {
        _button.interactable = this.Prefab == null || ResourceManager.Money >= this.Prefab.Cost;
    }

    #endregion

    #region Event Handlers

    private void ResourcesPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == GetPropertyName(() => ResourceManager.Money))
        {
            this.UpdateCanBuild();
        }
    }

    #endregion
}
