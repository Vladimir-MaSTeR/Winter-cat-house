using System;

public class EventsResources
{
    public static Action<int, int, int> onClouthInBucket; // ����� | ������ ��. - �� ��������.
                                                          //         ������ ��. - ���� �������.
                                                          //         ������ ��. - ��������� ��� ������� ������ (0 = �������. 1 ���������)

    public static Action<int, int, int> onLogInBucket;   // ������ | ������ ��. - �� ��������.
                                                         //          ������ ��. - ���� �������.
                                                         //          ������ ��. - ��������� ��� ������� ������ (0 = �������. 1 ���������)   

    public static Action<int, int, int> onStoneInBucket;  // ������ | ������ ��. - �� ��������.
                                                          //          ������ ��. - ���� �������.
                                                          //          ������ ��. - ��������� ��� ������� ������ (0 = �������. 1 ���������)  

    public static Action<int, int, int> onNeilInBucket;  // ������ | ������ ��. - �� ��������.
                                                         //          ������ ��. - ���� �������.
                                                         //          ������ ��. - ��������� ��� ������� ������ (0 = �������. 1 ���������)

    public static Func<int, int> onGetCurentClouth; // ���������� ���������� ������� ����������� �����
    public static Func<int, int> onGetCurentLog;    // ���������� ���������� ������� ����������� ������
    public static Func<int, int> onGetCurentStone;  // ���������� ���������� ������� ����������� �����
    public static Func<int, int> onGetCurentNeil;   // ���������� ���������� ������� ����������� �������

}