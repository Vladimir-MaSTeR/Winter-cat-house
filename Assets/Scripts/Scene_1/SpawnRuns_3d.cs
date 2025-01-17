using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnRuns_3d : MonoBehaviour {


    #region ���������� ��������
    [Header("�������")]
    
    [Tooltip("����� ����� ������� �������� ������ ������� ��� � ���")]
    [SerializeField] 
    private float _timeRespOneColumn = 10f;

    [Tooltip("����� ����� ������� �������� ������ ������� ��� � ���")]
    [SerializeField] 
    private float _timeRespTwoColumn = 10f;

    [Tooltip("����� ����� ������� �������� ������ ������� ��� � ���")]
    [SerializeField] 
    private float _timeRespThreeColumn = 10f;

    [Tooltip("����� ����� ������� �������� ��������� ������� ��� � ���")]
    [SerializeField] 
    private float _timeRespFourColumn = 10f;

    [Space(20)] // ������ � ���������� ����� ������
    #endregion

    #region ���������� ��� ������
    [Header("�����")]

    [Tooltip("��� ������ � ������� ���������� ����")]
    [SerializeField] 
    private GameObject[] _allSlots;

    [Tooltip("������ ������� ����")]
    [SerializeField] 
    private GameObject[] _oneColumSlots;

    [Tooltip("������ ������� ����")]
    [SerializeField] 
    private GameObject[] _twoColumSlots;

    [Tooltip("������ �������� ����")]
    [SerializeField] 
    private GameObject[] _threeColumSlots;

    [Tooltip("������ ���������� ����")]
    [SerializeField] 
    private GameObject[] _fourColumSlots;

    [Space(20)] // ������ � ���������� ����� ������
    #endregion

    #region ���������� ��� ���
    [Header("Items")]

    [Tooltip("���� ������� ������")]
    [SerializeField] 
    private GameObject[] _items_1lv;

    [Tooltip("���� ������� ������")]
    [SerializeField] 
    private GameObject[] _items_2lv;

    [Tooltip("���� �������� ������")]
    [SerializeField] 
    private GameObject[] _items_3lv;

    [Tooltip("���� ���������� ������")]
    [SerializeField]
    private GameObject[] _items_4lv;

    [Space(20)] // ������ � ���������� ����� ������
    #endregion

    #region ��� ������� SelectedAndMeargItem
    [Header("������ �� ������� ������ SelectedAndMeargItem")]

    [Tooltip("��� ������� SelectedAndMeargItem")]
    [SerializeField]
    private SelectedAndMeargItem _selectedAndMeargItem;
    #endregion

    private float _currentTimeRespOneColumn;
    private float _currentTimeRespTwoColumn;
    private float _currentTimeRespThreeColumn;
    private float _currentTimeRespFourColumn;

    private bool _startTimer = true;
    private bool _holdColumn = false;

    private bool _holdOneColumn = false;
    private bool _holdTwoColumn = false;
    private bool _holdThreeColumn = false;
    private bool _holdFourColumn = false;

    private int _currentSlotIndex;
    private int _currentItemIndex;
    private float _currentTimeRessItem;


    private List<GameObject> _combinedList;

    private void Start() {
        _combinedList = new List<GameObject>();
        _combinedList.AddRange(_items_1lv);
        _combinedList.AddRange(_items_2lv);
        _combinedList.AddRange(_items_3lv);
        _combinedList.AddRange(_items_4lv);

        _currentTimeRespOneColumn = _timeRespOneColumn;
        _currentTimeRespTwoColumn = _timeRespTwoColumn;
        _currentTimeRespThreeColumn = _timeRespThreeColumn;
        _currentTimeRespFourColumn = _timeRespFourColumn;

        _currentSlotIndex = Random.Range(0, _allSlots.Length);

        ReloadItems();
    }

    private void FixedUpdate() {
        if(_startTimer) {
            SpawnOneColumnRuns();
            SpawnTwoColumnRuns();
            SpawnThreeColumnRuns();
            SpawnFourColumnRuns();
        }
    }

    private void Update() {
        if(_startTimer) {
            //����� �����3
            Mearg3Column(_oneColumSlots);
            Mearg3Column(_twoColumSlots);
            Mearg3Column(_threeColumSlots);
            Mearg3Column(_fourColumSlots);

            Mearg3Row(0);
            Mearg3Row(1);
            Mearg3Row(2);
            Mearg3Row(3);
            Mearg3Row(4);

            //MoveSelecteSlot(_allSlots);
        }       
    }

    private void OnEnable() {
        ButtonsEvents.onSaveResouces += SaveItems;

        MeargGameEvents.onGetCurrentTimeSpawnOldColumn += GetCurrentTimer;
        MeargGameEvents.onSetTimeToSpawnRuns += SetCurrentTimeOldColumn;
        EventsResources.onSpawnItemToSlot += SpawnItemToSlot;

        MeargGameEvents.onTiefRuns += TiefRuns;
        MeargGameEvents.onRandomRuns += RandomRuns;
        MeargGameEvents.onAiceRuns += HoldColumn;
        MeargGameEvents.onFalseHoldColumn += SetFalseHoldColumn;

        MeargGameEvents.onSetStartTimerTrue += SetStartTimerTrue;
    }

    private void OnDisable() {
        ButtonsEvents.onSaveResouces -= SaveItems;

        MeargGameEvents.onGetCurrentTimeSpawnOldColumn -= GetCurrentTimer;
        MeargGameEvents.onSetTimeToSpawnRuns -= SetCurrentTimeOldColumn;
        EventsResources.onSpawnItemToSlot -= SpawnItemToSlot;

        MeargGameEvents.onTiefRuns -= TiefRuns;
        MeargGameEvents.onRandomRuns -= RandomRuns;
        MeargGameEvents.onAiceRuns -= HoldColumn;
        MeargGameEvents.onFalseHoldColumn -= SetFalseHoldColumn;
        
        MeargGameEvents.onSetStartTimerTrue -= SetStartTimerTrue;
    }

    /// <summary>
    /// ����� ��������� �������
    /// </summary>
    private void HoldColumn() {
        var currentSpawnPointHoldSpider = MeargGameEvents.onGetCurrentSpawnPointHoldSpider?.Invoke();

        if(currentSpawnPointHoldSpider == 0) {
            _holdOneColumn = true;
        } else if(currentSpawnPointHoldSpider == 1) {
            _holdTwoColumn = true;
        } else if(currentSpawnPointHoldSpider == 2) {
            _holdThreeColumn = true;
        } else if(currentSpawnPointHoldSpider == 3) {
            _holdFourColumn = true;
        }
    }

    /// <summary>
    /// ����� ���������� �������
    /// </summary>
    private void SetFalseHoldColumn(int column) {
        if(column == 0) {
            _holdOneColumn = false;
            SetCurrentTimeOldColumn(_currentTimeRespTwoColumn);
        } else if(column == 1) {
            _holdTwoColumn = false;
            SetCurrentTimeOldColumn(_currentTimeRespOneColumn);
        } else if(column == 2) {
            _holdThreeColumn = false;
            SetCurrentTimeOldColumn(_currentTimeRespOneColumn);
        } else if(column == 3) {
            _holdFourColumn = false;
            SetCurrentTimeOldColumn(_currentTimeRespOneColumn);
        }
    }

    /// <summary>
    /// ����� 3 � ���, ������� �� ���� ��� ������
    /// </summary>
    private void Mearg3Column(GameObject[] columSlots) {
        //�������� ����� ��� ������ 3 � ���

        GameObject object0 = null;
        GameObject object1 = null;
        GameObject object2 = null;
        GameObject object3 = null;
        GameObject object4 = null;

        if(columSlots[0].GetComponentInChildren<CanvasGroup>() != null) {
            //MeargGameEvents.onStartMeargThree?.Invoke(_oneColumSlots[0].GetComponentInChildren<CanvasGroup>().gameObject);

            object0 = columSlots[0].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(columSlots[1].GetComponentInChildren<CanvasGroup>() != null) {
            //MeargGameEvents.onStartMeargThree?.Invoke(_oneColumSlots[1].GetComponentInChildren<CanvasGroup>().gameObject);
            object1 = columSlots[1].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(columSlots[2].GetComponentInChildren<CanvasGroup>() != null) {
            //MeargGameEvents.onStartMeargThree?.Invoke(_oneColumSlots[2].GetComponentInChildren<CanvasGroup>().gameObject);
            object2 = columSlots[2].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(columSlots[3].GetComponentInChildren<CanvasGroup>() != null) {
            //MeargGameEvents.onStartMeargThree?.Invoke(_oneColumSlots[3].GetComponentInChildren<CanvasGroup>().gameObject);
            object3 = columSlots[3].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(columSlots[4].GetComponentInChildren<CanvasGroup>() != null) {
            //MeargGameEvents.onStartMeargThree?.Invoke(_oneColumSlots[4].GetComponentInChildren<CanvasGroup>().gameObject);
            object4 = columSlots[4].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        // 5 � ���
        if(object0 != null && object1 != null && object2 != null && object3 != null && object4 != null) {
            if(object0.tag == object1.tag && object0.tag == object2.tag && object0.tag == object3.tag && object0.tag == object4.tag
                && object1.tag == object2.tag && object1.tag == object3.tag && object1.tag == object4.tag
                && object2.tag == object3.tag && object2.tag == object4.tag
                && object3.tag == object4.tag) {
                Debug.Log("5 � ��� �� ���������");
                _startTimer = false;
                
                AddResouces(object0.tag);
                AddResouces(object1.tag);
                AddResouces(object2.tag);
                AddResouces(object3.tag);
                AddResouces(object4.tag);
                
                // Destroy(object1);
                // Destroy(object2);
                // Destroy(object3);
                // Destroy(object4);

                object0.GetComponentInChildren<Animator>().SetBool("Del", true);
                object1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object3.GetComponentInChildren<Animator>().SetBool("Del", true);
                object4.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(4);
                return;
            }
        }

        // 4 � ���
        if(object0 != null && object1 != null && object2 != null && object3 != null) {
            if(object0.tag == object1.tag && object0.tag == object2.tag && object0.tag == object3.tag
                && object1.tag == object2.tag && object1.tag == object3.tag
                && object2.tag == object3.tag) {
                Debug.Log("4 � ��� �� ���������");
                _startTimer = false;
                
                AddResouces(object0.tag);
                AddResouces(object1.tag);
                AddResouces(object2.tag);
                AddResouces(object3.tag);
                
                // Destroy(object1);
                // Destroy(object2);
                // Destroy(object3);
                // Destroy(object4);

                object0.GetComponentInChildren<Animator>().SetBool("Del", true);
                object1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object3.GetComponentInChildren<Animator>().SetBool("Del", true);
                
                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(3);
                return;
            }
        }

        if(object1 != null && object2 != null && object3 != null && object4 != null) {
            if(object1.tag == object2.tag && object1.tag == object3.tag && object1.tag == object4.tag
                && object2.tag == object3.tag && object2.tag == object4.tag
                && object3.tag == object4.tag) {
                Debug.Log("4 � ��� �� ���������");
                _startTimer = false;
                
                AddResouces(object1.tag);
                AddResouces(object2.tag);
                AddResouces(object3.tag);
                AddResouces(object4.tag);

                // Destroy(object1);
                // Destroy(object2);
                // Destroy(object3);
                // Destroy(object4);
                
                object1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object3.GetComponentInChildren<Animator>().SetBool("Del", true);
                object4.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(3);
                return;
            }
        }

        // 3 � ���
        if(object0 != null && object1 != null && object2 != null) {
            if(object0.tag == object1.tag && object0.tag == object2.tag && object1.tag == object2.tag) { //3 � ���
                Debug.Log("3 � ��� �� ���������");
                _startTimer = false;

                AddResouces(object0.tag);
                AddResouces(object1.tag);
                AddResouces(object2.tag);

                // Destroy(object0);
                // Destroy(object1);
                // Destroy(object2);
                
                object0.GetComponentInChildren<Animator>().SetBool("Del", true);
                object1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object2.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(2);
                return;
            }
        }

        if(object1 != null && object2 != null && object3) {
            if(object1.tag == object2.tag && object1.tag == object3.tag && object2.tag == object3.tag) {
                Debug.Log("3 � ��� �� ���������");
                _startTimer = false;

                AddResouces(object1.tag);
                AddResouces(object2.tag);
                AddResouces(object3.tag);

                // Destroy(object1);
                // Destroy(object2);
                // Destroy(object3);
                
                object1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object3.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(2);
                return;
            }
        }

        if(object2 != null && object3 != null && object4 != null) {
            if(object2.tag == object3.tag && object2.tag == object4.tag && object3.tag == object4.tag) {
                Debug.Log("3 � ��� �� ���������");
                _startTimer = false;

                AddResouces(object2.tag);
                AddResouces(object3.tag);
                AddResouces(object4.tag);

                // Destroy(object2);
                // Destroy(object3);
                // Destroy(object4);
                
                object2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object3.GetComponentInChildren<Animator>().SetBool("Del", true);
                object4.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(2);
                return;
            }
        }

    }

    /// <summary>
    /// ����� 3 � ���, ������� �� ���� ��� �����
    /// </summary> 
    private void Mearg3Row(int index) {
        //�������� ����� ��� ������ 3 � ���

        GameObject object0Column0 = null;
        GameObject object0Column1 = null;
        GameObject object0Column2 = null;
        GameObject object0Column3 = null;


        if(_oneColumSlots[index].GetComponentInChildren<CanvasGroup>() != null) {
            object0Column0 = _oneColumSlots[index].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(_twoColumSlots[index].GetComponentInChildren<CanvasGroup>() != null) {
            object0Column1 = _twoColumSlots[index].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(_threeColumSlots[index].GetComponentInChildren<CanvasGroup>() != null) {
            object0Column2 = _threeColumSlots[index].GetComponentInChildren<CanvasGroup>().gameObject;
        }

        if(_fourColumSlots[index].GetComponentInChildren<CanvasGroup>() != null) {
            object0Column3 = _fourColumSlots[index].GetComponentInChildren<CanvasGroup>().gameObject;
        }
        // 4 � ���
        if(object0Column0 != null && object0Column1 != null && object0Column2 != null && object0Column3 != null) {
            if(object0Column0.tag == object0Column1.tag && object0Column0.tag == object0Column2.tag && object0Column0.tag == object0Column3.tag 
                && object0Column1.tag == object0Column2.tag && object0Column1.tag == object0Column3.tag && object0Column2.tag == object0Column3.tag) {
                Debug.Log("4 � ��� �� �����������");
                _startTimer = false;
                
                AddResouces(object0Column0.tag);
                AddResouces(object0Column1.tag);
                AddResouces(object0Column2.tag);
                AddResouces(object0Column3.tag);

                // Destroy(object0Column0);
                // Destroy(object0Column1);
                // Destroy(object0Column2);
                // Destroy(object0Column3);
                
                object0Column0.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column3.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(3);
                return;
            }
        }

        // 3 � ���
        if(object0Column0 != null && object0Column1 != null && object0Column2 != null) {
            if(object0Column0.tag == object0Column1.tag && object0Column0.tag == object0Column2.tag &&
                object0Column1.tag == object0Column2.tag) {
                Debug.Log("3 � ��� �� �����������");
                _startTimer = false;
                
                AddResouces(object0Column0.tag);
                AddResouces(object0Column1.tag);
                AddResouces(object0Column2.tag);

                // Destroy(object0Column0);
                // Destroy(object0Column1);
                // Destroy(object0Column2);
                
                object0Column0.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column2.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(2);
                return;
            }
        }

        if(object0Column1 != null && object0Column2 != null && object0Column3 != null) {
            if(object0Column1.tag == object0Column2.tag && object0Column1.tag == object0Column3.tag 
                && object0Column2.tag == object0Column3.tag) {
                Debug.Log("3 � ��� �� �����������");
                _startTimer = false;
              
                AddResouces(object0Column1.tag);
                AddResouces(object0Column2.tag);
                AddResouces(object0Column3.tag);

                // Destroy(object0Column1);
                // Destroy(object0Column2);
                // Destroy(object0Column3);
                
                object0Column1.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column2.GetComponentInChildren<Animator>().SetBool("Del", true);
                object0Column3.GetComponentInChildren<Animator>().SetBool("Del", true);

                SoundsEvents.onMathSound?.Invoke();
                UpdateSparks(2);
                return;
            }
        }
    }

    private void UpdateSparks(int count) {
        if(count == 2) {
            EventsResources.onAddOrDeductSparkValue?.Invoke(1, true);
            // Debug.Log($"����������� ������� �� {1}");
        }
        if(count == 3) {
            EventsResources.onAddOrDeductSparkValue?.Invoke(3, true);
            // Debug.Log($"����������� ������� �� {3}");
        }
        if(count == 4) {
            EventsResources.onAddOrDeductSparkValue?.Invoke(5, true);
            // Debug.Log($"����������� ������� �� {5}");
        }
    }

    private void AddResouces(string tag) {
        if(ResourcesTags.Cloth_1.ToString() == tag) {
            EventsResources.onClouthInBucket?.Invoke(1, 1, 1);
        }
        if(ResourcesTags.Cloth_2.ToString() == tag) {
            EventsResources.onClouthInBucket?.Invoke(2, 1, 1);
        }
        if(ResourcesTags.Cloth_3.ToString() == tag) {
            EventsResources.onClouthInBucket?.Invoke(3, 1, 1);
        }

        if(ResourcesTags.Log_1.ToString() == tag) {
            EventsResources.onLogInBucket?.Invoke(1, 1, 1);
        }
        if(ResourcesTags.Log_2.ToString() == tag) {
            EventsResources.onLogInBucket?.Invoke(2, 1, 1);
        }
        if(ResourcesTags.Log_3.ToString() == tag) {
            EventsResources.onLogInBucket?.Invoke(3, 1, 1);
        }

        if(ResourcesTags.Neil_1.ToString() == tag) {
            EventsResources.onNeilInBucket?.Invoke(1, 1, 1);
        }
        if(ResourcesTags.Neil_2.ToString() == tag) {
            EventsResources.onNeilInBucket?.Invoke(2, 1, 1);
        }
        if(ResourcesTags.Neil_3.ToString() == tag) {
            EventsResources.onNeilInBucket?.Invoke(3, 1, 1);
        }

        if(ResourcesTags.Stone_1.ToString() == tag) {
            EventsResources.onStoneInBucket?.Invoke(1, 1, 1);
        }
        if(ResourcesTags.Stone_2.ToString() == tag) {
            EventsResources.onStoneInBucket?.Invoke(2, 1, 1);
        }
        if(ResourcesTags.Stone_3.ToString() == tag) {
            EventsResources.onStoneInBucket?.Invoke(3, 1, 1);
        }

    }
    

    /// <summary>
    /// ����� ��� ������ ���� � ���������, ��������� ������.
    /// </summary>
    /// <param name="itemTag">��� ���� ������� ����� ����������</param>
    private void SpawnItemRandomSlotToTag(string itemTag) {
        if(_allSlots[_currentSlotIndex].GetComponentInChildren<CanvasGroup>() != null) {
            _currentSlotIndex = Random.Range(0, _allSlots.Length);
        } else {

            foreach(var item in _combinedList) {
                var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

                if(itemTag == childrenTag) {
                    Instantiate(item, _allSlots[_currentSlotIndex].transform);
                    SoundsEvents.onSpawnRuns?.Invoke();
                    _currentSlotIndex = Random.Range(0, _allSlots.Length);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// ����� ������� ���� ���� � ���������� ������
    /// </summary>
    /// <param name="itemTag">��� ���� ������� ����� ����������</param>
    /// <param name="slotId">������������� ����� � ������� �������� ����</param>
    private void SpawnItemToSlot(string itemTag, int slotId) {
        foreach(var slot in _allSlots) {
            if(slot.GetComponentInChildren<Slot_3d>().GetSlotID() == slotId) {
                foreach(var item in _combinedList) {
                    var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

                    if(itemTag == childrenTag) {
                        Instantiate(item, slot.transform);
                        SoundsEvents.onSpawnRuns?.Invoke();
                        return;
                    }
                }
                return;
            }
        }
    }

    /// <summary>
    /// ����� ��� ������ ��� ������ ������� � �� ��������
    /// </summary>
    private void SpawnOneColumnRuns() {
        if(_holdOneColumn == false) {
            if(_currentTimeRespOneColumn <= 0) {
                if(_oneColumSlots[0].GetComponentInChildren<CanvasGroup>() == null) {
                    Instantiate(_items_1lv[Random.Range(0, _items_1lv.Length)], _oneColumSlots[0].transform);
                    SoundsEvents.onSpawnRuns?.Invoke();
                    
                    _currentTimeRespOneColumn = _timeRespOneColumn;
                } else {
                    if(_oneColumSlots[1].GetComponentInChildren<CanvasGroup>() == null) {
                        OneCirculeSelected(_oneColumSlots, _items_1lv);
                        _currentTimeRespOneColumn = _timeRespOneColumn;
                    } else {
                        if(_oneColumSlots[2].GetComponentInChildren<CanvasGroup>() == null) {
                            TwoCirculeSelected(_oneColumSlots, _items_1lv);
                            _currentTimeRespOneColumn = _timeRespOneColumn;
                        } else {
                            if(_oneColumSlots[3].GetComponentInChildren<CanvasGroup>() == null) {
                                ThreeCirculeSelected(_oneColumSlots, _items_1lv);
                                _currentTimeRespOneColumn = _timeRespOneColumn;
                            } else {
                                if(_oneColumSlots[4].GetComponentInChildren<CanvasGroup>() == null) {
                                    FoureCirculeSelected(_oneColumSlots, _items_1lv);
                                    _currentTimeRespOneColumn = _timeRespOneColumn;
                                } else {
                                    FiveCirculeSelected(_oneColumSlots, _items_1lv);
                                    _currentTimeRespOneColumn = _timeRespOneColumn;
                                }
                            }
                        }
                    }
                    MoveSelecteSlot(_oneColumSlots);
                }
            } else {
                _currentTimeRespOneColumn -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����� ��� ������ ��� ������ ������� � �� ��������
    /// </summary>
    private void SpawnTwoColumnRuns() {
        if(_holdTwoColumn == false) {
            if(_currentTimeRespTwoColumn <= 0) {
                if(_twoColumSlots[0].GetComponentInChildren<CanvasGroup>() == null) {
                    Instantiate(_items_1lv[Random.Range(0, _items_1lv.Length)], _twoColumSlots[0].transform);
                    SoundsEvents.onSpawnRuns?.Invoke();

                    _currentTimeRespTwoColumn = _timeRespTwoColumn;

                } else {
                    if(_twoColumSlots[1].GetComponentInChildren<CanvasGroup>() == null) {
                        OneCirculeSelected(_twoColumSlots, _items_1lv);
                        _currentTimeRespTwoColumn = _timeRespTwoColumn;

                    } else {
                        if(_twoColumSlots[2].GetComponentInChildren<CanvasGroup>() == null) {
                            TwoCirculeSelected(_twoColumSlots, _items_1lv);
                            _currentTimeRespTwoColumn = _timeRespTwoColumn;
                        } else {
                            if(_twoColumSlots[3].GetComponentInChildren<CanvasGroup>() == null) {
                                ThreeCirculeSelected(_twoColumSlots, _items_1lv);
                                _currentTimeRespTwoColumn = _timeRespTwoColumn;
                            } else {
                                if(_twoColumSlots[4].GetComponentInChildren<CanvasGroup>() == null) {
                                    FoureCirculeSelected(_twoColumSlots, _items_1lv);
                                    _currentTimeRespTwoColumn = _timeRespTwoColumn;
                                } else {
                                    FiveCirculeSelected(_twoColumSlots, _items_1lv);
                                    _currentTimeRespTwoColumn = _timeRespTwoColumn;
                                }
                            }
                        }
                    }
                    MoveSelecteSlot(_twoColumSlots);
                }
            } else {
                _currentTimeRespTwoColumn -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����� ��� ������ ��� ������� ������� � �� ��������
    /// </summary>
    private void SpawnThreeColumnRuns() {
        if(_holdThreeColumn == false) {
            if(_currentTimeRespThreeColumn <= 0) {
                if(_threeColumSlots[0].GetComponentInChildren<CanvasGroup>() == null) {
                    Instantiate(_items_1lv[Random.Range(0, _items_1lv.Length)], _threeColumSlots[0].transform);
                    SoundsEvents.onSpawnRuns?.Invoke();
                    
                    _currentTimeRespThreeColumn = _timeRespThreeColumn;
                } else {
                    if(_threeColumSlots[1].GetComponentInChildren<CanvasGroup>() == null) {
                        OneCirculeSelected(_threeColumSlots, _items_1lv);
                        _currentTimeRespThreeColumn = _timeRespThreeColumn;

                    } else {
                        if(_threeColumSlots[2].GetComponentInChildren<CanvasGroup>() == null) {
                            TwoCirculeSelected(_threeColumSlots, _items_1lv);
                            _currentTimeRespThreeColumn = _timeRespThreeColumn;
                        } else {
                            if(_threeColumSlots[3].GetComponentInChildren<CanvasGroup>() == null) {
                                ThreeCirculeSelected(_threeColumSlots, _items_1lv);
                                _currentTimeRespThreeColumn = _timeRespThreeColumn;
                            } else {
                                if(_threeColumSlots[4].GetComponentInChildren<CanvasGroup>() == null) {
                                    FoureCirculeSelected(_threeColumSlots, _items_1lv);
                                    _currentTimeRespThreeColumn = _timeRespThreeColumn;
                                } else {
                                    FiveCirculeSelected(_threeColumSlots, _items_1lv);
                                    _currentTimeRespThreeColumn = _timeRespThreeColumn;
                                }
                            }
                        }
                    }
                    MoveSelecteSlot(_threeColumSlots);
                }
            } else {
                _currentTimeRespThreeColumn -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����� ��� ������ ��� ��������� ������� � �� ��������
    /// </summary>
    private void SpawnFourColumnRuns() {
        if(_holdFourColumn == false) {
            if(_currentTimeRespFourColumn <= 0) {
                if(_fourColumSlots[0].GetComponentInChildren<CanvasGroup>() == null) {
                    Instantiate(_items_1lv[Random.Range(0, _items_1lv.Length)], _fourColumSlots[0].transform);
                    SoundsEvents.onSpawnRuns?.Invoke();

                    _currentTimeRespFourColumn = _timeRespFourColumn;

                } else {
                    if(_fourColumSlots[1].GetComponentInChildren<CanvasGroup>() == null) {
                        OneCirculeSelected(_fourColumSlots, _items_1lv);
                        _currentTimeRespFourColumn = _timeRespFourColumn;

                    } else {
                        if(_fourColumSlots[2].GetComponentInChildren<CanvasGroup>() == null) {
                            TwoCirculeSelected(_fourColumSlots, _items_1lv);
                            _currentTimeRespFourColumn = _timeRespFourColumn;
                        } else {
                            if(_fourColumSlots[3].GetComponentInChildren<CanvasGroup>() == null) {
                                ThreeCirculeSelected(_fourColumSlots, _items_1lv);
                                _currentTimeRespFourColumn = _timeRespFourColumn;
                            } else {
                                if(_fourColumSlots[4].GetComponentInChildren<CanvasGroup>() == null) {
                                    FoureCirculeSelected(_fourColumSlots, _items_1lv);
                                    _currentTimeRespFourColumn = _timeRespFourColumn;
                                } else {
                                    FiveCirculeSelected(_fourColumSlots, _items_1lv);
                                    _currentTimeRespFourColumn = _timeRespFourColumn;
                                }
                            }
                        }
                    }
                    MoveSelecteSlot(_fourColumSlots);
                }
            } else {
                _currentTimeRespFourColumn -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����� ���������� ���������� ���� ������ � ����������� ���
    /// </summary>
    /// <param name="columSlots"></param>
    private void MoveSelecteSlot(GameObject[] columSlots) {

        //GameObject currentObject = MeargGameEvents.onGetCurrentObject?.Invoke();
        GameObject currentObject = _selectedAndMeargItem.GetCurrentGameObject();

        //Debug.Log($"�����  MoveSelecteSlot: ���� = {currentObject}");

        //var currentSlot = MeargGameEvents.onGetCurrentSlot?.Invoke();
        var currentSlot = _selectedAndMeargItem.GetCurrentSlot();
        //Debug.Log($"�����  MoveSelecteSlot: ���� = {currentSlot}");

        for(int i = 0; i < columSlots.Length; i++) {

            Slot_3d slot = columSlots[i].GetComponentInChildren<Slot_3d>();

            
                if(currentSlot != null && slot.GetSlotID() == currentSlot.GetSlotID() ) {

                if(i + 1 < columSlots.Length) {
                    Slot_3d nextSlot = columSlots[i + 1].GetComponentInChildren<Slot_3d>();
                    var nextRuns = columSlots[i + 1].GetComponentInChildren<CanvasGroup>();

                    //Debug.Log($"�����  MoveSelecteSlot: ������� ����� ������ ��������� ������");
                    slot.DeselectSlot(slot.gameObject);

                    //Debug.Log($"�����  MoveSelecteSlot: ������� ������ �������� ��� � �������� = {nextRuns.gameObject}");
                    _selectedAndMeargItem.ClearVariables();
                    _selectedAndMeargItem.ClearSlot();

                    //Debug.Log($"�����  MoveSelecteSlot: ������� ����� ����������� ���� = {nextRuns.gameObject}");
                    //MeargGameEvents.onSelectedSlot?.Invoke(nextSlot.GetSlotID(), currentObject);
                    _selectedAndMeargItem.CheckSelectedInSlot(nextSlot.GetSlotID(), nextRuns.gameObject);


                    //MeargGameEvents.onSetOldObject?.Invoke(currentObject);
                    _selectedAndMeargItem.SetOldObject(nextRuns.gameObject);
                    //MeargGameEvents.onStartEventSetOldObject?.Invoke(currentObject); // ������ �������� �� �����
                    //Debug.Log($"�����  MoveSelecteSlot: ������� ����� ���������� ������� ������� = {nextRuns.gameObject}");

                    //MeargGameEvents.onSetOldSlot?.Invoke(nextSlot);
                    _selectedAndMeargItem.SetOldSlot(nextSlot);
                    //Debug.Log($"�����  MoveSelecteSlot: ������� ����� ���������� ������� ����� = {nextSlot}");
                    //MeargGameEvents.onSelectedSlot?.Invoke(nextSlot.GetSlotID(), currentObject);

                    //Slot_3d oldSlot = MeargGameEvents.onGetOldSlot?.Invoke();
                    Slot_3d oldSlot = _selectedAndMeargItem.GetOldSlot();
                    //Debug.Log($"�����  MoveSelecteSlot: ������ ���� = {oldSlot}");
                    //GameObject oldObject = MeargGameEvents.onGetOldObject?.Invoke();
                    GameObject oldObject = _selectedAndMeargItem.GetOldGameObject();
                    //Debug.Log($"�����  MoveSelecteSlot: ������ ���� = {oldObject}");

                    nextSlot.SelectSlot(nextSlot.gameObject);

                    break;
                } else {
                    //Debug.Log("��������� ������ � �������");
                    slot.DeselectSlot(slot.gameObject);

                    _selectedAndMeargItem.ClearVariables();
                    _selectedAndMeargItem.ClearSlot();

                    //Debug.Log("�������� �������� ������ ����");
                    break;
                }
            }
        }


        //for(int i = 0; i < columSlots.Length; i++) {

        //    Slot_3d oldSlot = columSlots[i].GetComponentInChildren<Slot_3d>();
        //    //Debug.Log("����� ���� �� �� �����������");

        //    if(oldSlot.GetSelected() == true) {
        //        Debug.Log($"������������� ����������� ����� = {oldSlot}");

        //        if(i + 1 < columSlots.Length) {
        //            Slot_3d slot = columSlots[i + 1].GetComponent<Slot_3d>();
        //            //Debug.Log("����� ���� �� ���� �� �������");
        //            GameObject pointerObject = columSlots[i + 1].GetComponentInChildren<CanvasGroup>().gameObject;

        //            //MeargGameEvents.onClearOldSlot?.Invoke();
        //            MeargGameEvents.onSelectedSlot?.Invoke(slot.GetSlotID(), pointerObject);
        //            slot.SelectSlot(slot.gameObject);
        //            oldSlot.DeselectSlot(oldSlot.gameObject);

        //            break;
        //        } else {
        //            oldSlot.DeselectSlot(oldSlot.gameObject);
        //            MeargGameEvents.onClearOldSlot?.Invoke();
        //            //Debug.Log("�������� �������� ������ ����");

        //            break;
        //        }
        //    }
        //}
    }

    private void OneCirculeSelected(GameObject[] columSlots, GameObject[] itemsLvl) {
        var oneSlotRuneTag = columSlots[0].GetComponentInChildren<CanvasGroup>().tag;

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(oneSlotRuneTag == childrenTag) {
                Destroy(columSlots[0].GetComponentInChildren<CanvasGroup>().gameObject);
                Instantiate(item, columSlots[1].transform);
                Instantiate(SelectRuns(childrenTag, itemsLvl), columSlots[0].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                //currentTime = startTime;
                break;
            }
        }

        //MoveSelecteSlot(columSlots);
    }

    private void TwoCirculeSelected(GameObject[] columSlots, GameObject[] itemsLvl) {
        var oneSlotRuneTag = columSlots[0].GetComponentInChildren<CanvasGroup>().tag;
        var twoSlotRuneTag = columSlots[1].GetComponentInChildren<CanvasGroup>().tag;

        //Debug.Log("��� ���� ��������");

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Destroy(columSlots[0].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[1].GetComponentInChildren<CanvasGroup>().gameObject);
                //Debug.Log("������  ��� ����");
                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[2].transform);
                SoundsEvents.onSpawnRuns?.Invoke();
                //Debug.Log("��������� ���� � ������� �����");
                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(oneSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[1].transform);
                Instantiate(SelectRuns(childrenTag, itemsLvl), columSlots[0].transform);
                SoundsEvents.onSpawnRuns?.Invoke();
                //Debug.Log("��������� ���� �� ������ � ������ �����");
                //currentTime = startTime;
                break;
            }
        }
        //MoveSelecteSlot(columSlots);
    }

    private void ThreeCirculeSelected(GameObject[] columSlots, GameObject[] itemsLvl) {
        var oneSlotRuneTag = columSlots[0].GetComponentInChildren<CanvasGroup>().tag;
        var twoSlotRuneTag = columSlots[1].GetComponentInChildren<CanvasGroup>().tag;
        var threeSlotRuneTag = columSlots[2].GetComponentInChildren<CanvasGroup>().tag;

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Destroy(columSlots[0].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[1].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[2].GetComponentInChildren<CanvasGroup>().gameObject);
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(threeSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[3].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[2].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(oneSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[1].transform);
                Instantiate(SelectRuns(childrenTag, itemsLvl), columSlots[0].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                //currentTime = startTime;
                break;
            }
        }

        //MoveSelecteSlot(columSlots);
    }

    private void FoureCirculeSelected(GameObject[] columSlots, GameObject[] itemsLvl) {
        var oneSlotRuneTag = columSlots[0].GetComponentInChildren<CanvasGroup>().tag;
        var twoSlotRuneTag = columSlots[1].GetComponentInChildren<CanvasGroup>().tag;
        var threeSlotRuneTag = columSlots[2].GetComponentInChildren<CanvasGroup>().tag;
        var foureSlotRuneTag = columSlots[3].GetComponentInChildren<CanvasGroup>().tag;

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Destroy(columSlots[0].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[1].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[2].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[3].GetComponentInChildren<CanvasGroup>().gameObject);
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(foureSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[4].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(threeSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[3].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[2].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(oneSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[1].transform);
                Instantiate(SelectRuns(childrenTag, itemsLvl), columSlots[0].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                //currentTime = startTime;
                break;
            }
        }

        //MoveSelecteSlot(columSlots);
    }

    private void FiveCirculeSelected(GameObject[] columSlots, GameObject[] itemsLvl) {
        var oneSlotRuneTag = columSlots[0].GetComponentInChildren<CanvasGroup>().tag;
        var twoSlotRuneTag = columSlots[1].GetComponentInChildren<CanvasGroup>().tag;
        var threeSlotRuneTag = columSlots[2].GetComponentInChildren<CanvasGroup>().tag;
        var foureSlotRuneTag = columSlots[3].GetComponentInChildren<CanvasGroup>().tag;
        var fiveSlotRuneTag = columSlots[4].GetComponentInChildren<CanvasGroup>().tag;

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(fiveSlotRuneTag == childrenTag) {
                Destroy(columSlots[0].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[1].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[2].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[3].GetComponentInChildren<CanvasGroup>().gameObject);
                Destroy(columSlots[4].GetComponentInChildren<CanvasGroup>().gameObject);

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(foureSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[4].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(threeSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[3].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(twoSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[2].transform);
                SoundsEvents.onSpawnRuns?.Invoke();

                break;
            }
        }

        foreach(var item in _combinedList) {
            var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

            if(oneSlotRuneTag == childrenTag) {
                Instantiate(item, columSlots[1].transform);
                Instantiate(SelectRuns(childrenTag, itemsLvl), columSlots[0].transform);
                SoundsEvents.onSpawnRuns?.Invoke();
               
                break;
            }
        }

        //MoveSelecteSlot(columSlots);
    }
    private GameObject SelectRuns(string  tag, GameObject[] items) {
        GameObject returneObject = items[Random.Range(0, items.Length)];

        while(returneObject.tag == tag) {
            //Debug.Log("�������� �������� �� �� ���� �������");
            returneObject = items[Random.Range(0, items.Length)];
        }

        return returneObject;
    }

    /// <summary>
    /// ���������� ������� ����� ������� ����� �������.
    /// </summary>
    /// <returns></returns>
    private float GetCurrentTimer() {
        //return _currentTimeRespOneColumn;
        return _timeRespOneColumn;
    }

    /// <summary>
    /// ������������� ������� ����� ������� ��� ������ ���� ������� ���
    /// </summary>
    /// <param name="currentTime"></param>
    private void SetCurrentTimeOldColumn(float currentTime) {
        _currentTimeRespOneColumn = currentTime;
        _currentTimeRespTwoColumn = currentTime;
        _currentTimeRespThreeColumn = currentTime;
        _currentTimeRespFourColumn = currentTime;
    }

    /// <summary>
    /// ����� ��������� ���
    /// </summary>
    private void TiefRuns() {
        var randomSelectedRuns = Random.Range(0, _allSlots.Length);

        if(_allSlots[randomSelectedRuns].GetComponentInChildren<CanvasGroup>() == null) {
            TiefRuns();
        } else {
            Destroy(_allSlots[randomSelectedRuns].GetComponentInChildren<CanvasGroup>().gameObject);
            Debug.Log("���� ����� ����");
        }
    }

    /// <summary>
    /// ����� ������������� ��� �� �����
    /// </summary>
    private void RandomRuns() {
        _startTimer = false;

        List<string> tagRuns = new List<string>();
        List<int> slotRunsList = new List<int>();
        HashSet<int> slotNUmbers = new HashSet<int>();

        foreach(var slot in _allSlots) {
            if(slot.GetComponentInChildren<CanvasGroup>() != null) {
                tagRuns.Add(slot.GetComponentInChildren<CanvasGroup>().tag);
                Destroy(slot.GetComponentInChildren<CanvasGroup>().gameObject);
            }
        }

        while(slotNUmbers.Count < tagRuns.Count) {
            slotNUmbers.Add(Random.Range(0, _allSlots.Length));
        }

        slotRunsList = slotNUmbers.ToList();
        for(int i = 0; i < tagRuns.Count; i++) {
            var tag = tagRuns[i];

            foreach(var runs in _combinedList) {
                var runTag = runs.GetComponentInChildren<CanvasGroup>().tag;

                if(tag == runTag) {
                    Instantiate(runs, _allSlots[slotRunsList[i]].transform);
                    SoundsEvents.onSpawnRuns?.Invoke();                
                    break;
                }
            }
        }

        Debug.Log("���� ��������� ����");

        _startTimer = true;
    }

    /**
     * ������������� _startTimer � true. ���������� �������.
     */
    private void SetStartTimerTrue() {
        _startTimer = true;
    }

    #region ���������� � �������� ��� �� �����
    private void SaveItems() {
        for(int i = 0; i < _allSlots.Length; i++) {
            if(_allSlots[i].GetComponentInChildren<CanvasGroup>() != null) {
                //var level = _slots[i].GetComponentInChildren<Item>().GetCurrentAmountForText();
                var tag = _allSlots[i].GetComponentInChildren<CanvasGroup>().tag;

                PlayerPrefs.SetInt($"numberSlot_{i}", i);
                //PlayerPrefs.SetInt($"numberSlot_{i}_level", level);
                PlayerPrefs.SetString($"numberSlot_{i}_tag", tag);

                //Debug.Log($"�������� ������� � ����� {i} � ������� {level} � ����� {tag}");
            }
        }
    }

    private void ReloadItems() {
        IDictionary<int, string> reloadDictionary = new Dictionary<int, string>();

        for(int i = 0; i < _allSlots.Length; i++) {
            if(PlayerPrefs.HasKey($"numberSlot_{i}") && PlayerPrefs.HasKey($"numberSlot_{i}_tag")) {
                var numberSlot = PlayerPrefs.GetInt($"numberSlot_{i}");
                var tag = PlayerPrefs.GetString($"numberSlot_{i}_tag");

                reloadDictionary.Add(numberSlot, tag);
                Debug.Log($"�������� ������� �� ������ ��� ����� {numberSlot} � ����� {tag}");
            }
        }

        SpawnReloadItems(reloadDictionary);
    }

    private void SpawnReloadItems(IDictionary<int, string> dictionary) {
        foreach(var dict in dictionary) {
            var slot = dict.Key;
            var tag = dict.Value;

            foreach(var item in _combinedList) {
                var childrenTag = item.GetComponentInChildren<CanvasGroup>().tag;

                if(tag == childrenTag) {
                    Instantiate(item, _allSlots[slot].transform);

                    PlayerPrefs.DeleteKey($"numberSlot_{slot}");
                    PlayerPrefs.DeleteKey($"numberSlot_{slot}_tag");
                }
            }
        }
    }
    #endregion
}
