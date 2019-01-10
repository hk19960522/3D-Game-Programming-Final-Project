using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SceneItem : MonoBehaviour {

    public enum BreakState
    {
        ALL, 
        EDIT, 
        NONE
    }
    public enum Direction
    {
        FRONT, 
        BACK, 
        LEFT, 
        RIGHT
    }

    // Basic Info
    private Vector3 m_RootPosition;
    [SerializeField]
    private List<Vector3> m_Offset;

    private BreakState m_BreakState;
    private Direction m_Direction;

    [SerializeField]
    private string m_ItemType; 

    public void Start()
    {
        ////m_Offset = new List<Vector3>();
        //m_RootPosition = Vector3.zero;

        ////m_Offset.Add(Vector3.zero);
        //m_BreakState = BreakState.ALL;
        //m_Direction = Direction.FRONT;

        //Test();
    }

    private void Test()
    {
        m_Direction = Direction.FRONT;
        for (int i = -25; i <= 25; i++)
        {
            for (int j = -25; j <= 25; j++)
            {
                if (!AddPosition(new Vector3(i, 0, j)))
                {
                    Debug.Log(i + " " + j + " SAME.");
                }
            }
        }
    }

    /// <summary>
    /// Copy value from ResourceManager.
    /// Usually used in first load.
    /// </summary>
    /// <param name="itemInfo"></param>
    public void Set(SceneItemInfo itemInfo)
    {
        m_ItemType = itemInfo.hashTag;
        m_Offset = new List<Vector3>(itemInfo.offset);
    }

    /// <summary>
    /// Copy value from SceneManager.
    /// Usually used in load scene.
    /// </summary>
    /// <param name="placement"></param>
    public void Set(SceneItemPlacement placement)
    {
        m_RootPosition = placement.position;
        m_BreakState = placement.breakState;
        m_ItemType = placement.hash;

        transform.position = m_RootPosition;
        Rotate(placement.direction);
    }

    /// <summary>
    /// Copy value from other game object.
    /// Usually used in get new object from item pool.
    /// </summary>
    /// <param name="sceneItem"></param>
    public void Set(SceneItem sceneItem)
    {
        m_ItemType = sceneItem.m_ItemType;
        m_Offset = new List<Vector3>(sceneItem.m_Offset);
        m_BreakState = sceneItem.m_BreakState;
        m_RootPosition = sceneItem.m_RootPosition;

        transform.position = m_RootPosition;
        Rotate(sceneItem.m_Direction);

        //Debug.Log(m_ItemType);
        //Debug.Log(m_Offset.Count);
        //Debug.Log(sceneItem.m_Offset.Count);
    }

    public int GetItemSize()
    {
        return m_Offset.Count;
    }

    public void SetPosition(Vector3 pos)
    {
        m_RootPosition.x = (int)pos.x;
        m_RootPosition.y = (int)pos.y;
        m_RootPosition.z = (int)pos.z;

        transform.position = m_RootPosition;
    }

    public Vector3 GetPosition()
    {
        return m_RootPosition;
    }

    public bool GetPosition(int index, out Vector3 pos)
    {
        pos = Vector3.zero;
        if (index >= 0 && index < m_Offset.Count)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (m_Direction == Direction.LEFT)
            {
                rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (m_Direction == Direction.RIGHT)
            {
                rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (m_Direction == Direction.BACK)
            {
                rotation = Quaternion.Euler(0, 180, 0);
            }

            pos = rotation * m_Offset[index] + m_RootPosition;
            return true;
        }
        return false;
    }

    public bool GetPosition(int index, out Vector3 pos, Direction direction)
    {
        pos = Vector3.zero;
        if (index >= 0 && index < m_Offset.Count)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (direction == Direction.LEFT)
            {
                rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (direction == Direction.RIGHT)
            {
                rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (direction == Direction.BACK)
            {
                rotation = Quaternion.Euler(0, 180, 0);
            }

            pos = rotation * m_Offset[index] + m_RootPosition;
            return true;
        }
        return false;
    }

    public List<Vector3> GetAllPosition()
    {
        return m_Offset;
    }

    public bool AddPosition(Vector3 pos)
    {
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        pos.z = (int)pos.z;
        try
        {
            int index = m_Offset.FindIndex(Item => Item.Equals(pos));
            // If the position is not exist
            if (index == -1)
            {
                m_Offset.Add(pos);
                return true;
            }
            return false;
        }
        catch
        {
            m_Offset.Add(pos);
            return true;
        }
    }

    private void Rotate(Direction direction)
    {
        if (direction == Direction.LEFT)
        {
            transform.Rotate(0, -90, 0, Space.Self);
        }
        else if (direction == Direction.RIGHT)
        {
            transform.Rotate(0, 90, 0, Space.Self);
        }
        else if (direction == Direction.BACK)
        {
            transform.Rotate(0, 180, 0, Space.Self);
        }
        m_Direction = direction;
    }

    public void SetRotate(Direction direction)
    {
        if (m_Direction == Direction.LEFT)
        {
            Rotate(Direction.RIGHT);
        }
        else if (m_Direction == Direction.RIGHT)
        {
            Rotate(Direction.LEFT);
        }
        else if (m_Direction == Direction.BACK)
        {
            Rotate(Direction.BACK);
        }
        Rotate(direction);
    }

    public Direction GetDirection()
    {
        return m_Direction;
    }

    public Direction NextDirection(bool isClockwise)
    {
        if (m_Direction == Direction.FRONT)
        {
            return isClockwise ? Direction.RIGHT : Direction.LEFT;
        }

        if (m_Direction == Direction.RIGHT)
        {
            return isClockwise ? Direction.BACK : Direction.FRONT;
        }

        if (m_Direction == Direction.BACK)
        {
            return isClockwise ? Direction.LEFT : Direction.RIGHT;
        }

        return isClockwise ? Direction.FRONT : Direction.BACK;
    }

    public string GetItemType()
    {
        return m_ItemType;
    }
}
