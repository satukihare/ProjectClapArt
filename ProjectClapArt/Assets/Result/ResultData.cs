using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ResultData
{
    //noteの合計数
    public static int total_notes = 1;

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

    public static int bonus_score = 0;

    public static int bonus_max = 40;

    public static float bonus_rate
    {
        get
        {
            return (float)bonus_score / bonus_max;
        }
    }

    public static int voltage_score = 20;

    public static int voltage_max = 100;

    public static float voltage_rate
    {
        get
        {
            return (float)voltage_score / voltage_max;
        }
    }

    public static int rank
    {
        get
        {
            //TODO: 3種類目のスコアも計算する

            //合計スコア（0~3）
            float total = bonus_rate + score_rate + voltage_rate;
            //平均スコア（0~1）
            float average = total / 3;
            //0~1の平均スコアを0・1・2のランク値に変換する
            return (int)(average * 3); 
        }
    }
}
