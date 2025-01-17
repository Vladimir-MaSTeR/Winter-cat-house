using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{

    [Header("������ �������� ����")]
    [Tooltip("������ ���� ������")]
    [SerializeField] private GameObject _fireplaceObje;
    [Tooltip("������ ���� ������")]
    [SerializeField] private GameObject _armchairObje;
    [Tooltip("������ ���� �����")]
    [SerializeField] private GameObject _kitchenObje;
    [Tooltip("������ ���� �������� �� 2 ����")]
    [SerializeField] private GameObject _ladderGoTo2Obje;
    [Tooltip("������ ���� �������")]
    [SerializeField] private GameObject _bedObje;
    [Tooltip("������ ���� �������� �� 3 ����")]
    [SerializeField] private GameObject _ladderGoTo3Obje;
    [Tooltip("������ ���� ���� � ������")]
    [SerializeField] private GameObject _cupboardObje;

    [Tooltip("����� �� 2 �����")]
    [SerializeField] private GameObject _mist2storey;
    [SerializeField] Animation _anim_Open2storey;
    [Tooltip("����� �� 3 �����")]
    [SerializeField] private GameObject _mist3storey;
    [SerializeField] Animation _anim_Open3storey;

    [SerializeField]
    private int _openStorey = 0;


    [Header("������� ������� ����")]
    [Tooltip("������� ������")]
    private int _fireplaceLv;
    [Tooltip("������� ������")]
    private int _armchairLv;
    [Tooltip("������� �����")]
    private int _kitchenLv;
    [Tooltip("������� �������� �� 2 ����")]
    private int _ladderGoTo2Lv;
    [Tooltip("������� �������")]
    private int _bedLv;
    [Tooltip("������� �������� �� 3 ����")]
    private int _ladderGoTo3Lv;
    [Tooltip("������� ���� � ������")]
    private int _cupboardLv;
    [Tooltip("������� ��� ����")]
    private IDictionary<string, int> _homeDictionary = new Dictionary<string, int>();


    // Start is called before the first frame update
    void Start()
    {
        LoadResouces();

        if (_openStorey >= 1)
        {
            offMist2Stroery();
        }
    }

    private void Update()
    {
        _checkQuests();

    }

    private void OnEnable()
    {
         EventsResources.onGetLvObjHome += _checkLvObjHomeNow;

    }

    private void OnDisable()
    {
          EventsResources.onGetLvObjHome -= _checkLvObjHomeNow;

    }

    /// <summary>
    /// �������� ������ �������� � ����
    /// </summary>
    private IDictionary<string, int> _checkLvObjHomeNow(int levelQuests)
    {
        _homeDictionary.Clear();
        AddLvObjHomeDictionary(_homeDictionary);
        return _homeDictionary;
    }

    [Tooltip("����� ���������� ������ �������� ���� � �������")]
    private void AddLvObjHomeDictionary(IDictionary<string, int> homeDictionary)
    {
        if (homeDictionary.Count == 0)
        {
            homeDictionary.Add(_fireplaceObje.GetComponent<ClickRepair>().NameObject.ToString(),
                                _fireplaceObje.GetComponent<ClickRepair>().LvObj);
            homeDictionary.Add(_armchairObje.GetComponent<ClickRepair>().NameObject.ToString(),
                                _armchairObje.GetComponent<ClickRepair>().LvObj);
            homeDictionary.Add(_kitchenObje.GetComponent<ClickRepair>().NameObject.ToString(),
                                _kitchenObje.GetComponent<ClickRepair>().LvObj);
            homeDictionary.Add(_ladderGoTo2Obje.GetComponent<ClickRepair>().NameObject.ToString(),
                                _ladderGoTo2Obje.GetComponent<ClickRepair>().LvObj);
            homeDictionary.Add(_bedObje.GetComponent<ClickRepair>().NameObject.ToString(),
                                _bedObje.GetComponent<ClickRepair>().LvObj);
            homeDictionary.Add(_ladderGoTo3Obje.GetComponent<ClickRepair>().NameObject.ToString(),
                                _ladderGoTo3Obje.GetComponent<ClickRepair>().LvObj);
            homeDictionary.Add(_cupboardObje.GetComponent<ClickRepair>().NameObject.ToString(),
                               _cupboardObje.GetComponent<ClickRepair>().LvObj);
        }
    }


    private void _checkQuests()
    {
        {
            // onGetQuestsDictionary
            bool _fireplaceQuests = false;
            bool _armchairQuests = false;
            bool _kitchenQuests = false;
            if (_openStorey == 0) /// ����� - ������� 2 ����
            {
                if (_fireplaceObje.GetComponent<ClickRepair>().LvObj > 0)
                {
                    _fireplaceQuests = true;
                }
                if (_armchairObje.GetComponent<ClickRepair>().LvObj > 0)
                {
                    _armchairQuests = true;
                }
                if (_kitchenObje.GetComponent<ClickRepair>().LvObj > 0)
                {
                    _kitchenQuests = true;
                }

                if (_kitchenQuests == true && _armchairQuests == true && _fireplaceQuests == true)
                {
                    ///Debug.Log("����� ���");
                    EventsResources.onEndMainQuest?.Invoke();
                    //_mist2storey.GetComponent<Animator>().enabled = false;
                    _mist2storey.GetComponent<Animator>().SetBool("OperStorey", true);
                    _mist2storey.GetComponent<Animator>().SetTrigger("Oper2storey");

                    //   _anim_Open2storey; //m_Controller
                    _openStorey = 1;
                    SaveResources();
                    Debug.Log("���� ������" + _openStorey + 1);
                    Invoke("offMist2Stroery", 10f);
                    Debug.Log("������ ����� 2 ����");

                }
            }
        }
    }



    private void offMist2Stroery()
    {
        _mist2storey.SetActive(false);
    }




    /// <summary>
    /// ���������� � ��������� (��������)
    /// </summary>
    private void _readyUp()
    {


    }


    
    private void SaveResources()
    {
        PlayerPrefs.SetInt("OpenHomeStorey", _openStorey);


        
        PlayerPrefs.Save();
    }

    private void LoadResouces()
    {
        
        {
            if (PlayerPrefs.HasKey("OpenHomeStorey"))
            {
                _openStorey = PlayerPrefs.GetInt("OpenHomeStorey");
            }



        }
    }
}
