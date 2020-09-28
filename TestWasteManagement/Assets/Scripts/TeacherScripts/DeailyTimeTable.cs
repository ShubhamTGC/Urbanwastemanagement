using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using SimpleSQL;

public class DeailyTimeTable : MonoBehaviour
{
    [HideInInspector] public List<string> Months = new List<string> { "January", "February", "March", "April","May","June","July","August","September","October","November","December" };
    public Text Date, Month;
    public List<InputField> events;
    public SimpleSQLManager dbmanager;
    public PopupView NativePopup;
    private DateTime todate;
    public DateTime CurrentdateTime;
    private string  eventdata;
    private int datecounter;
    void Start()
    {
        
    }
     void OnEnable()
    {
        datecounter = 0;
        CurrentDaySetup();
    }

    void CurrentDaySetup()
    {

        todate = DateTime.Today;
        Date.text = todate.Day.ToString();
        Month.text = Months[todate.Month-1];
        GetTeacherEvent(todate);
      
    }

    void GetTeacherEvent(DateTime date)
    {
        var tableLog = dbmanager.Table<TeacherEvent>().FirstOrDefault(x => x.Date == date.Day && x.Month == date.Month && x.Year == date.Year);
        if (tableLog != null)
        {
            string[] Eventmsg = tableLog.Event.Split("@"[0]);
            for (int a = 0; a < Eventmsg.Length; a++)
            {
                events[a].text = Eventmsg[a];
            }
        }
        else
        {
            events.ForEach(x =>
            {
                x.text = "No events...";
            });
        }
    }



    void Update()
    {
        
    }


    public void insertevent()
    {
       
        for(int a = 0; a < events.Count; a++)
        {
            if(events[a].text != "")
            {
                eventdata = eventdata + "@" + events[a].text;
            }
        }
    }

    public void Savedata()
    {
        var log = dbmanager.Table<TeacherEvent>().FirstOrDefault(x => x.Date == todate.Day && x.Month == todate.Month && x.Year == todate.Year);
        if(log == null)
        {
            TeacherEvent Log = new TeacherEvent
            {
                Date = CurrentdateTime.Day,
                Month = CurrentdateTime.Month,
                Year = CurrentdateTime.Year,
                Event = eventdata
            };
            dbmanager.Insert(Log);
        }
        else
        {
            log.Date = CurrentdateTime.Day;
            log.Month = CurrentdateTime.Month;
            log.Year = CurrentdateTime.Year;
            log.Event = eventdata;
            dbmanager.UpdateTable(log);
        }
    }


    public void NextDateEvent()
    {
        datecounter++;
     
        DateTime Updateeddate = todate.AddDays(datecounter);
        Date.text = Updateeddate.Day.ToString();
        Month.text = Months[Updateeddate.Month - 1];
        GetTeacherEvent(Updateeddate);
    }

    public void PreviousDateEvent()
    {
        datecounter--;
        DateTime Updateeddate = todate.AddDays(datecounter);
        Date.text = Updateeddate.Day.ToString();
        Month.text = Months[Updateeddate.Month - 1];
        GetTeacherEvent(Updateeddate);
    }



}
