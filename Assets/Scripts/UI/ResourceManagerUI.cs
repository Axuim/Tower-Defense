using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

public class ResourceManagerUI : MonoBehaviour
{
    #region Private Properties

    [SerializeField]
    private Text _moneyText;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.UpdateMoneyText();
    }

    void OnEnable()
    {
        ResourceManager.PropertyChanged += this.ResourceManagerPropertyChangedHandler;
    }

    void OnDisable()
    {
        ResourceManager.PropertyChanged -= this.ResourceManagerPropertyChangedHandler;
    }

    #endregion

    #region Private Methods

    private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
        return (propertyExpression.Body as MemberExpression).Member.Name;
    }

    private void UpdateMoneyText()
    {
        _moneyText.text = ResourceManager.Money.ToString();
    }

    #endregion

    #region Event Handlers

    private void ResourceManagerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == GetPropertyName(() => ResourceManager.Money))
        {
            this.UpdateMoneyText();
        }
    }

    #endregion
}
