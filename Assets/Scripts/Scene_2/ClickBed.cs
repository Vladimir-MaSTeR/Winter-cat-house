
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickBed : MonoBehaviour, IPointerClickHandler
{
    [Header("��������� ���������� ��� ��������� �������� ��������")]
    [SerializeField] private bool loadResorces = true;

    /// <summary>
    /// ������ ������� �� ������� �� 0 -������� �� n- ��������
    /// </summary>
    [SerializeField]
    private GameObject[] _objectModel;
    /// <summary>
    /// ������ ������� ������
    /// </summary>
    [SerializeField]
    private GameObject _objectNow;
    /// <summary>
    /// ��������� ������� ������� 
    /// </summary>
    [SerializeField] 
    private int _lvObject = 0;
    /// <summary>
    /// ������������ ������� ������� 
    /// </summary>
[SerializeField]
    private int _lvObjectMax;
    /// <summary>
    ///      ����� ��� ������� �������
    /// </summary>
    [SerializeField]
    private GameObject _scaleProgress;
    /// <summary>
    ///     ����� ��� ������� ���������� (���������)
    /// </summary>
    [SerializeField]
    private GameObject _scaleProgressUp;
    /// <summary>
    /// ����������� ����������� ������ (��������) ��� Lv Up
    /// </summary>
    [SerializeField]
    private int _amtRequiredResourceGoLvUp = 1;
    /// <summary>
    /// ���������� ������������� ������ ��� Lv Up  (������� ���������� �����)
    /// </summary>
    [SerializeField]
    private float _amtAddResource = 0;
    /// <summary>
    /// �����  �� ������� ��� Up
    /// </summary>
    [SerializeField]
    private int _needTimeGoLvUp = 2;
    /// <summary>
    ///  ������ �����������
    /// </summary>
[SerializeField]
    private bool _activTimeGoLvUp = false;

    /// <summary>
    /// ������� ������� ������� 
    /// </summary>
    private int _lvObjectNow;
    /// <summary>
    /// ������� ����������� ������� � ����� 
    /// </summary>
    private int _needResourceBagNow;
    /// <summary>
    /// ��������� ������� ������� ��� ������� 
    /// </summary>
    private int _needLvResource;


    private void Start()
    {
        if (!loadResorces)
        { _lvObjectNow = _lvObject; }
        else
        { LoadResouces(); }

        _lvObjectMax = _objectModel.Length -1;
        AddModel(_lvObjectNow);
        _needTimeGoLvUp = _amtRequiredResourceGoLvUp / 4;
        _needLvResource = _lvObjectNow + 1;
        _needResourceBagNow = (int)EventsResources.onGetCurentNeil?.Invoke(_needLvResource);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {


        // �������� �������
        _needLvResource = _lvObjectNow + 1;
        _needResourceBagNow = (int)EventsResources.onGetCurentNeil?.Invoke(_needLvResource);

        if (_lvObjectNow < _lvObjectMax  &&
              _needResourceBagNow >= _amtRequiredResourceGoLvUp)  /// �������� �������
         {
            _amtAddResource += 1;
            //if (_activTimeGoLvUp == true)
            //{ ScaleProgress(true); }

            if (_scaleProgress.activeSelf == false)
            {
                // Invoke("_timeScaleOff", _needTimeGoLvUp);
                _activTimeGoLvUp = true;
                ScaleProgress(true);
                _timeScaleOff();
            }
            else if ( _amtAddResource >= _amtRequiredResourceGoLvUp )
            {
                LvUp();
                _amtAddResource = 0;
                ScaleProgress(false);
            }



        } // ������� �� �������
        else
        {
            
        }



    }


    /// <summary>
    /// ��������� ������
    /// </summary>
   private void LvUp()
    {
        _lvObjectNow += 1;
         AddModel(_lvObjectNow);
        EventsResources.onNeilInBucket?.Invoke(_lvObjectNow, _amtRequiredResourceGoLvUp, 0); // ������� ������ ��� LvUp ;
        _amtRequiredResourceGoLvUp = (int)(_amtRequiredResourceGoLvUp * 1.3f);
        _needTimeGoLvUp = _amtRequiredResourceGoLvUp / 4;
        _needLvResource = _lvObjectNow + 1;
        SaveResources();

    }


    /// <summary>
    /// ������ ������� ���� �������
    /// </summary>
    /// <param name="_LvMod"></param>
    private void AddModel(int _LvMod)
    {
        Destroy(_objectNow);
        if (_LvMod <= _objectModel.Length && _LvMod >= 0)
        {
            _objectNow = Instantiate(_objectModel[_LvMod], transform.position, Quaternion.Euler(0f, 0f, 0f));

            _objectNow.transform.SetParent(transform);
            _objectNow.transform.SetAsFirstSibling(); // ������ ������
        }
    }


    /// <summary>
    /// ����� ���������
    /// </summary>
    /// <param name="OnOff">true= ���,false=����</param>
    private void ScaleProgress (bool OnOff)
    {
        if (OnOff == true)
        {
            _scaleProgress.SetActive(true);
            _scaleProgressUp.SetActive(true);
            float _progressResource = (float)_amtAddResource / (float)_amtRequiredResourceGoLvUp;
            { _scaleProgressUp.GetComponent<Image>().fillAmount = _progressResource; }
        }
        else
        {
            _scaleProgress.SetActive(false);
            _scaleProgressUp.SetActive(false);
        }

    }



    private void _timeScaleOff()
    {
        if (_amtAddResource <= 0)
        {
            ScaleProgress(false);
        }
        if (_scaleProgress.activeSelf == true)
        {
            Invoke("_timeScaleOff", 0.1f);
            _amtAddResource = _amtAddResource - 0.1f;
            ScaleProgress(true);
        }

    }

    private void SaveResources()
    {
        PlayerPrefs.SetInt("lvBed", _lvObjectNow);
        PlayerPrefs.SetInt("needResourcBed", _amtRequiredResourceGoLvUp);

        PlayerPrefs.Save();
    }

    private void LoadResouces()
    {
        if (loadResorces)
        {
            if (PlayerPrefs.HasKey("lvBed"))
            {
                _lvObjectNow = PlayerPrefs.GetInt("lvBed");
            }
            if (PlayerPrefs.HasKey("needResourcBed"))
            {
                _amtRequiredResourceGoLvUp = PlayerPrefs.GetInt("needResourcBed");
            }


        }
    }

}