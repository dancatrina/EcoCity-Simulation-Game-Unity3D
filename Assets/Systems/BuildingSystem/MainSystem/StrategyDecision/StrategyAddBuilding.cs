using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class StrategyAddBuilding : StrategyBuildingJob
{
    private Transform transformBuildingAnimation;
    private GameObject visualBuildingAnimation;

    private ABuilding infoBuilding;

    private float pret;

    public StrategyAddBuilding() { }
    public StrategyAddBuilding(BuildingType buildingType)
    {
        this.buildingType = buildingType;
        visualBuildingAnimation = new GameObject("VisualBuildingAnimation");

        refreshVisual();
    }

    public StrategyAddBuilding(BuildingType buildingType, ABuilding infoBuilding)
    {
        this.buildingType = buildingType;
        this.infoBuilding = infoBuilding;


        visualBuildingAnimation = new GameObject("VisualBuildingAnimation");
        refreshVisual();
    }

    public StrategyAddBuilding(BuildingType buildingType, ABuilding infoBuilding, float pret)
    {
        this.buildingType = buildingType;
        this.infoBuilding = infoBuilding;
        this.pret = pret;

        visualBuildingAnimation = new GameObject("VisualBuildingAnimation");
        refreshVisual();
    }


    public override void doJob()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                destroyGarbage();
                pointTo.setStrategyJob(new StrategyBuildingInfo());
            }
        }
        else
        {
            if (visualBuildingAnimation != null)
            {
                Vector3 targetPosition = getMouseWorldSnappedPositionWithLayermask(7);
                targetPosition.y = 0;
                visualBuildingAnimation.transform.position = Vector3.Lerp(visualBuildingAnimation.transform.position, targetPosition, Time.deltaTime * 15f);
                visualBuildingAnimation.transform.rotation = Quaternion.Lerp(visualBuildingAnimation.transform.rotation, getPlacedObjectRotation(), Time.deltaTime * 15f);
            }


            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Mouse3D.getMouseWorldPositionWithLayerMask(7);

                pointTo.getGrid().getPositon(mousePosition, out int x, out int z);

                if (x >= 0 && x < pointTo.GridWidth && z < pointTo.GridHeight & z >= 0)
                {

                    Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                    placedObjectOrigin = pointTo.getGrid().validateGridPosition(placedObjectOrigin);

                    List<Vector2Int> gridPositionList = buildingType.getGridPositionList(placedObjectOrigin, direction);
                    bool canBuild = true;

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        if (!pointTo.getGrid().getGridObject(gridPosition.x, gridPosition.y).canBuild())
                        {

                            canBuild = false;
                            break;
                        }
                    }

                    if (canBuild)
                    {
                        if ((EconomyManager.getInstance().baniOras - pret) > 0)
                        {
                            Vector2Int rotationOffset = buildingType.getRotationOffset(direction);
                            Vector3 placedObjectWorldPosition = pointTo.getGrid().getWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * pointTo.getGrid().CellSize;

                            Building placedObject = buildingType.Create(placedObjectWorldPosition, placedObjectOrigin, direction);
                            EconomyManager.getInstance().BaniOras = EconomyManager.getInstance().BaniOras - pret;



                            placedObject.HoldingBuilding = infoBuilding.clone();
                            EconomyManager.getInstance().addBasedOnType(placedObject.HoldingBuilding);
                            setLayerRecursive(placedObject.gameObject, 6);

                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                pointTo.getGrid().getGridObject(gridPosition.x, gridPosition.y).setBuilding(placedObject);
                            }
                            eventObjectPlaced();
                        }
                        else
                        {
                            UtilsClass.CreateWorldTextPopup("Fonduri insuficiente", mousePosition);
                        }
                    }
                    else
                    {
                        UtilsClass.CreateWorldTextPopup("Nu se poate construi aici", mousePosition);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                direction = BuildingType.GetNextDir(direction);
            }
        }
    }

    void refreshVisual()
    {
        if (buildingType != null)
        {
            transformBuildingAnimation = buildingType.createAnimation();
            transformBuildingAnimation.parent = visualBuildingAnimation.transform;
            transformBuildingAnimation.localPosition = Vector3.zero;
            transformBuildingAnimation.localEulerAngles = Vector3.zero;
            setLayerRecursive(visualBuildingAnimation, 6);
        }
    }

    private void setLayerRecursive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            setLayerRecursive(child.gameObject, layer);
        }
    }

    private void clearVisual()
    {
        if(visualBuildingAnimation != null) GameObject.Destroy(visualBuildingAnimation);
        visualBuildingAnimation = null;
        transformBuildingAnimation=null;

    }

    public override void destroyGarbage()
    {
        clearVisual();
    }
    
}