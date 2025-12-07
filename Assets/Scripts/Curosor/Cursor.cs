using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class Cursor : MonoBehaviour
{
    [SerializeField] private SlotManager _slotManager;
    [SerializeField] private Slot _cursorSlot;
    [SerializeField] private CursorView _cursorView;

    private Slot _selectedSlot;
    private List<Slot> _draggedSlotList = new List<Slot>();
    private int _distributeItemIndex;
    private int _preDistributeCount;

    // ----- Life Cycle Methods -----
    private void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        this.transform.position = new Vector3(worldPos.x, worldPos.y, -5f);

        _selectedSlot = Physics2D.OverlapPoint(this.transform.position)?.GetComponent<Slot>();
        _cursorView.DisplayItemName(_selectedSlot?.ItemObject ?? null);
    }

    // ----- Public Methods -----

    public void OnLeftSingleClick()
    {
        if (!HasSelectedSlot()) return;

        Take();
    }

    public void OnLeftDoubleClick()
    {
        if (!HasSelectedSlot()) return;

        CollectSameItem();
    }

    public void OnLeftHoldInit()
    {
        ResetDistributeVariable();

        if (!HasSelectedSlot()) return;
        if (!_cursorSlot.HasItem()) return;
        
        _distributeItemIndex = _cursorSlot.ItemObject.ItemIndex;
        _preDistributeCount = _cursorSlot.ItemObject.StackCount;
    }
    public void OnLeftHolding()
    {
        if (!HasSelectedSlot()) return;
        if (_distributeItemIndex == -1) return;

        if (!_draggedSlotList.Contains(_selectedSlot) && ( !_selectedSlot.HasItem() || _selectedSlot.ItemObject.ItemIndex == _distributeItemIndex) )
        {
            _draggedSlotList.Add(_selectedSlot);
        }

        if (_draggedSlotList.Count != 0)
        {
            DistributeEvenly();
        }
    }

    public void OnLeftShiftSingleClick()
    {
        if (!HasSelectedSlot()) return;
        if (!_selectedSlot.HasItem()) return;

        QuickShiftItem();
    }

    public void OnRightSingleClick()
    {
        if (!HasSelectedSlot()) return;

        if (_cursorSlot.HasItem())
        {
            PlaceOne();
        }
        else
        {
            TakeHalf();
        }
    }

    public void OnRightHoldInit()
    {
        ResetDistributeVariable();

        if (!HasSelectedSlot()) return;
        if (!_cursorSlot.HasItem()) return;
        
        _distributeItemIndex = _cursorSlot.ItemObject.ItemIndex;
        _preDistributeCount = _cursorSlot.ItemObject.StackCount;
    }
    public void OnRightHolding()
    {
        if (!HasSelectedSlot()) return;

        if (!_draggedSlotList.Contains(_selectedSlot) && ( !_selectedSlot.HasItem() || _selectedSlot.ItemObject.ItemIndex == _distributeItemIndex) )
        {
            _draggedSlotList.Add(_selectedSlot);
        }

        if (_draggedSlotList.Count != 0)
        {
            DistributeOneByOne();
        }
    }

    // ----- Private Methods -----

    private void Take()
    {
        if (HasSameItem() && _selectedSlot.ItemObject.CanAddStackCount())
        {
            while (_cursorSlot.HasItem() && _selectedSlot.ItemObject.CanAddStackCount())
            {
                _selectedSlot.SetItemCount(_selectedSlot.ItemObject.StackCount + 1);
                _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - 1);
            }
        }
        else
        {
            ExchangeItemObject();
        }   
    }

    private void CollectSameItem()
    {
        if (!_cursorSlot.HasItem() && !_selectedSlot.HasItem()) return;
        else if (!_cursorSlot.HasItem())
        {
            ExchangeItemObject();
        }

        int collectItemIndex = _cursorSlot.ItemObject.ItemIndex;

        if (HasSameItem())
        {
            while (_selectedSlot.ItemObject != null && _cursorSlot.ItemObject.CanAddStackCount())
            {
                _selectedSlot.SetItemCount(_selectedSlot.ItemObject.StackCount - 1);
                _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount + 1);
            }
        }

        List<Slot> slotList = _slotManager.InventorySlots;
        slotList.AddRange(_slotManager.HotBarSlots);
        foreach (Slot slot in slotList)
        {
            if (!_cursorSlot.ItemObject.CanAddStackCount()) break;
            if (!slot.HasItem()) continue;

            if (slot.ItemObject.ItemIndex == _cursorSlot.ItemObject.ItemIndex)
            {
                while (slot.ItemObject != null && _cursorSlot.ItemObject.CanAddStackCount())
                {
                    slot.SetItemCount(slot.ItemObject.StackCount - 1);
                    _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount + 1);
                }
            }
        }
    }


    private void DistributeEvenly()
    {
        if (!_cursorSlot.HasItem()) _cursorSlot.ItemObject = new ItemObject(_distributeItemIndex, _preDistributeCount);
        else _cursorSlot.SetItemCount(_preDistributeCount);

        foreach (Slot slot in _draggedSlotList)
        {
            if (slot.DistributedCount != 0)
            {
                slot.SetItemCount(slot.ItemObject.StackCount - slot.DistributedCount);
                slot.DistributedCount = 0;
            }
        }

        int distributeCount = Mathf.FloorToInt((float)_preDistributeCount / _draggedSlotList.Count);
        distributeCount = distributeCount > 0 ? distributeCount : 1;
        foreach (Slot slot in _draggedSlotList)
        {
            if (!_cursorSlot.HasItem()) break;

            if (!slot.HasItem())
            {
                slot.ItemObject = new ItemObject(_distributeItemIndex, distributeCount);
                _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - distributeCount);
                slot.DistributedCount = distributeCount;
            }
            else
            {
                int addCount = 0;
                for (int j = 0; j < distributeCount; j++)
                {
                    if (slot.ItemObject.CanAddStackCount())
                    {
                        slot.SetItemCount(slot.ItemObject.StackCount + 1);
                        _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - 1);
                        addCount++;
                    }
                }
                slot.DistributedCount = addCount;
            }
        }
    }

    private void DistributeOneByOne()
    {
        if (!_cursorSlot.HasItem()) _cursorSlot.ItemObject = new ItemObject(_distributeItemIndex, _preDistributeCount);
        else _cursorSlot.SetItemCount(_preDistributeCount);

        foreach (Slot slot in _draggedSlotList)
        {
            if (slot.DistributedCount != 0)
            {
                slot.SetItemCount(slot.ItemObject.StackCount - slot.DistributedCount);
                slot.DistributedCount = 0;
            }
        }

        foreach (Slot slot in _draggedSlotList)
        {
            if (!_cursorSlot.HasItem()) break;

            if (!slot.HasItem())
            {
                slot.ItemObject = new ItemObject(_distributeItemIndex, 1);
                _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - 1);
                slot.DistributedCount = 1;
            }
            else
            {
                if (slot.ItemObject.CanAddStackCount())
                {
                    slot.SetItemCount(slot.ItemObject.StackCount + 1);
                    _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - 1);
                    slot.DistributedCount = 1;
                }
            }
        }
    }

    private void ResetDistributeVariable()
    {
        _distributeItemIndex = -1;
        _preDistributeCount = 0;
        foreach (Slot slot in _draggedSlotList)
        {
            slot.DistributedCount = 0;
        }
        _draggedSlotList.Clear();
    }

    public void PlaceOne()
    {
        if (HasSameItem())
        {
            if (!_selectedSlot.ItemObject.CanAddStackCount()) return;

            _selectedSlot.SetItemCount(_selectedSlot.ItemObject.StackCount + 1);
            _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - 1);
        }
        else if (!_selectedSlot.HasItem())
        {
            _selectedSlot.ItemObject = new ItemObject(_cursorSlot.ItemObject.ItemIndex, 1);
            _cursorSlot.SetItemCount(_cursorSlot.ItemObject.StackCount - 1);
        }
    }

    private void TakeHalf()
    {
        if (!_selectedSlot.HasItem()) return;

        if (_selectedSlot.ItemObject.IsEquipment())
        {
            ExchangeItemObject();
        }
        else
        {
            _cursorSlot.ItemObject = new ItemObject(_selectedSlot.ItemObject.ItemIndex, _selectedSlot.TakeHalf());
        }
    }

    private void QuickShiftItem()
    {
        List<Slot> targetSlots = new List<Slot>();
        if (_slotManager.IsInventorySlot(_selectedSlot)) targetSlots = _slotManager.HotBarSlots;
        else if (_slotManager.IsHotBarSlot(_selectedSlot)) targetSlots = _slotManager.InventorySlots;
        
        foreach (Slot targetSlot in targetSlots)
        {
            if (!_selectedSlot.HasItem()) break;

            if (!targetSlot.HasItem())
            {
                targetSlot.ItemObject = new ItemObject(_selectedSlot.ItemObject.ItemIndex, 1);
                _selectedSlot.SetItemCount(_selectedSlot.ItemObject.StackCount - 1);
            }
            else if (targetSlot.ItemObject.ItemIndex !=  _selectedSlot.ItemObject.ItemIndex) continue;

            while (targetSlot.ItemObject.CanAddStackCount() && _selectedSlot.HasItem())
            {
                targetSlot.SetItemCount(targetSlot.ItemObject.StackCount + 1);
                _selectedSlot.SetItemCount(_selectedSlot.ItemObject.StackCount - 1);
            }
        }
    }

    private void ExchangeItemObject()
    {
        ItemObject tempItemObject = _selectedSlot.ItemObject;
        _selectedSlot.ItemObject = _cursorSlot.ItemObject;
        _cursorSlot.ItemObject = tempItemObject;
    }

    private bool HasSelectedSlot()
    {
        return _selectedSlot != null;
    }

    private bool HasSameItem()
    {
        return _cursorSlot.HasItem() && _selectedSlot.HasItem() && _cursorSlot.ItemObject.ItemIndex == _selectedSlot.ItemObject.ItemIndex;
    }
}