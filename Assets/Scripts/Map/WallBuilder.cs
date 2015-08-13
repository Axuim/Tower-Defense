using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WallBuilder : MonoBehaviour
{
    #region Private Properties

    private BuildingWall _buildingWall;
    private int _lastX;
    private int _lastZ;

    [SerializeField]
    private BuildingWall _buildingWallPrefab;
    [SerializeField]
    private Wall _wallPrefab;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        _buildingWall = GameObject.Instantiate<BuildingWall>(_buildingWallPrefab);
        _buildingWall.transform.parent = this.transform;
        _buildingWall.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        _buildingWall.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        _buildingWall.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayerMask))
        {
            var localHitPosition = this.transform.InverseTransformPoint(hit.point);
            int x = Mathf.Clamp(Mathf.FloorToInt(localHitPosition.x), 0, _width - 1);
            int z = Mathf.Clamp(Mathf.FloorToInt(localHitPosition.z), 0, _height - 1);
            
            if (x != _lastX || z != _lastZ)
            {
                _lastX = x;
                _lastZ = z;

                _buildingWall.transform.localPosition = new Vector3(x + Wall.HALF_SIZE, localHitPosition.y, z + Wall.HALF_SIZE);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (_buildingWall.CanBuildAtPosition())
            {
                var wall = GameObject.Instantiate<Wall>(_wallPrefab);
                wall.transform.parent = Map.Singleton.InnerWallsParent;
                wall.transform.position = _buildingWall.transform.position;
            }
        }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion
}
