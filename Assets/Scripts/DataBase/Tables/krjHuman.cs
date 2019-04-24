using System;

public class krjHuman : krjCommon
{
    public static Int64 lastRecid;
    /* временное упрощение таблицы чтобы отладить вывод на экран
    public krjGender gender { get; set; }
    public krjHeroClass heroClass { get; set; }
    public krjHeroAge ageLevel { get; set; }
    public int minCountSkill { get; set; }
    public int maxCountSkill { get; set; }
    */
    public string name { get; set; }
    public string gender { get; set; }
    public int x { get; set; }
    public int y { get; set; }

    public krjHuman(string _name, string _gender, int _x, int _y) : base()
    {
        name = _name;
        gender = _gender;
        x = _x;
        y = _y;
    }

    static krjHuman()
    {
        lastRecid = 0;
    }

    public override Int64 getNewRecId()
    {
        lastRecid++;
        return lastRecid;
    }
}
