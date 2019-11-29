using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ResultData
{
    //noteの合計数
    public static int total_notes = 0;

    //押したnoteの数(パーフェクトで1、グッドで0.7、ミスで0)
    public static float hit_notes = 0;

    //押せたnoteの割合
    public static float score_rate
    {
        get
        {
            return hit_notes / total_notes;
        }
    }
}