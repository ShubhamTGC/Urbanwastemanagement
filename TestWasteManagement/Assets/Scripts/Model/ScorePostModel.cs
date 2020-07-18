﻿
public class ScorePostModel

{
    public int UID { get; set; }
    public int OID { get; set; }
    public Log log { get; set; }
}

public class Log
{
    public int id_log { get; set; }
    public int id_user { get; set; }
    public int id_game_content { get; set; }
    public int score { get; set; }
    public int id_score_unit { get; set; }
    public int score_type { get; set; }
    public string score_unit { get; set; }
    public string status { get; set; }
    public string updated_date_time { get; set; }
    public int id_level { get; set; }
    public int id_org_game { get; set; }
    public int attempt_no { get; set; }
    public string timetaken_to_complete { get; set; }
    public int is_completed { get; set; }

}

